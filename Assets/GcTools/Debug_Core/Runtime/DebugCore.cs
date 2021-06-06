using UniRx;
using UnityEngine;

namespace GcTools
{
    public class DebugCore : MonoBehaviour, IDebugCore
    {
        [SerializeField] private bool _forceReleaseBuild;
        [SerializeField] private BoolReactiveProperty _isDebugMode;

        private void Awake()
        {
            if (!Debug.isDebugBuild || _forceReleaseBuild || ServiceLocator.IsRegistered<IDebugCore>())
            {
                Destroy(gameObject);
                return;
            }

            ServiceLocator.Register<IDebugCore>(this);

            IsDebugMode.Where(x => !x).Subscribe(x =>
            {
                Disposables.Clear();
            });
        }

        private void OnDestroy()
        {
            if (ServiceLocator.IsRegistered(this))
            {
                ServiceLocator.Unregister<IDebugCore>(this);
            }
        }

        public BoolReactiveProperty IsDebugMode => _isDebugMode;
        public CompositeDisposable Disposables { get; } = new CompositeDisposable();
    }
}
