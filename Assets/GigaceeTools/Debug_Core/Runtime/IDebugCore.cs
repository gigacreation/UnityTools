using System;
using GigaCreation.Tools.Service;
using UniRx;

namespace GigaceeTools
{
    public interface IDebugCore : IService, IDisposable
    {
        IReactiveProperty<bool> IsDebugMode { get; }
        CompositeDisposable DebugDisposables { get; }
    }
}
