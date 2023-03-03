// Original code from https://github.com/Unity-Technologies/2d-extras/blob/layerbrush/Editor/Brushes/LayerBrush/LayerBrush.cs
// Licensed under https://github.com/Unity-Technologies/2d-extras/blob/master/LICENSE.md

using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GigaCreation.Tools.Tilemaps.Editor
{
    /// <summary>
    /// This Brush targets multiple Tilemaps linked through the same GridLayout.
    /// Use this as an example to edit multiple Tilemaps at once.
    /// </summary>
    [CustomGridBrush(true, false, false, "Layer Brush")]
    public class LayerBrush : GridBrush
    {
        internal GridBrush[] GridBrushes;
        internal GameObject[] BrushTargets;

        private GridLayout _cachedGridLayout;

        internal bool ValidateAndCacheBrushTargetsFromGridLayout(GridLayout gridLayout)
        {
            if (_cachedGridLayout == gridLayout)
            {
                return true;
            }

            if (GridBrushes == null)
            {
                return false;
            }

            Tilemap[] tilemaps = gridLayout.GetComponentsInChildren<Tilemap>();

            if (tilemaps.Length != GridBrushes.Length)
            {
                return false;
            }

            _cachedGridLayout = gridLayout;
            CacheBrushTargets(tilemaps);

            return true;
        }

        private void CacheBrushTargets(Tilemap[] tilemaps)
        {
            if ((BrushTargets == null) || (BrushTargets.Length != tilemaps.Length))
            {
                BrushTargets = new GameObject[tilemaps.Length];
            }

            for (var i = 0; i < tilemaps.Length; ++i)
            {
                BrushTargets[i] = tilemaps[i].gameObject;
            }
        }

        private void CacheGridLayout(GridLayout gridLayout)
        {
            if (_cachedGridLayout == gridLayout)
            {
                return;
            }

            Tilemap[] tilemaps = gridLayout.gameObject.GetComponentsInChildren<Tilemap>();

            if ((GridBrushes == null) || (GridBrushes.Length != tilemaps.Length))
            {
                GridBrushes = new GridBrush[tilemaps.Length];

                for (var i = 0; i < tilemaps.Length; ++i)
                {
                    GridBrushes[i] = CreateInstance<GridBrush>();
                }
            }
            else
            {
                foreach (GridBrush gridBrush in GridBrushes)
                {
                    gridBrush.Reset();
                }
            }

            _cachedGridLayout = gridLayout;
            CacheBrushTargets(tilemaps);
        }

        public override void Select(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
        {
            CacheGridLayout(gridLayout);

            for (var i = 0; i < GridBrushes.Length; ++i)
            {
                GridBrushes[i].Select(gridLayout, BrushTargets[i], position);
            }
        }

        public override void Move(GridLayout gridLayout, GameObject brushTarget, BoundsInt from, BoundsInt to)
        {
            CacheGridLayout(gridLayout);

            for (var i = 0; i < GridBrushes.Length; ++i)
            {
                GridBrushes[i].Move(gridLayout, BrushTargets[i], from, to);
            }
        }

        public override void MoveStart(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
        {
            CacheGridLayout(gridLayout);

            for (var i = 0; i < GridBrushes.Length; ++i)
            {
                GridBrushes[i].MoveStart(gridLayout, BrushTargets[i], position);
            }
        }

        public override void MoveEnd(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
        {
            CacheGridLayout(gridLayout);

            for (var i = 0; i < GridBrushes.Length; ++i)
            {
                GridBrushes[i].MoveEnd(gridLayout, BrushTargets[i], position);
            }
        }

        public override void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
        {
            CacheGridLayout(gridLayout);

            for (var i = 0; i < GridBrushes.Length; ++i)
            {
                GridBrushes[i].Paint(gridLayout, BrushTargets[i], position);
            }
        }

        public override void Erase(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
        {
            if (!ValidateAndCacheBrushTargetsFromGridLayout(gridLayout))
            {
                return;
            }

            for (var i = 0; i < GridBrushes.Length; ++i)
            {
                GridBrushes[i].Erase(gridLayout, BrushTargets[i], position);
            }
        }

        public override void Pick(
            GridLayout gridLayout, GameObject brushTarget, BoundsInt position, Vector3Int pickStart
        )
        {
            CacheGridLayout(gridLayout);

            for (var i = 0; i < GridBrushes.Length; ++i)
            {
                GridBrushes[i].Pick(gridLayout, BrushTargets[i], position, pickStart);
            }

            base.Pick(gridLayout, brushTarget, position, pickStart);
        }

        public override void FloodFill(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
        {
            if (!ValidateAndCacheBrushTargetsFromGridLayout(gridLayout))
            {
                return;
            }

            for (var i = 0; i < GridBrushes.Length; ++i)
            {
                GridBrushes[i].FloodFill(gridLayout, BrushTargets[i], position);
            }
        }

        public override void BoxFill(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
        {
            if (!ValidateAndCacheBrushTargetsFromGridLayout(gridLayout))
            {
                return;
            }

            for (var i = 0; i < GridBrushes.Length; ++i)
            {
                GridBrushes[i].BoxFill(gridLayout, BrushTargets[i], position);
            }
        }

        public override void Flip(FlipAxis flip, GridLayout.CellLayout layout)
        {
            if (GridBrushes == null)
            {
                return;
            }

            foreach (GridBrush gridBrush in GridBrushes)
            {
                gridBrush.Flip(flip, layout);
            }
        }

        public override void Rotate(RotationDirection direction, GridLayout.CellLayout layout)
        {
            if (GridBrushes == null)
            {
                return;
            }

            foreach (GridBrush gridBrush in GridBrushes)
            {
                gridBrush.Rotate(direction, layout);
            }
        }

        public override void ChangeZPosition(int change)
        {
            if (GridBrushes == null)
            {
                return;
            }

            foreach (GridBrush gridBrush in GridBrushes)
            {
                gridBrush.ChangeZPosition(change);
            }
        }

        public override void ResetZPosition()
        {
            if (GridBrushes == null)
            {
                return;
            }

            foreach (GridBrush gridBrush in GridBrushes)
            {
                gridBrush.ResetZPosition();
            }
        }
    }

    /// <summary>
    /// The Brush Editor for a Layer Brush.
    /// </summary>
    [CustomEditor(typeof(LayerBrush))]
    public class LayerBrushEditor : GridBrushEditor
    {
        private GridBrushEditor[] _editors;

        private LayerBrush LayerBrush => target as LayerBrush;

        private void CreateEditor()
        {
            if ((LayerBrush.GridBrushes == null) ||
                ((_editors != null) && (_editors.Length == LayerBrush.GridBrushes.Length)))
            {
                return;
            }

            _editors = new GridBrushEditor[LayerBrush.GridBrushes.Length];

            for (var i = 0; i < _editors.Length; ++i)
            {
                _editors[i] = CreateEditor(LayerBrush.GridBrushes[i]) as GridBrushEditor;
            }
        }

        private bool ValidatePreview(GridLayout gridLayout)
        {
            if ((_editors == null) || (LayerBrush.GridBrushes == null) || (LayerBrush.BrushTargets == null))
            {
                return false;
            }

            return LayerBrush.ValidateAndCacheBrushTargetsFromGridLayout(gridLayout is Tilemap tilemap
                ? tilemap.layoutGrid
                : gridLayout);
        }

        public override void RegisterUndo(GameObject brushTarget, GridBrushBase.Tool tool)
        {
            if ((LayerBrush.BrushTargets == null) || (LayerBrush.BrushTargets.Length == 0))
            {
                return;
            }

            int count = LayerBrush.BrushTargets.Length;
            var undoObjects = new Object[count * 2];

            for (var i = 0; i < LayerBrush.BrushTargets.Length; i++)
            {
                undoObjects[i] = LayerBrush.BrushTargets[i];
                undoObjects[i + count] = LayerBrush.BrushTargets[i].GetComponent<Tilemap>();
            }

            Undo.RegisterCompleteObjectUndo(undoObjects, tool.ToString());
        }

        public override void OnPaintSceneGUI(
            GridLayout gridLayout, GameObject brushTarget, BoundsInt position, GridBrushBase.Tool tool, bool executing
        )
        {
            CreateEditor();

            if (ValidatePreview(gridLayout))
            {
                for (var i = 0; i < _editors.Length; ++i)
                {
                    _editors[i].OnPaintSceneGUI(gridLayout, LayerBrush.BrushTargets[i], position, tool, executing);
                }
            }
            else
            {
                base.OnPaintSceneGUI(gridLayout, brushTarget, position, tool, executing);
            }
        }

        public override void PaintPreview(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
        {
            CreateEditor();

            if (!ValidatePreview(gridLayout))
            {
                return;
            }

            for (var i = 0; i < _editors.Length; ++i)
            {
                _editors[i].PaintPreview(gridLayout, LayerBrush.BrushTargets[i], position);
            }
        }

        public override void BoxFillPreview(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
        {
            CreateEditor();

            if (!ValidatePreview(gridLayout))
            {
                return;
            }

            for (var i = 0; i < _editors.Length; ++i)
            {
                _editors[i].BoxFillPreview(gridLayout, LayerBrush.BrushTargets[i], position);
            }
        }

        public override void FloodFillPreview(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
        {
            CreateEditor();

            if (!ValidatePreview(gridLayout))
            {
                return;
            }

            for (var i = 0; i < _editors.Length; ++i)
            {
                _editors[i].FloodFillPreview(gridLayout, LayerBrush.BrushTargets[i], position);
            }
        }

        public override void ClearPreview()
        {
            CreateEditor();

            if (_editors == null)
            {
                return;
            }

            foreach (GridBrushEditor editor in _editors)
            {
                editor.ClearPreview();
            }
        }
    }
}
