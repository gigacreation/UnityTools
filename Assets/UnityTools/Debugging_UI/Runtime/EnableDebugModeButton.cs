using GigaCreation.Tools.Debugging.Core;
using GigaCreation.Tools.Service;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace GigaCreation.Tools.Debugging.Ui
{
    [RequireComponent(typeof(CanvasGroup), typeof(Image), typeof(Selectable))]
    public class EnableDebugModeButton : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _image;
        [SerializeField] private Selectable _selectable;

        [Header("Parameters")]
        [SerializeField] private float _longPressDuration = 1f;

        private IDebugManager _debugManager;

        private bool _isPressed;

        private void Start()
        {
            if (!ServiceLocator.TryGet(out _debugManager))
            {
                return;
            }

            var pressedTime = 0f;
            _image.fillAmount = 0f;

            this
                .UpdateAsObservable()
                .Where(_ => _isPressed)
                .Subscribe(_ =>
                {
                    _image.fillAmount = (Time.realtimeSinceStartup - pressedTime) / _longPressDuration;

                    if (Time.realtimeSinceStartup - pressedTime < _longPressDuration)
                    {
                        return;
                    }

                    ResetToDefault();
                    _debugManager.IsDebugMode.Value = true;
                })
                .AddTo(this);

            _selectable
                .OnPointerDownAsObservable()
                .Where(_ => !_debugManager.IsDebugMode.Value)
                .Subscribe(_ =>
                {
                    _isPressed = true;
                    pressedTime = Time.realtimeSinceStartup;
                })
                .AddTo(this);

            _selectable
                .OnPointerUpAsObservable()
                .Where(_ => !_debugManager.IsDebugMode.Value)
                .Subscribe(_ =>
                {
                    ResetToDefault();
                })
                .AddTo(this);

            _debugManager
                .IsDebugMode
                .Subscribe(x =>
                {
                    _canvasGroup.alpha = x ? 0f : 1f;
                    _canvasGroup.interactable = !x;
                    _canvasGroup.blocksRaycasts = !x;
                })
                .AddTo(this);
        }

        private void Reset()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _image = GetComponent<Image>();
            _selectable = GetComponent<Selectable>();
        }

        private void ResetToDefault()
        {
            _isPressed = false;
            _image.fillAmount = 0f;
        }
    }
}
