// Original code from https://github.com/StompyRobot/SRF/blob/master/Scripts/UI/CopyLayoutElement.cs
// Licensed under https://github.com/StompyRobot/SRF/blob/master/LICENSE

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GigaCreation.Tools.Ui
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class CopiedLayoutElement : UIBehaviour, ILayoutElement
    {
        [SerializeField] private bool _shouldCopyMinWidth;
        [SerializeField] private RectTransform _copySourceOfMinWidth;

        [Space]
        [SerializeField] private bool _shouldCopyMinHeight;
        [SerializeField] private RectTransform _copySourceOfMinHeight;

        [Space]
        [SerializeField] private bool _shouldCopyPreferredWidth;
        [SerializeField] private RectTransform _copySourceOfPreferredWidth;

        [Space]
        [SerializeField] private bool _shouldCopyPreferredHeight;
        [SerializeField] private RectTransform _copySourceOfPreferredHeight;

        [Space]
        [SerializeField] private Vector2 _padding;

        private float _prevMinWidth;
        private float _prevMinHeight;
        private float _prevPreferredWidth;
        private float _prevPreferredHeight;

        public float minWidth
        {
            get
            {
                if (!_shouldCopyMinWidth || (_copySourceOfMinWidth == null) || !IsActive())
                {
                    return -1f;
                }

                // ReSharper disable once InvertIf
                if (_copySourceOfMinWidth == transform as RectTransform)
                {
                    Debug.LogWarning("コピー元に自身が設定されています。", gameObject);
                    return -1f;
                }

                float current = LayoutUtility.GetMinWidth(_copySourceOfMinWidth) + _padding.x * 2f;

                if (!Mathf.Approximately(_prevMinWidth, current))
                {
                    SetDirty();
                }

                _prevMinWidth = current;
                return current;
            }
        }

        public float minHeight
        {
            get
            {
                if (!_shouldCopyMinHeight || (_copySourceOfMinHeight == null) || !IsActive())
                {
                    return -1f;
                }

                // ReSharper disable once InvertIf
                if (_copySourceOfMinHeight == transform as RectTransform)
                {
                    Debug.LogWarning("コピー元に自身が設定されています。", gameObject);
                    return -1f;
                }

                float current = LayoutUtility.GetMinHeight(_copySourceOfMinHeight) + _padding.y * 2f;

                if (!Mathf.Approximately(_prevMinHeight, current))
                {
                    SetDirty();
                }

                _prevMinHeight = current;
                return current;
            }
        }

        public float preferredWidth
        {
            get
            {
                if (!_shouldCopyPreferredWidth || (_copySourceOfPreferredWidth == null) || !IsActive())
                {
                    return -1f;
                }

                // ReSharper disable once InvertIf
                if (_copySourceOfPreferredWidth == transform as RectTransform)
                {
                    Debug.LogWarning("コピー元に自身が設定されています。", gameObject);
                    return -1f;
                }

                float current = LayoutUtility.GetPreferredWidth(_copySourceOfPreferredWidth) + _padding.x * 2f;

                if (!Mathf.Approximately(_prevPreferredWidth, current))
                {
                    SetDirty();
                }

                _prevPreferredWidth = current;
                return current;
            }
        }

        public float preferredHeight
        {
            get
            {
                if (!_shouldCopyPreferredHeight || (_copySourceOfPreferredHeight == null) || !IsActive())
                {
                    return -1f;
                }

                // ReSharper disable once InvertIf
                if (_copySourceOfPreferredHeight == transform as RectTransform)
                {
                    Debug.LogWarning("コピー元に自身が設定されています。", gameObject);
                    return -1f;
                }

                float current = LayoutUtility.GetPreferredHeight(_copySourceOfPreferredHeight) + _padding.y * 2f;

                if (!Mathf.Approximately(_prevPreferredHeight, current))
                {
                    SetDirty();
                }

                _prevPreferredHeight = current;
                return current;
            }
        }

        public float flexibleWidth => -1f;
        public float flexibleHeight => -1f;
        public int layoutPriority => 2;

        protected override void OnEnable()
        {
            base.OnEnable();
            SetDirty();
        }

        protected override void OnDisable()
        {
            SetDirty();
            base.OnDisable();
        }

        protected override void OnBeforeTransformParentChanged()
        {
            SetDirty();
        }

        protected override void OnTransformParentChanged()
        {
            SetDirty();
        }

        protected override void OnDidApplyAnimationProperties()
        {
            SetDirty();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            SetDirty();
        }
#endif

        public void CalculateLayoutInputHorizontal()
        {
        }

        public void CalculateLayoutInputVertical()
        {
        }

        private void SetDirty()
        {
            if (!IsActive())
            {
                return;
            }

            LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
        }
    }
}
