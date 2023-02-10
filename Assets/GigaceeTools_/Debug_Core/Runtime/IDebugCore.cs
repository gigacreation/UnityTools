using UniRx;

namespace GigaceeTools
{
    public interface IDebugCore
    {
        IReactiveProperty<bool> IsDebugMode { get; }
        CompositeDisposable DebugDisposables { get; }
    }
}
