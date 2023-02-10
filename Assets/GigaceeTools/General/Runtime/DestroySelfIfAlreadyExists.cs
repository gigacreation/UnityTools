using System.Linq;
using UnityEngine;

namespace GigaceeTools
{
    public class DestroySelfIfAlreadyExists : MonoBehaviour
    {
        [SerializeField] private Component _targetComponent;
        [SerializeField] private DestroyTarget _destroyTarget;

        private bool WillBeDestroyed { get; set; }

        private void Awake()
        {
            if (_targetComponent == null)
            {
                return;
            }

            Object[] objectsOfSameTypeExceptSelf = FindObjectsOfType(_targetComponent.GetType(), true)
                .Where(obj => (obj != _targetComponent)
                    && obj is Component c
                    && !c.GetComponent<DestroySelfIfAlreadyExists>().WillBeDestroyed)
                .ToArray();

            if (objectsOfSameTypeExceptSelf.Length == 0)
            {
                return;
            }

            WillBeDestroyed = true;

            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (_destroyTarget)
            {
                case DestroyTarget.Component:
                    Destroy(_targetComponent);
                    break;

                case DestroyTarget.GameObject:
                    Destroy(_targetComponent.gameObject);
                    break;
            }
        }

        private void OnValidate()
        {
            if ((_targetComponent == null) || (_targetComponent.gameObject == gameObject))
            {
                return;
            }

            Debug.LogWarning("自身と同じ GameObject にアタッチされたコンポーネントを指定してください。", gameObject);
            _targetComponent = null;
        }

        private enum DestroyTarget
        {
            Component,
            GameObject
        }
    }
}
