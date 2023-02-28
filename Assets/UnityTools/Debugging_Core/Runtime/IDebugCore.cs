using System;
using GigaCreation.Tools.Service;
using UniRx;

namespace GigaCreation.Tools
{
    public interface IDebugCore : IService, IDisposable
    {
        IReactiveProperty<bool> IsDebugMode { get; }
        CompositeDisposable DebugDisposables { get; }
    }
}
