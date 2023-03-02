using System;
using System.Threading;
using Cysharp.Threading.Tasks;
#if UNITASK_DOTWEEN_SUPPORT
using DG.Tweening;
#endif

namespace GigaCreation.Tools.UniTasks
{
    public static class UniTaskHelper
    {
        public static async UniTask SkippableDelay(
            int millisecondsDelay,
            Func<bool> condToSkip,
            bool ignoreTimeScale = false,
            PlayerLoopTiming delayTiming = PlayerLoopTiming.Update,
            CancellationToken ct = default
        )
        {
            TimeSpan delayTimeSpan = TimeSpan.FromMilliseconds(millisecondsDelay);

            await SkippableDelay(delayTimeSpan, condToSkip, ignoreTimeScale, delayTiming, ct);
        }

        public static async UniTask SkippableDelay(
            TimeSpan delayTimeSpan,
            Func<bool> condToSkip,
            bool ignoreTimeScale = false,
            PlayerLoopTiming delayTiming = PlayerLoopTiming.Update,
            CancellationToken ct = default
        )
        {
            using var cts = new CancellationTokenSource();
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, ct);

            await UniTask.WhenAny(
                UniTask.Delay(delayTimeSpan, ignoreTimeScale, delayTiming, linkedCts.Token),
                UniTask.WaitUntil(condToSkip, delayTiming, linkedCts.Token)
            );

            cts.Cancel();
        }

#if UNITASK_DOTWEEN_SUPPORT
        public static async UniTask SkippableTween(
            Tween tween,
            Func<bool> condToSkip,
            TweenCancelBehaviour tweenCancelBehaviour = TweenCancelBehaviour.Complete,
            PlayerLoopTiming delayTiming = PlayerLoopTiming.Update,
            CancellationToken ct = default
        )
        {
            using var cts = new CancellationTokenSource();
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, ct);

            await UniTask.WhenAny(
                tween.ToUniTask(tweenCancelBehaviour, linkedCts.Token),
                UniTask.WaitUntil(condToSkip, delayTiming, linkedCts.Token)
            );

            cts.Cancel();
        }
#endif
    }
}
