using UniRx;
using UnityEngine;

namespace GigaceeTools
{
    [DefaultExecutionOrder(-1)]
    public class DebugPresenter : MonoBehaviour
    {
        [SerializeField] private BoolReactiveProperty _debugMode;
        [SerializeField] private bool _forceReleaseBuild;

        private IDebugCore _debugCore;

        private void Awake()
        {
            if (!Debug.isDebugBuild || _forceReleaseBuild || ServiceLocator.IsRegistered<IDebugCore>())
            {
                Destroy(gameObject);
                return;
            }

            _debugCore = new DebugCore(_debugMode.Value, this);

            ServiceLocator.Register(_debugCore);
        }

        private void Start()
        {
            _debugCore
                .IsDebugMode
                .Subscribe(x =>
                {
                    _debugMode.Value = x;
                })
                .AddTo(this);

            _debugMode
                .Subscribe(x =>
                {
                    _debugCore.IsDebugMode.Value = x;
                })
                .AddTo(this);
        }

        private void OnDestroy()
        {
            if (ServiceLocator.IsRegistered(_debugCore))
            {
                ServiceLocator.Unregister(_debugCore);
            }
        }
    }
}
