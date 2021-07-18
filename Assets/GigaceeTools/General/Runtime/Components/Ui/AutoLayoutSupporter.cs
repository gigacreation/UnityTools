using System.Collections;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

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
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                RebuildLayoutOnEdit();
                return;
            }
#endif

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
