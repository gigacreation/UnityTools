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
        [SerializeField] private TargetKind _targetKind;
        [SerializeField] private bool _visible;
        [SerializeField] private Target[] _selections;

        private void OnValidate()
        {
            IEnumerable<Target> targets;

            switch (_targetKind)
            {
                case TargetKind.Self:
                    targets = new[]
                    {
                        new Target
                        {
                            GameObject = gameObject,
                            Visible = _visible
                        }
                    };

                    break;

                case TargetKind.ChildrenOneLevelDown:
                    targets = transform
                        .Cast<Transform>()
                        .Select(x => new Target
                        {
                            GameObject = x.gameObject,
                            Visible = _visible
                        });

                    break;

                case TargetKind.Selections:
                    targets = _selections;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            foreach (Target target in targets)
            {
                if (target.Visible)
                {
#if UNITY_EDITOR
                    SceneVisibilityManager.instance.Show(target.GameObject, true);
#endif
                }
                else
                {
#if UNITY_EDITOR
                    SceneVisibilityManager.instance.Hide(target.GameObject, true);
#endif
                }
            }
        }

        private enum TargetKind
        {
            Self,
            ChildrenOneLevelDown,
            Selections
        }

        [Serializable]
        private class Target
        {
            public GameObject GameObject;
            public bool Visible;
        }
    }
}
