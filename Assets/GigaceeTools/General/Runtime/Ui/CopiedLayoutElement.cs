// Original code from https://github.com/StompyRobot/SRF/blob/master/Scripts/UI/CopyLayoutElement.cs
// Licensed under https://github.com/StompyRobot/SRF/blob/master/LICENSE

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GigaceeTools
{
    [ExecuteAlways, RequireComponent(typeof(RectTransform))]
    public class CopiedLayoutElement : UIBehaviour, ILayoutElement
    {
        [SerializeField] private bool _copyMinWidth;
        [SerializeField] private RectTransform _copySourceOfMinWidth;
        [SerializeField] private bool _copyMinHeight;
        [SerializeField] private RectTransform _copySourceOfMinHeight;
        [SerializeField] private bool _copyPreferredWidth;
        [SerializeField] private RectTransform _copySourceOfPreferredWidth;
        [SerializeField] private bool _copyPreferredHeight;
        [SerializeField] private RectTransform _copySourceOfPreferredHeight;

        public float minWidth
        {
            get
            {
                if (!_copyMinWidth || (_copySourceOfMinWidth == null) || !IsActive())
                {
                    return -1f;
                }

                return LayoutUtility.GetMinWidth(_copySourceOfMinWidth);
            }
        }

        public float minHeight
        {
            get
            {
                if (!_copyMinHeight || (_copySourceOfMinHeight == null) || !IsActive())
                {
                    return -1f;
                }

                return LayoutUtility.GetMinHeight(_copySourceOfMinHeight);
            }
        }

        public float preferredWidth
        {
            get
            {
                if (!_copyPreferredWidth || (_copySourceOfPreferredWidth == null) || !IsActive())
                {
                    return -1f;
                }

                return LayoutUtility.GetPreferredWidth(_copySourceOfPreferredWidth);
            }
        }

        public float preferredHeight
        {
            get
            {
                if (!_copyPreferredHeight || (_copySourceOfPreferredHeight == null) || !IsActive())
                {
                    return -1f;
                }

                return LayoutUtility.GetPreferredHeight(_copySourceOfPreferredHeight);
            }
        }

        public int layoutPriority => 2;
        public float flexibleHeight => -1f;
        public float flexibleWidth => -1f;

        public void CalculateLayoutInputHorizontal()
        {
        }

        public void CalculateLayoutInputVertical()
        {
        }
    }
}
