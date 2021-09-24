using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace GigaceeTools
{
    public class FireOnSwitchingDebugMode : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onEnterDebugMode;
        [SerializeField] private UnityEvent _onExitDebugMode;

        private void Start()
        {
            if (ServiceLocator.TryGetInstance(out IDebugCore debugCore))
            {
                debugCore
                    .IsDebugMode
                    .Subscribe(x =>
                    {
                        if (x)
                        {
                            OnEnterDebugMode();
                        }
                        else
                        {
                            OnExitDebugMode();
                        }
                    })
                    .AddTo(this);
            }
        }

        protected virtual void OnEnterDebugMode()
        {
            _onEnterDebugMode?.Invoke();
        }

        protected virtual void OnExitDebugMode()
        {
            _onExitDebugMode?.Invoke();
        }
    }
}
