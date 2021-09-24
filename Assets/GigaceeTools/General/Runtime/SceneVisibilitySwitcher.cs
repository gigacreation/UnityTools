using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GigaceeTools
{
    public class SceneVisibilitySwitcher : MonoBehaviour
    {
        [SerializeField] private bool _visible;
        [SerializeField] private Target _target;

        private void OnValidate()
        {
            IEnumerable<GameObject> targetGameObjects;

            switch (_target)
            {
                case Target.Self:
                    targetGameObjects = new[] { gameObject };
                    break;

                case Target.ChildrenOneLevelDown:
                    targetGameObjects = transform.Cast<Transform>().Select(x => x.gameObject);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

#if UNITY_EDITOR
            foreach (GameObject go in targetGameObjects)
            {
                if (_visible)
                {
                    SceneVisibilityManager.instance.Show(go, true);
                }
                else
                {
                    SceneVisibilityManager.instance.Hide(go, true);
                }
            }
#endif
        }

        private enum Target
        {
            Self,
            ChildrenOneLevelDown
        }
    }
}
