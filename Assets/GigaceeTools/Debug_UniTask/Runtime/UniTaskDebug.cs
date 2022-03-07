using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace GigaceeTools
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public static class UniTaskDebug
    {
        private static bool s_isReleaseMode;
        private static IDebugCore s_debugCore;

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
            if (!s_isReleaseMode && (s_debugCore == null) && !ServiceLocator.TryGet(out s_debugCore))
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
            if (!s_isReleaseMode && (s_debugCore == null) && !ServiceLocator.TryGet(out s_debugCore))
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

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStaticFields()
        {
            s_isReleaseMode = false;
            s_debugCore = null;
        }
    }
}
