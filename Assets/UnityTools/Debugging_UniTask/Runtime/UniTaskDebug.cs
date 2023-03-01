﻿using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GigaCreation.Tools.Debugging.Core;
using GigaCreation.Tools.Service;
using JetBrains.Annotations;
using UnityEngine;

namespace GigaCreation.Tools.Debugging
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public static class UniTaskDebug
    {
        private static bool s_isReleaseMode;
        private static IDebuggingService s_debuggingService;

        /// <summary>
        /// デバッグモードの時のみスキップできる UniTask.Delay。
        /// </summary>
        public static async UniTask Delay(
            int millisecondsDelay,
            bool ignoreTimeScale = false,
            PlayerLoopTiming delayTiming = PlayerLoopTiming.Update,
            CancellationToken cancellationToken = default
        )
        {
            if (!s_isReleaseMode && (s_debuggingService == null) && !ServiceLocator.TryGet(out s_debuggingService))
            {
                s_isReleaseMode = true;
            }

            await UniTask.WhenAny(
                UniTask.Delay(millisecondsDelay, ignoreTimeScale, delayTiming, cancellationToken),
                UniTask.WaitUntil(
                    () => (s_debuggingService != null) && s_debuggingService.IsDebugMode.Value && Input.anyKeyDown,
                    delayTiming,
                    cancellationToken
                )
            );
        }

        /// <summary>
        /// デバッグモードの時のみスキップできる UniTask.Delay。
        /// </summary>
        public static async UniTask Delay(
            TimeSpan delayTimeSpan,
            bool ignoreTimeScale = false,
            PlayerLoopTiming delayTiming = PlayerLoopTiming.Update,
            CancellationToken cancellationToken = default
        )
        {
            if (!s_isReleaseMode && (s_debuggingService == null) && !ServiceLocator.TryGet(out s_debuggingService))
            {
                s_isReleaseMode = true;
            }

            await UniTask.WhenAny(
                UniTask.Delay(delayTimeSpan, ignoreTimeScale, delayTiming, cancellationToken),
                UniTask.WaitUntil(
                    () => (s_debuggingService != null) && s_debuggingService.IsDebugMode.Value && Input.anyKeyDown,
                    delayTiming,
                    cancellationToken
                )
            );
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStaticFields()
        {
            s_isReleaseMode = false;
            s_debuggingService = null;
        }
    }
}
