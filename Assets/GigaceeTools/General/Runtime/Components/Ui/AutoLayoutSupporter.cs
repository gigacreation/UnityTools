using UnityEngine;
using UnityEngine.UI;

namespace GigaceeTools
{
    [RequireComponent(typeof(RectTransform))]
    public class AutoLayoutSupporter : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private ContentSizeFitter _contentSizeFitter;
        [SerializeField] private LayoutGroup _layoutGroup;

        private void Reset()
        {
            _rectTransform = transform as RectTransform;
            _contentSizeFitter = GetComponent<ContentSizeFitter>();
            _layoutGroup = GetComponent<LayoutGroup>();

            ComponentHelper.EnableOrDisableComponentIfExists(_contentSizeFitter, false);
            ComponentHelper.EnableOrDisableComponentIfExists(_layoutGroup, false);
        }

        public void RebuildLayout()
        {
            ComponentHelper.EnableOrDisableComponentIfExists(_contentSizeFitter, true);
            ComponentHelper.EnableOrDisableComponentIfExists(_layoutGroup, true);

            LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);

            ComponentHelper.EnableOrDisableComponentIfExists(_contentSizeFitter, false);
            ComponentHelper.EnableOrDisableComponentIfExists(_layoutGroup, false);
        }
    }
}
