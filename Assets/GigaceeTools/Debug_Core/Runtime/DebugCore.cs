using System;
using UniRx;

namespace GigaceeTools
{
    public class DebugCore : IDebugCore, IDisposable
    {
        private readonly ReactiveProperty<bool> _isDebugMode;

        public DebugCore(bool initialMode)
        {
            _isDebugMode = new ReactiveProperty<bool>(initialMode);

            _isDebugMode
                .Where(x => !x)
                .Subscribe(_ =>
                {
                    DebugDisposables.Clear();
                });
        }

        public IReactiveProperty<bool> IsDebugMode => _isDebugMode;
        public CompositeDisposable DebugDisposables { get; } = new CompositeDisposable();

        public void Dispose()
        {
            _isDebugMode.Dispose();
            DebugDisposables.Dispose();
        }
    }
}
