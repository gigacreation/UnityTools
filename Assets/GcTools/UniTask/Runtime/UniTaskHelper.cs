using System;
using System.Threading;
using Cysharp.Threading.Tasks;
#if UNITASK_DOTWEEN_SUPPORT
using DG.Tweening;
#endif

namespace GcTools
{
    public static class UniTaskHelper
    {
        public static async UniTask SkippableDelay(
            int millisecondsDelay,
            Func<bool> cond,
            bool ignoreTimeScale = false,
            PlayerLoopTiming delayTiming = PlayerLoopTiming.Update
        )
        {
            var cancellationTokenSource = new CancellationTokenSource();

            await UniTask.WhenAny(
                UniTask.Delay(millisecondsDelay, ignoreTimeScale, delayTiming, cancellationTokenSource.Token),
                UniTask.WaitUntil(cond, delayTiming, cancellationTokenSource.Token)
            );

            cancellationTokenSource.Cancel();
        }

#if UNITASK_DOTWEEN_SUPPORT
        public static async UniTask SkippableTween(
            Tween tween,
            Func<bool> cond,
            TweenCancelBehaviour tweenCancelBehaviour = TweenCancelBehaviour.Kill,
            PlayerLoopTiming delayTiming = PlayerLoopTiming.Update
        )
        {
            var cancellationTokenSource = new CancellationTokenSource();

            await UniTask.WhenAny(
                tween.ToUniTask(tweenCancelBehaviour, cancellationTokenSource.Token),
                UniTask.WaitUntil(cond, delayTiming, cancellationTokenSource.Token)
            );

            cancellationTokenSource.Cancel();
        }
#endif
    }
}
