﻿using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GigaceeTools
{
    public static class UniTaskDebug
    {
        private static bool s_isReleaseMode;
        private static IDebugCore s_debugCore;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Initialize()
        {
            s_isReleaseMode = false;
            s_debugCore = null;
        }

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
            if (!s_isReleaseMode && (s_debugCore == null) && !ServiceLocator.TryGetInstance(out s_debugCore))
            {
                s_isReleaseMode = true;
            }

            await UniTask.WhenAny(
                UniTask.Delay(millisecondsDelay, ignoreTimeScale, delayTiming, cancellationToken),
                UniTask.WaitUntil(
                    () => (s_debugCore != null) && s_debugCore.IsDebugMode.Value && Input.anyKeyDown,
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
            if (!s_isReleaseMode && (s_debugCore == null) && !ServiceLocator.TryGetInstance(out s_debugCore))
            {
                s_isReleaseMode = true;
            }

            await UniTask.WhenAny(
                UniTask.Delay(delayTimeSpan, ignoreTimeScale, delayTiming, cancellationToken),
                UniTask.WaitUntil(
                    () => (s_debugCore != null) && s_debugCore.IsDebugMode.Value && Input.anyKeyDown,
                    delayTiming,
                    cancellationToken
                )
            );
        }
    }
}