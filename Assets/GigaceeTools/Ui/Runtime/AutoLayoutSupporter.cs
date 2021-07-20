using System.Collections;
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

        private bool _isDestroying;

        private void OnDestroy()
        {
            _isDestroying = true;
        }

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
            if (_isDestroying)
            {
                return;
            }

            if (Application.isEditor && !Application.isPlaying)
            {
                RebuildLayoutOnEdit();
                return;
            }

            StartCoroutine(RebuildLayoutAtRuntime());
        }

        private void RebuildLayoutOnEdit()
        {
            ComponentHelper.EnableOrDisableComponentIfExists(_contentSizeFitter, true);
            ComponentHelper.EnableOrDisableComponentIfExists(_layoutGroup, true);

            LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);

            ComponentHelper.EnableOrDisableComponentIfExists(_contentSizeFitter, false);
            ComponentHelper.EnableOrDisableComponentIfExists(_layoutGroup, false);
        }

        private IEnumerator RebuildLayoutAtRuntime()
        {
            ComponentHelper.EnableOrDisableComponentIfExists(_contentSizeFitter, true);
            ComponentHelper.EnableOrDisableComponentIfExists(_layoutGroup, true);

            LayoutRebuilder.MarkLayoutForRebuild(_rectTransform);

            yield return new WaitForEndOfFrame();

            ComponentHelper.EnableOrDisableComponentIfExists(_contentSizeFitter, false);
            ComponentHelper.EnableOrDisableComponentIfExists(_layoutGroup, false);
        }
    }
}
