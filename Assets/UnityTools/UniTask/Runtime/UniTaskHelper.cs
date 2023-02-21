using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
#if UNITASK_DOTWEEN_SUPPORT
using DG.Tweening;
#endif

namespace GigaCreation.Tools
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public static class UniTaskHelper
    {
        public static async UniTask SkippableDelay(
            int millisecondsDelay,
            Func<bool> cond,
            bool ignoreTimeScale = false,
            PlayerLoopTiming delayTiming = PlayerLoopTiming.Update,
            CancellationToken ct = default
        )
        {
            var currentCts = new CancellationTokenSource();
            var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(currentCts.Token, ct);

            await UniTask.WhenAny(
                UniTask.Delay(millisecondsDelay, ignoreTimeScale, delayTiming, linkedCts.Token),
                UniTask.WaitUntil(cond, delayTiming, linkedCts.Token)
            );

            currentCts.Cancel();
            currentCts.Dispose();
            linkedCts.Dispose();
        }

        public static async UniTask SkippableDelay(
            TimeSpan delayTimeSpan,
            Func<bool> cond,
            bool ignoreTimeScale = false,
            PlayerLoopTiming delayTiming = PlayerLoopTiming.Update,
            CancellationToken ct = default
        )
        {
            var currentCts = new CancellationTokenSource();
            var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(currentCts.Token, ct);

            await UniTask.WhenAny(
                UniTask.Delay(delayTimeSpan, ignoreTimeScale, delayTiming, linkedCts.Token),
                UniTask.WaitUntil(cond, delayTiming, linkedCts.Token)
            );

            currentCts.Cancel();
            currentCts.Dispose();
            linkedCts.Dispose();
        }

#if UNITASK_DOTWEEN_SUPPORT
        public static async UniTask SkippableTween(
            Tween tween,
            Func<bool> cond,
            TweenCancelBehaviour tweenCancelBehaviour = TweenCancelBehaviour.Kill,
            PlayerLoopTiming delayTiming = PlayerLoopTiming.Update,
            CancellationToken ct = default
        )
        {
            var currentCts = new CancellationTokenSource();
            var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(currentCts.Token, ct);

            await UniTask.WhenAny(
                tween.ToUniTask(tweenCancelBehaviour, linkedCts.Token),
                UniTask.WaitUntil(cond, delayTiming, linkedCts.Token)
            );

            currentCts.Cancel();
            currentCts.Dispose();
            linkedCts.Dispose();
        }
#endif
    }
}
