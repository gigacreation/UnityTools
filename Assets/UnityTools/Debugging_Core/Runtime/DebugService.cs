using UniRx;

namespace GigaCreation.Tools.Debugging.Core
{
    public class DebugService : IDebugService
    {
        private readonly ReactiveProperty<bool> _isDebugMode;

        public IReactiveProperty<bool> IsDebugMode => _isDebugMode;
        public CompositeDisposable DebuggingDisposables { get; } = new();

        public DebugService(bool initialMode)
        {
            _isDebugMode = new ReactiveProperty<bool>(initialMode);

            _isDebugMode
                .Where(x => !x)
                .Subscribe(_ =>
                {
                    DebuggingDisposables.Clear();
                });
        }

        public void Dispose()
        {
            _isDebugMode.Dispose();
            DebuggingDisposables.Dispose();
        }
    }
}
