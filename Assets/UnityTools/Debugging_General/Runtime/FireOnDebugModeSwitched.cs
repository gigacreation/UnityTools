using GigaCreation.Tools.Debugging.Core;
using GigaCreation.Tools.Service;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace GigaCreation.Tools.Debugging
{
    public class FireOnDebugModeSwitched : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onEnterDebugMode;
        [SerializeField] private UnityEvent _onExitDebugMode;

        private void Start()
        {
            if (ServiceLocator.TryGet(out IDebuggingService debuggingCore))
            {
                debuggingCore
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
}
