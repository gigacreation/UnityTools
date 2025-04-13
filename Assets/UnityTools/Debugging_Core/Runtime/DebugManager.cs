using UniRx;

namespace GigaCreation.Tools.Debugging.Core
{
    public class DebugManager : IDebugManager
    {
        private readonly ReactiveProperty<bool> _isDebugMode;

        public IReactiveProperty<bool> IsDebugMode => _isDebugMode;

        public DebugManager(bool initialMode)
        {
            _isDebugMode = new ReactiveProperty<bool>(initialMode);
        }

        public void Dispose()
        {
            _isDebugMode.Dispose();
        }
    }
}
