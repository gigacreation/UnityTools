using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GigaCreation.Tools.Ui;
using GigaCreation.Tools.UniTasks;
using UnityEngine;

namespace GigaCreation.Tools.Demo
{
    public class UniTaskDemo : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SlicedFilledImage _slicedFilledImage;

        [Header("Parameters")]
        [SerializeField] private float _duration;

        private CancellationToken _ctOnDestroy;

        private void Awake()
        {
            _ctOnDestroy = this.GetCancellationTokenOnDestroy();

            SetFillAmount(0f);
        }

        public void Delay()
        {
            DelayAsync(_ctOnDestroy).Forget();
        }

        public void Fill()
        {
            FillAsync(_ctOnDestroy).Forget();
        }

        private async UniTask DelayAsync(CancellationToken ct = default)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(_duration);

            Debug.Log("Delay start.");

            await UniTaskHelper.SkippableDelay(timeSpan, static () => Input.anyKeyDown, ct: ct);

            Debug.Log("Delay end.");
        }

        private async UniTask FillAsync(CancellationToken ct = default)
        {
            Tweener tweener = DOVirtual.Float(0f, 1f, _duration, SetFillAmount).SetEase(Ease.Linear);

            Debug.Log("Fill start.");

            await UniTaskHelper.SkippableTween(tweener, static () => Input.anyKeyDown, ct: ct);

            Debug.Log("Fill end.");
        }

        private void SetFillAmount(float amount)
        {
            _slicedFilledImage.FillAmount = amount;
        }
    }
}
