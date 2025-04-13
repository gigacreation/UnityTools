﻿using System;
using GigaCreation.Tools.Service;
using UniRx;

namespace GigaCreation.Tools.Debugging.Core
{
    public interface IDebugManager : IService, IDisposable
    {
        /// <summary>
        /// 現在、デバッグモードか否か。
        /// </summary>
        IReactiveProperty<bool> IsDebugMode { get; }
    }
}
