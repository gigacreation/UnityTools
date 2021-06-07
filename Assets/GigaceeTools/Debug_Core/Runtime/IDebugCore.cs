using UniRx;

namespace GigaceeTools
{
    public interface IDebugCore
    {
        BoolReactiveProperty IsDebugMode { get; }
        CompositeDisposable Disposables { get; }
    }
}
