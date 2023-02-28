using GigaCreation.Tools.Service;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace GigaCreation.Tools.Debugging
{
    public class FireOnSwitchingDebugMode : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onEnterDebugMode;
        [SerializeField] private UnityEvent _onExitDebugMode;

        private void Start()
        {
            if (ServiceLocator.TryGet(out IDebuggingCore debugCore))
            {
                debugCore
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
