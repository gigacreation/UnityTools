using GigaCreation.Tools.Debugging.Core.Models;
using GigaCreation.Tools.Service;
using UniRx;
using UnityEngine;

namespace GigaCreation.Tools.Debugging.Core.Presenters
{
    public class DebugPresenter : MonoBehaviour
    {
        [SerializeField] private bool _forceReleaseBuild;
        [SerializeField] private BoolReactiveProperty _isDebugMode;

        private IDebugManager _debugManager;

        private void Awake()
        {
            // リリースビルド時は自身を破棄する
            if (!Debug.isDebugBuild || _forceReleaseBuild)
            {
                Destroy(this);
                return;
            }

            if (ServiceLocator.TryGet(out _debugManager))
            {
                DebugPresenter[] debugPresentersInScene
                    = FindObjectsByType<DebugPresenter>(FindObjectsInactive.Include, FindObjectsSortMode.None);

                // DebugManager はすでに登録されているが、DebugPresenter はシーン上に自分しかいない場合、
                // 自身のデバッグモードフラグを DebugManager とリンクさせて終了する
                // （別のシーンで DebugPresenter が DebugManager を登録した後にこのシーンへ遷移してきた場合など）
                if (debugPresentersInScene.Length == 1)
                {
                    LinkDebugModeFlags(_debugManager);
                    return;
                }

                // 自身の他に DebugPresenter が存在していたら、自身を破棄する
                Destroy(this);
                return;
            }

            // DebugManager がまだ登録されていなかった場合、DebugManager を生成し、デバッグモードフラグを自身とリンクさせ、登録を行う
            _debugManager = new DebugManager(_isDebugMode.Value);
            LinkDebugModeFlags(_debugManager);
            ServiceLocator.Register(_debugManager);
        }

        private void OnApplicationQuit()
        {
            ServiceLocator.Unregister(_debugManager);
        }

        /// <summary>
        /// この Presenter のデバッグモードフラグと、DebugManager のデバッグモードフラグを連動させます。
        /// </summary>
        /// <param name="debugManager">デバッグサービス。</param>
        private void LinkDebugModeFlags(IDebugManager debugManager)
        {
            debugManager
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
                    debugManager.IsDebugMode.Value = x;
                })
                .AddTo(this);
        }
    }
}
