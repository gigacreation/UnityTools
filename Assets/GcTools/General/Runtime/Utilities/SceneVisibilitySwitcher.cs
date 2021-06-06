using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GcTools
{
    public class SceneVisibilitySwitcher : MonoBehaviour
    {
        [SerializeField] private bool _visible;

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (_visible)
            {
                SceneVisibilityManager.instance.Show(gameObject, true);
            }
            else
            {
                SceneVisibilityManager.instance.Hide(gameObject, true);
            }
#endif
        }
    }
}
