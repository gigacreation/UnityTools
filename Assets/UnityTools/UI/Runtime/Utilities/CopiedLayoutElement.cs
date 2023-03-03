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

                return LayoutUtility.GetMinWidth(_copySourceOfMinWidth) + _padding.x * 2f;
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

                return LayoutUtility.GetMinHeight(_copySourceOfMinHeight) + _padding.y * 2f;
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

                return LayoutUtility.GetPreferredWidth(_copySourceOfPreferredWidth) + _padding.x * 2f;
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

                return LayoutUtility.GetPreferredHeight(_copySourceOfPreferredHeight) + _padding.y * 2f;
            }
        }

        public int layoutPriority => 2;
        public float flexibleHeight => -1f;
        public float flexibleWidth => -1f;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
        }
#endif

        public void CalculateLayoutInputHorizontal()
        {
        }

        public void CalculateLayoutInputVertical()
        {
        }
    }
}
