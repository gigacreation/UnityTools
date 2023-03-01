using System;
using GigaCreation.Tools.Service;
using UniRx;

namespace GigaCreation.Tools.Debugging.Core
{
    public interface IDebuggingService : IService, IDisposable
    {
        IReactiveProperty<bool> IsDebugMode { get; }
        CompositeDisposable DebuggingDisposables { get; }
    }
}
