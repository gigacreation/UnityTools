using GigaCreation.Tools.Service;
using UniRx;
using UnityEngine;

namespace GigaCreation.Tools.Debugging.Core
{
    public class DebugPresenter : MonoBehaviour
    {
        [SerializeField] private bool _forceReleaseBuild;
        [SerializeField] private BoolReactiveProperty _isDebugMode;

        private IDebugService _debugService;

        private void Awake()
        {
            // リリースビルド時は自身を破棄する
            if (!Debug.isDebugBuild || _forceReleaseBuild)
            {
                Destroy(this);
                return;
            }

            if (ServiceLocator.TryGet(out _debugService))
            {
                DebugPresenter[] debuggingPresentersInScene
                    = FindObjectsByType<DebugPresenter>(FindObjectsInactive.Include, FindObjectsSortMode.None);

                // DebuggingService はすでに登録されているが、DebuggingPresenter はシーン上に自分しかいない場合、
                // 自身のデバッグモードフラグを DebuggingService とリンクさせて終了する
                // （別のシーンで DebuggingPresenter が DebuggingService を登録した後にこのシーンへ遷移してきた場合など）
                if (debuggingPresentersInScene.Length == 1)
                {
                    LinkDebugModeFlags(_debugService);
                    return;
                }

                // 自身の他に DebuggingPresenter が存在していたら、自身を破棄する
                Destroy(this);
                return;
            }

            // DebuggingService がまだ登録されていなかった場合、DebuggingService を生成し、デバッグモードフラグを自身とリンクさせ、登録を行う
            _debugService = new DebugService(_isDebugMode.Value);
            LinkDebugModeFlags(_debugService);
            ServiceLocator.Register(_debugService);
        }

        private void OnApplicationQuit()
        {
            ServiceLocator.Unregister(_debugService);
        }

        /// <summary>
        /// この Presenter のデバッグモードフラグと、DebuggingService のデバッグモードフラグを連動させます。
        /// </summary>
        /// <param name="debugService">デバッグサービス。</param>
        private void LinkDebugModeFlags(IDebugService debugService)
        {
            debugService
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
                    debugService.IsDebugMode.Value = x;
                })
                .AddTo(this);
        }
    }
}
