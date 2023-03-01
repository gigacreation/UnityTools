using GigaCreation.Tools.Service;
using UniRx;
using UnityEngine;

namespace GigaCreation.Tools.Debugging.Core
{
    [DefaultExecutionOrder(-1)]
    public class DebuggingPresenter : MonoBehaviour
    {
        [SerializeField] private bool _forceReleaseBuild;
        [SerializeField] private BoolReactiveProperty _isDebugMode;

        private IDebuggingService _debuggingService;

        private void Awake()
        {
            // リリースビルド痔は自身を破棄する
            if (!Debug.isDebugBuild || _forceReleaseBuild)
            {
                Destroy(this);
                return;
            }

            if (ServiceLocator.TryGet(out _debuggingService))
            {
                DebuggingPresenter[] debuggingPresentersInScene
                    = FindObjectsByType<DebuggingPresenter>(FindObjectsInactive.Include, FindObjectsSortMode.None);

                // 自身が唯一の DebuggingPresenter だった場合は、自身のデバッグモードフラグをサービス側とリンクさせる
                if (debuggingPresentersInScene.Length == 1)
                {
                    LinkDebugModeFlags(_debuggingService);
                    return;
                }

                // 自身の他に DebuggingPresenter が存在していたら、自身を破棄する
                Destroy(this);
                return;
            }

            _debuggingService = new DebuggingService(_isDebugMode.Value);

            LinkDebugModeFlags(_debuggingService);

            ServiceLocator.Register(_debuggingService);
        }

        private void OnApplicationQuit()
        {
            ServiceLocator.Unregister(_debuggingService);
        }

        /// <summary>
        /// この Presenter のデバッグモードフラグと、DebuggingService のデバッグモードフラグを連動させます。
        /// </summary>
        /// <param name="debuggingService">デバッグサービス。</param>
        private void LinkDebugModeFlags(IDebuggingService debuggingService)
        {
            debuggingService
                .IsDebugMode
                .Subscribe(x =>
                {
                    _isDebugMode.Value = x;
                })
                .AddTo(this);

            _isDebugMode
                .SkipLatestValueOnSubscribe()
                .Subscribe(x =>
                {
                    debuggingService.IsDebugMode.Value = x;
                })
                .AddTo(this);
        }
    }
}
