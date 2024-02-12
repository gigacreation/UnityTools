using GigaCreation.Tools.Debugging.Core;
using GigaCreation.Tools.Service;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace GigaCreation.Tools.Debugging.General
{
    /// <summary>
    /// デバッグモードが切り替わったとき、特定のイベントを実行します。
    /// </summary>
    public class FireOnDebugModeChanged : MonoBehaviour
    {
        /// <summary>
        /// デバッグモードに入ったときに実行されるイベントです。
        /// </summary>
        [SerializeField] private UnityEvent _onEnterDebugMode;

        /// <summary>
        /// デバッグモードが解除されたときに実行されるイベントです。
        /// </summary>
        [SerializeField] private UnityEvent _onExitDebugMode;

        private void Start()
        {
            if (!ServiceLocator.TryGet(out IDebugManager debugManager))
            {
                return;
            }

            debugManager
                .IsDebugMode
                .Subscribe(x =>
                {
                    if (x)
                    {
                        _onEnterDebugMode.Invoke();
                    }
                    else
                    {
                        _onExitDebugMode.Invoke();
                    }
                })
                .AddTo(this);
        }
    }
}
