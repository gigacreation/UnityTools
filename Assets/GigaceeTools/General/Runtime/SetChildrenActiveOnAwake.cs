using System;
using System.Linq;
using UnityEngine;

namespace GigaceeTools
{
    public class SetChildrenActiveOnAwake : MonoBehaviour
    {
        [SerializeField] private bool _shouldBeActive = true;
        [SerializeField] private Target _target;

        private void Awake()
        {
            switch (_target)
            {
                case Target.All:
                    foreach (Transform child in GetComponentsInChildren<Transform>(true))
                    {
                        child.gameObject.SetActive(_shouldBeActive);
                    }

                    break;

                case Target.OneLevelDownOnly:
                    foreach (Transform child in transform.Cast<Transform>())
                    {
                        child.gameObject.SetActive(_shouldBeActive);
                    }

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private enum Target
        {
            All,
            OneLevelDownOnly
        }
    }
}
