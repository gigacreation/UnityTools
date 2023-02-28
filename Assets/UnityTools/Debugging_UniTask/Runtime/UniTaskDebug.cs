using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GigaCreation.Tools.Service;
using JetBrains.Annotations;
using UnityEngine;

namespace GigaCreation.Tools.Debugging
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public static class UniTaskDebug
    {
        private static bool s_isReleaseMode;
        private static IDebuggingCore s_debuggingCore;

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
            if (!s_isReleaseMode && (s_debuggingCore == null) && !ServiceLocator.TryGet(out s_debuggingCore))
            {
                s_isReleaseMode = true;
            }

            await UniTask.WhenAny(
                UniTask.Delay(millisecondsDelay, ignoreTimeScale, delayTiming, cancellationToken),
                UniTask.WaitUntil(
                    () => (s_debuggingCore != null) && s_debuggingCore.IsDebugMode.Value && Input.anyKeyDown,
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
            if (!s_isReleaseMode && (s_debuggingCore == null) && !ServiceLocator.TryGet(out s_debuggingCore))
            {
                s_isReleaseMode = true;
            }

            await UniTask.WhenAny(
                UniTask.Delay(delayTimeSpan, ignoreTimeScale, delayTiming, cancellationToken),
                UniTask.WaitUntil(
                    () => (s_debuggingCore != null) && s_debuggingCore.IsDebugMode.Value && Input.anyKeyDown,
                    delayTiming,
                    cancellationToken
                )
            );
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStaticFields()
        {
            s_isReleaseMode = false;
            s_debuggingCore = null;
        }
    }
}
