// Original code from https://gist.github.com/yasirkula/391fa12bc173acdf5ac48c466f180708
// Licensed under https://choosealicense.com/licenses/0bsd/

using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;
#if UNITY_2017_4 || UNITY_2018_2_OR_NEWER
using UnityEngine.U2D;
#endif

namespace GigaceeTools
{
    // Credit: https://bitbucket.org/Unity-Technologies/ui/src/2018.4/UnityEngine.UI/UI/Core/Image.cs
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    [RequireComponent(typeof(CanvasRenderer))]
    [AddComponentMenu("UI/Sliced Filled Image", 11)]
    public class SlicedFilledImage :
        MaskableGraphic,
        ISerializationCallbackReceiver,
        ILayoutElement,
        ICanvasRaycastFilter
    {
        public enum FillDirection
        {
            Right = 0,
            Left = 1,
            Up = 2,
            Down = 3
        }

        private static readonly Vector3[] s_vertices = new Vector3[4];
        private static readonly Vector2[] s_uVs = new Vector2[4];
        private static readonly Vector2[] s_slicedVertices = new Vector2[4];
        private static readonly Vector2[] s_slicedUVs = new Vector2[4];

        [SerializeField] private Sprite _sprite;
        [SerializeField] private FillDirection _fillDirection;
        [SerializeField] [Range(0, 1)] private float _fillAmount = 1f;
        [SerializeField] private bool _fillCenter = true;
        [SerializeField] private float _pixelsPerUnitMultiplier = 1f;

        [SerializeField] private float _alphaHitTestMinimumThreshold;

        // Whether this is being tracked for Atlas Binding
        private bool _tracked;

        [NonSerialized] private Sprite _overrideSprite;

        public Sprite Sprite
        {
            get => _sprite;
            set
            {
                if (!SetPropertyUtility.SetClass(ref _sprite, value))
                {
                    return;
                }

                SetAllDirty();
                TrackImage();
            }
        }

        public FillDirection Direction
        {
            get => _fillDirection;
            set
            {
                if (SetPropertyUtility.SetStruct(ref _fillDirection, value))
                {
                    SetVerticesDirty();
                }
            }
        }

        public float FillAmount
        {
            get => _fillAmount;
            set
            {
                if (SetPropertyUtility.SetStruct(ref _fillAmount, Mathf.Clamp01(value)))
                {
                    SetVerticesDirty();
                }
            }
        }

        public bool FillCenter
        {
            get => _fillCenter;
            set
            {
                if (SetPropertyUtility.SetStruct(ref _fillCenter, value))
                {
                    SetVerticesDirty();
                }
            }
        }

        public float PixelsPerUnitMultiplier
        {
            get => _pixelsPerUnitMultiplier;
            set => _pixelsPerUnitMultiplier = Mathf.Max(0.01f, value);
        }

        private float PixelsPerUnit
        {
            get
            {
                var spritePixelsPerUnit = 100f;

                if (ActiveSprite)
                {
                    spritePixelsPerUnit = ActiveSprite.pixelsPerUnit;
                }

                var referencePixelsPerUnit = 100f;

                if (canvas)
                {
                    referencePixelsPerUnit = canvas.referencePixelsPerUnit;
                }

                return _pixelsPerUnitMultiplier * spritePixelsPerUnit / referencePixelsPerUnit;
            }
        }

        public Sprite OverrideSprite
        {
            get => ActiveSprite;
            set
            {
                if (!SetPropertyUtility.SetClass(ref _overrideSprite, value))
                {
                    return;
                }

                SetAllDirty();
                TrackImage();
            }
        }

        private Sprite ActiveSprite => _overrideSprite != null ? _overrideSprite : _sprite;

        public override Texture mainTexture
        {
            get
            {
                if (ActiveSprite != null)
                {
                    return ActiveSprite.texture;
                }

                return (material != null) && (material.mainTexture != null) ? material.mainTexture : s_WhiteTexture;
            }
        }

        private bool HasBorder
        {
            get
            {
                if (ActiveSprite == null)
                {
                    return false;
                }

                Vector4 v = ActiveSprite.border;

                return v.sqrMagnitude > 0f;
            }
        }

        public override Material material
        {
            get
            {
                if (m_Material != null)
                {
                    return m_Material;
                }

                // ReSharper disable once InvertIf
                if (ActiveSprite && (ActiveSprite.associatedAlphaSplitTexture != null))
                {
#if UNITY_EDITOR
                    if (Application.isPlaying)
#endif
                    {
                        return Image.defaultETC1GraphicMaterial;
                    }
                }

                return defaultMaterial;
            }
            set { base.material = value; }
        }

        private float AlphaHitTestMinimumThreshold => _alphaHitTestMinimumThreshold;

        protected SlicedFilledImage()
        {
            useLegacyMeshGeneration = false;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            TrackImage();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (_tracked)
            {
                UnTrackImage();
            }
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            _pixelsPerUnitMultiplier = Mathf.Max(0.01f, _pixelsPerUnitMultiplier);
        }
#endif

        bool ICanvasRaycastFilter.IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            if (AlphaHitTestMinimumThreshold <= 0f)
            {
                return true;
            }

            if (AlphaHitTestMinimumThreshold > 1f)
            {
                return false;
            }

            if (ActiveSprite == null)
            {
                return true;
            }

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    rectTransform,
                    screenPoint,
                    eventCamera,
                    out Vector2 local
                ))
            {
                return false;
            }

            Rect rect = GetPixelAdjustedRect();

            // Convert to have lower left corner as reference point.
            Vector2 pivot = rectTransform.pivot;
            local.x += pivot.x * rect.width;
            local.y += pivot.y * rect.height;

            Rect spriteRect = ActiveSprite.rect;
            Vector4 border = ActiveSprite.border;
            Vector4 adjustedBorder = GetAdjustedBorders(border / PixelsPerUnit, rect);

            for (var i = 0; i < 2; i++)
            {
                if (local[i] <= adjustedBorder[i])
                {
                    continue;
                }

                if (rect.size[i] - local[i] <= adjustedBorder[i + 2])
                {
                    local[i] -= rect.size[i] - spriteRect.size[i];
                    continue;
                }

                float lerp = Mathf.InverseLerp(adjustedBorder[i], rect.size[i] - adjustedBorder[i + 2], local[i]);
                local[i] = Mathf.Lerp(border[i], spriteRect.size[i] - border[i + 2], lerp);
            }

            // Normalize local coordinates.
            Rect textureRect = ActiveSprite.textureRect;
            var normalized = new Vector2(local.x / textureRect.width, local.y / textureRect.height);

            // Convert to texture space.
            float x = Mathf.Lerp(textureRect.x, textureRect.xMax, normalized.x) / ActiveSprite.texture.width;
            float y = Mathf.Lerp(textureRect.y, textureRect.yMax, normalized.y) / ActiveSprite.texture.height;

            switch (_fillDirection)
            {
                case FillDirection.Right:
                    if (x > _fillAmount)
                    {
                        return false;
                    }

                    break;

                case FillDirection.Left:
                    if (1f - x > _fillAmount)
                    {
                        return false;
                    }

                    break;

                case FillDirection.Up:
                    if (y > _fillAmount)
                    {
                        return false;
                    }

                    break;

                case FillDirection.Down:
                    if (1f - y > _fillAmount)
                    {
                        return false;
                    }

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            try
            {
                return ActiveSprite.texture.GetPixelBilinear(x, y).a >= AlphaHitTestMinimumThreshold;
            }
            catch (UnityException e)
            {
                Debug.LogError(
                    "Using alphaHitTestMinimumThreshold greater than 0 on Image whose sprite texture cannot be read. "
                    + e.Message
                    + " Also make sure to disable sprite packing for this sprite.",
                    this
                );

                return true;
            }
        }

        int ILayoutElement.layoutPriority => 0;

        float ILayoutElement.minWidth => 0f;

        float ILayoutElement.minHeight => 0f;

        float ILayoutElement.flexibleWidth => -1f;

        float ILayoutElement.flexibleHeight => -1f;

        float ILayoutElement.preferredWidth
        {
            get
            {
                if (ActiveSprite == null)
                {
                    return 0f;
                }

                return DataUtility.GetMinSize(ActiveSprite).x / PixelsPerUnit;
            }
        }

        float ILayoutElement.preferredHeight
        {
            get
            {
                if (ActiveSprite == null)
                {
                    return 0f;
                }

                return DataUtility.GetMinSize(ActiveSprite).y / PixelsPerUnit;
            }
        }

        void ILayoutElement.CalculateLayoutInputHorizontal()
        {
        }

        void ILayoutElement.CalculateLayoutInputVertical()
        {
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            _fillAmount = Mathf.Clamp01(_fillAmount);
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            if (ActiveSprite == null)
            {
                base.OnPopulateMesh(vh);
                return;
            }

            GenerateSlicedFilledSprite(vh);
        }

        /// <summary>
        /// Update the renderer's material.
        /// </summary>
        protected override void UpdateMaterial()
        {
            base.UpdateMaterial();

            // Check if this sprite has an associated alpha texture (generated when splitting RGBA = RGB + A as two textures without alpha)
            if (ActiveSprite == null)
            {
                canvasRenderer.SetAlphaTexture(null);

                return;
            }

            Texture2D alphaTex = ActiveSprite.associatedAlphaSplitTexture;

            if (alphaTex != null)
            {
                canvasRenderer.SetAlphaTexture(alphaTex);
            }
        }

        private void GenerateSlicedFilledSprite(VertexHelper vh)
        {
            vh.Clear();

            if (_fillAmount < 0.001f)
            {
                return;
            }

            Rect rect = GetPixelAdjustedRect();
            Vector4 outer = DataUtility.GetOuterUV(ActiveSprite);
            Vector4 padding = DataUtility.GetPadding(ActiveSprite);

            if (!HasBorder)
            {
                Vector2 size = ActiveSprite.rect.size;

                int spriteW = Mathf.RoundToInt(size.x);
                int spriteH = Mathf.RoundToInt(size.y);

                // Image's dimensions used for drawing. X = left, Y = bottom, Z = right, W = top.
                var vertices = new Vector4(
                    rect.x + rect.width * (padding.x / spriteW),
                    rect.y + rect.height * (padding.y / spriteH),
                    rect.x + rect.width * ((spriteW - padding.z) / spriteW),
                    rect.y + rect.height * ((spriteH - padding.w) / spriteH)
                );

                GenerateFilledSprite(vh, vertices, outer, _fillAmount);
                return;
            }

            Vector4 inner = DataUtility.GetInnerUV(ActiveSprite);
            Vector4 border = GetAdjustedBorders(ActiveSprite.border / PixelsPerUnit, rect);

            padding /= PixelsPerUnit;

            s_slicedVertices[0] = new Vector2(padding.x, padding.y);
            s_slicedVertices[3] = new Vector2(rect.width - padding.z, rect.height - padding.w);

            s_slicedVertices[1].x = border.x;
            s_slicedVertices[1].y = border.y;

            s_slicedVertices[2].x = rect.width - border.z;
            s_slicedVertices[2].y = rect.height - border.w;

            for (var i = 0; i < 4; ++i)
            {
                s_slicedVertices[i].x += rect.x;
                s_slicedVertices[i].y += rect.y;
            }

            s_slicedUVs[0] = new Vector2(outer.x, outer.y);
            s_slicedUVs[1] = new Vector2(inner.x, inner.y);
            s_slicedUVs[2] = new Vector2(inner.z, inner.w);
            s_slicedUVs[3] = new Vector2(outer.z, outer.w);

            float rectStartPos;
            float _1OverTotalSize;

            if ((_fillDirection == FillDirection.Left) || (_fillDirection == FillDirection.Right))
            {
                rectStartPos = s_slicedVertices[0].x;

                float totalSize = s_slicedVertices[3].x - s_slicedVertices[0].x;
                _1OverTotalSize = totalSize > 0f ? 1f / totalSize : 1f;
            }
            else
            {
                rectStartPos = s_slicedVertices[0].y;

                float totalSize = s_slicedVertices[3].y - s_slicedVertices[0].y;
                _1OverTotalSize = totalSize > 0f ? 1f / totalSize : 1f;
            }

            for (var x = 0; x < 3; x++)
            {
                int x2 = x + 1;

                for (var y = 0; y < 3; y++)
                {
                    if (!_fillCenter && (x == 1) && (y == 1))
                    {
                        continue;
                    }

                    int y2 = y + 1;

                    float sliceStart, sliceEnd;

                    switch (_fillDirection)
                    {
                        case FillDirection.Right:
                            sliceStart = (s_slicedVertices[x].x - rectStartPos) * _1OverTotalSize;
                            sliceEnd = (s_slicedVertices[x2].x - rectStartPos) * _1OverTotalSize;
                            break;

                        case FillDirection.Up:
                            sliceStart = (s_slicedVertices[y].y - rectStartPos) * _1OverTotalSize;
                            sliceEnd = (s_slicedVertices[y2].y - rectStartPos) * _1OverTotalSize;
                            break;

                        case FillDirection.Left:
                            sliceStart = 1f - (s_slicedVertices[x2].x - rectStartPos) * _1OverTotalSize;
                            sliceEnd = 1f - (s_slicedVertices[x].x - rectStartPos) * _1OverTotalSize;
                            break;

                        case FillDirection.Down:
                            sliceStart = 1f - (s_slicedVertices[y2].y - rectStartPos) * _1OverTotalSize;
                            sliceEnd = 1f - (s_slicedVertices[y].y - rectStartPos) * _1OverTotalSize;
                            break;

                        default: // Just there to get rid of the "Use of unassigned local variable" compiler error
                            sliceStart = sliceEnd = 0f;
                            break;
                    }

                    if (sliceStart >= _fillAmount)
                    {
                        continue;
                    }

                    var vertices = new Vector4(
                        s_slicedVertices[x].x,
                        s_slicedVertices[y].y,
                        s_slicedVertices[x2].x,
                        s_slicedVertices[y2].y
                    );

                    var uvs = new Vector4(s_slicedUVs[x].x, s_slicedUVs[y].y, s_slicedUVs[x2].x, s_slicedUVs[y2].y);
                    float fillAmount = (_fillAmount - sliceStart) / (sliceEnd - sliceStart);

                    GenerateFilledSprite(vh, vertices, uvs, fillAmount);
                }
            }
        }

        private Vector4 GetAdjustedBorders(Vector4 border, Rect adjustedRect)
        {
            Rect originalRect = rectTransform.rect;

            for (var axis = 0; axis <= 1; axis++)
            {
                float borderScaleRatio;

                // The adjusted rect (adjusted for pixel correctness) may be slightly larger than the original rect.
                // Adjust the border to match the adjustedRect to avoid small gaps between borders (case 833201).
                if (originalRect.size[axis] != 0f)
                {
                    borderScaleRatio = adjustedRect.size[axis] / originalRect.size[axis];
                    border[axis] *= borderScaleRatio;
                    border[axis + 2] *= borderScaleRatio;
                }

                // If the rect is smaller than the combined borders, then there's not room for the borders at their normal size.
                // In order to avoid artefacts with overlapping borders, we scale the borders down to fit.
                float combinedBorders = border[axis] + border[axis + 2];

                if (!(adjustedRect.size[axis] < combinedBorders) || Mathf.Approximately(combinedBorders, 0f))
                {
                    continue;
                }

                borderScaleRatio = adjustedRect.size[axis] / combinedBorders;
                border[axis] *= borderScaleRatio;
                border[axis + 2] *= borderScaleRatio;
            }

            return border;
        }

        private void GenerateFilledSprite(VertexHelper vh, Vector4 vertices, Vector4 uvs, float fillAmount)
        {
            if (_fillAmount < 0.001f)
            {
                return;
            }

            float uvLeft = uvs.x;
            float uvBottom = uvs.y;
            float uvRight = uvs.z;
            float uvTop = uvs.w;

            if (fillAmount < 1f)
            {
                switch (_fillDirection)
                {
                    case FillDirection.Left:
                        vertices.x = vertices.z - (vertices.z - vertices.x) * fillAmount;
                        uvLeft = uvRight - (uvRight - uvLeft) * fillAmount;
                        break;

                    case FillDirection.Right:
                        vertices.z = vertices.x + (vertices.z - vertices.x) * fillAmount;
                        uvRight = uvLeft + (uvRight - uvLeft) * fillAmount;
                        break;

                    case FillDirection.Up:
                        break;

                    case FillDirection.Down:
                        vertices.y = vertices.w - (vertices.w - vertices.y) * fillAmount;
                        uvBottom = uvTop - (uvTop - uvBottom) * fillAmount;
                        break;

                    default:
                        vertices.w = vertices.y + (vertices.w - vertices.y) * fillAmount;
                        uvTop = uvBottom + (uvTop - uvBottom) * fillAmount;
                        break;
                }
            }

            s_vertices[0] = new Vector3(vertices.x, vertices.y);
            s_vertices[1] = new Vector3(vertices.x, vertices.w);
            s_vertices[2] = new Vector3(vertices.z, vertices.w);
            s_vertices[3] = new Vector3(vertices.z, vertices.y);

            s_uVs[0] = new Vector2(uvLeft, uvBottom);
            s_uVs[1] = new Vector2(uvLeft, uvTop);
            s_uVs[2] = new Vector2(uvRight, uvTop);
            s_uVs[3] = new Vector2(uvRight, uvBottom);

            int startIndex = vh.currentVertCount;

            for (var i = 0; i < 4; i++)
            {
                vh.AddVert(s_vertices[i], color, s_uVs[i]);
            }

            vh.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
            vh.AddTriangle(startIndex + 2, startIndex + 3, startIndex);
        }

        private void TrackImage()
        {
            if ((ActiveSprite == null) || (ActiveSprite.texture != null))
            {
                return;
            }

#if UNITY_2017_4 || UNITY_2018_2_OR_NEWER
            if (!s_initialized)
            {
                SpriteAtlasManager.atlasRegistered += RebuildImage;
                s_initialized = true;
            }

            s_trackedTexturelessImages.Add(this);
#endif

            _tracked = true;
        }

        private void UnTrackImage()
        {
#if UNITY_2017_4 || UNITY_2018_2_OR_NEWER
            s_trackedTexturelessImages.Remove(this);
#endif

            _tracked = false;
        }

#if UNITY_2017_4 || UNITY_2018_2_OR_NEWER
        private static void RebuildImage(SpriteAtlas spriteAtlas)
        {
            for (int i = s_trackedTexturelessImages.Count - 1; i >= 0; i--)
            {
                SlicedFilledImage image = s_trackedTexturelessImages[i];

                if (!spriteAtlas.CanBindTo(image.ActiveSprite))
                {
                    continue;
                }

                image.SetAllDirty();
                s_trackedTexturelessImages.RemoveAt(i);
            }
        }
#endif

        private static class SetPropertyUtility
        {
            public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
            {
                if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
                {
                    return false;
                }

                currentValue = newValue;

                return true;
            }

            public static bool SetClass<T>(ref T currentValue, T newValue) where T : class
            {
                if (((currentValue == null) && (newValue == null)) ||
                    ((currentValue != null) && currentValue.Equals(newValue)))
                {
                    return false;
                }

                currentValue = newValue;

                return true;
            }
        }

#if UNITY_2017_4 || UNITY_2018_2_OR_NEWER
        private static readonly List<SlicedFilledImage> s_trackedTexturelessImages = new List<SlicedFilledImage>();
        private static bool s_initialized;
#endif
    }
}
