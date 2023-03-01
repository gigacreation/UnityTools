using System;
using GigaCreation.Tools.Service;
using UniRx;

namespace GigaCreation.Tools.Debugging.Core
{
    public interface IDebugService : IService, IDisposable
    {
        /// <summary>
        /// 現在、デバッグモードか否か。
        /// </summary>
        IReactiveProperty<bool> IsDebugMode { get; }
        
        /// <summary>
        /// デバッグモード解除時にクリアされる Disposable。
        /// </summary>
        CompositeDisposable DebuggingDisposables { get; }
    }
}
