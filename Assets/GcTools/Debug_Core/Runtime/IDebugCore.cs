using UniRx;

namespace GcTools
{
    public interface IDebugCore
    {
        BoolReactiveProperty IsDebugMode { get; }
        CompositeDisposable Disposables { get; }
    }
}
