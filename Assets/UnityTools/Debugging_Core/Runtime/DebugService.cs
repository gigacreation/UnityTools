using UniRx;

namespace GigaCreation.Tools.Debugging.Core
{
    public class DebugService : IDebugService
    {
        private readonly ReactiveProperty<bool> _isDebugMode;

        public IReactiveProperty<bool> IsDebugMode => _isDebugMode;
        public CompositeDisposable DebugDisposables { get; } = new();

        public DebugService(bool initialMode)
        {
            _isDebugMode = new ReactiveProperty<bool>(initialMode);

            _isDebugMode
                .Where(x => !x)
                .Subscribe(_ =>
                {
                    DebugDisposables.Clear();
                });
        }

        public void Dispose()
        {
            _isDebugMode.Dispose();
            DebugDisposables.Dispose();
        }
    }
}
