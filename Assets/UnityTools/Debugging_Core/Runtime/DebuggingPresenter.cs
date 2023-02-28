using GigaCreation.Tools.Service;
using UniRx;
using UnityEngine;

namespace GigaCreation.Tools.Debugging
{
    [DefaultExecutionOrder(-1)]
    public class DebuggingPresenter : MonoBehaviour
    {
        [SerializeField] private bool _forceReleaseBuild;

        // TODO: _forceReleaseBuild が true の場合は Disable にする
        [SerializeField] private BoolReactiveProperty _debugMode;

        private IDebuggingCore _debuggingCore;

        private void Awake()
        {
            if (!Debug.isDebugBuild)
            {
                Destroy(this);
                return;
            }

            if (ServiceLocator.TryGet(out _debuggingCore))
            {
                if (FindObjectsOfType<DebuggingPresenter>(true).Length == 1)
                {
                    LinkDebugModeFlags(_debuggingCore);
                    return;
                }

                Destroy(this);
                return;
            }

            if (_forceReleaseBuild)
            {
                Destroy(this);
                return;
            }

            _debuggingCore = new DebuggingCore(_debugMode.Value);

            LinkDebugModeFlags(_debuggingCore);

            ServiceLocator.Register(_debuggingCore);
        }

        private void OnApplicationQuit()
        {
            ServiceLocator.Unregister(_debuggingCore);
        }

        /// <summary>
        /// この Presenter のデバッグモードフラグと、DebuggingCore のデバッグモードフラグを連動させます。
        /// </summary>
        /// <param name="debuggingCore"></param>
        private void LinkDebugModeFlags(IDebuggingCore debuggingCore)
        {
            debuggingCore
                .IsDebugMode
                .Subscribe(x =>
                {
                    _debugMode.Value = x;
                })
                .AddTo(this);

            _debugMode
                .SkipLatestValueOnSubscribe()
                .Subscribe(x =>
                {
                    debuggingCore.IsDebugMode.Value = x;
                })
                .AddTo(this);
        }
    }
}
