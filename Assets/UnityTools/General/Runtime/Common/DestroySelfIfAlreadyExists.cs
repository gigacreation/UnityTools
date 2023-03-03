using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GigaCreation.Tools.General
{
    public class DestroySelfIfAlreadyExists : MonoBehaviour
    {
        /// <summary>
        /// 自身と同じ GameObject にアタッチされたこのコンポーネントと同じコンポーネントをシーン内から検索し、存在していた場合に自身を破棄します。
        /// </summary>
        [SerializeField] private Component _targetComponent;

        /// <summary>
        /// コンポーネントのみ削除するか、GameObject ごと削除するかを設定します。
        /// </summary>
        [SerializeField] private DestroyTarget _destroyTarget;

        private bool WillBeDestroyed { get; set; }

        private void Awake()
        {
            if (_targetComponent == null)
            {
                return;
            }

            // 自身を除く、同じシーン上に存在する、_targetComponent と同じコンポーネント
            IEnumerable<Component> sameComponentsWithoutSelf = FindObjectsOfType(_targetComponent.GetType(), true)
                .Select(obj => obj as Component)
                .Where(comp => comp && (comp != _targetComponent));

            foreach (Component comp in sameComponentsWithoutSelf)
            {
                // すでに対象の破棄が予約されていた場合は何もしない
                if (comp.TryGetComponent(out DestroySelfIfAlreadyExists ds) && ds.WillBeDestroyed)
                {
                    continue;
                }

                // 自身を破棄する
                DestroySelf();
            }
        }

        private void OnValidate()
        {
            if (_targetComponent == null)
            {
                return;
            }

            if (_targetComponent.gameObject != gameObject)
            {
                Debug.LogWarning("自身と同じ GameObject にアタッチされたコンポーネントを指定してください。", gameObject);
                _targetComponent = null;
            }
        }

        private void DestroySelf()
        {
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

        private enum DestroyTarget
        {
            Component,
            GameObject
        }
    }
}
