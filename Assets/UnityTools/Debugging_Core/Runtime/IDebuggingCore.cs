using System;
using GigaCreation.Tools.Service;
using UniRx;

namespace GigaCreation.Tools.Debugging
{
    public interface IDebuggingCore : IService, IDisposable
    {
        IReactiveProperty<bool> IsDebugMode { get; }
        CompositeDisposable DebugDisposables { get; }
    }
}
