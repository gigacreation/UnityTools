using UniRx;
using UnityEngine;

namespace GigaceeTools
{
    public class DebugCore : IDebugCore
    {
        private readonly ReactiveProperty<bool> _isDebugMode;

        public DebugCore(bool initialMode, Component linkedComponent)
        {
            _isDebugMode = new ReactiveProperty<bool>(initialMode);

            _isDebugMode
                .Where(x => !x)
                .Subscribe(_ =>
                {
                    DebugDisposables.Clear();
                })
                .AddTo(linkedComponent);
        }

        public IReactiveProperty<bool> IsDebugMode => _isDebugMode;
        public CompositeDisposable DebugDisposables { get; } = new CompositeDisposable();
    }
}
