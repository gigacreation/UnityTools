using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GigaCreation.Tools.Ui;
using GigaCreation.Tools.UniTasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace GigaCreation.Tools.Demo
{
    public class UniTaskDemo : MonoBehaviour
    {
        [SerializeField] private SlicedFilledImage _slicedFilledImage;
        [SerializeField] private Button _delayDemoButton;
        [SerializeField] private Button _tweenDemoButton;

        private void Awake()
        {
            SetFillAmount(0f);
        }

        private void Start()
        {
            CancellationToken ctOnDestroy = this.GetCancellationTokenOnDestroy();

            _delayDemoButton
                .OnClickAsObservable()
                .Subscribe(_ =>
                {
                    DelayAsync(ctOnDestroy).Forget();
                })
                .AddTo(this);

            _tweenDemoButton
                .OnClickAsObservable()
                .Subscribe(_ =>
                {
                    Tweener tweener = DOVirtual.Float(0f, 1f, 5f, SetFillAmount).SetEase(Ease.Linear);
                    FillAsync(tweener, ctOnDestroy).Forget();
                })
                .AddTo(this);
        }

        private static async UniTask DelayAsync(CancellationToken ct = default)
        {
            Debug.Log("Delay start.");

            await UniTaskHelper.SkippableDelay(TimeSpan.FromSeconds(5f), () => Input.anyKeyDown, ct: ct);

            Debug.Log("Delay end.");
        }

        private static async UniTask FillAsync(Tween tweener, CancellationToken ct = default)
        {
            Debug.Log("Fill start.");

            await UniTaskHelper.SkippableTween(tweener, () => Input.anyKeyDown, ct: ct);

            Debug.Log("Fill end.");
        }

        private void SetFillAmount(float amount)
        {
            _slicedFilledImage.FillAmount = amount;
        }
    }
}
