using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GigaCreation.Tools
{
    // TODO: _targetKind が Selections でないときは _selections を非表示にする
    public class SetActiveOnAwake : MonoBehaviour
    {
        [SerializeField] private TargetKind _targetKind;
        [SerializeField] private bool _active = true;
        [SerializeField] private Target[] _selections;

        private void Awake()
        {
            IEnumerable<Target> targets = _targetKind switch
            {
                TargetKind.AllChildren => GetComponentsInChildren<Transform>(true)
                    .Select(x => new Target { GameObject = x.gameObject, Active = _active }),

                TargetKind.ChildrenOneLevelDown => transform
                    .Cast<Transform>()
                    .Select(x => new Target { GameObject = x.gameObject, Active = _active }),

                TargetKind.Selections => _selections,

                _ => throw new ArgumentOutOfRangeException()
            };

            foreach (Target target in targets)
            {
                if (!target.GameObject)
                {
                    continue;
                }

                target.GameObject.SetActive(target.Active);
            }
        }

        private enum TargetKind
        {
            AllChildren,
            ChildrenOneLevelDown,
            Selections
        }

        [Serializable]
        private class Target
        {
            public GameObject GameObject;
            public bool Active;
        }
    }
}
