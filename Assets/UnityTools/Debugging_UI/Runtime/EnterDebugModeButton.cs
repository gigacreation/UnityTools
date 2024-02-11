using GigaCreation.Tools.Debugging.Core.Models;
using GigaCreation.Tools.Service;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace GigaCreation.Tools.Debugging.Ui
{
    [RequireComponent(typeof(Image), typeof(Selectable))]
    public class EnterDebugModeButton : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Selectable _selectable;

        [SerializeField] private float _longPressDuration = 1f;

        private IDebugManager _debugManager;

        private bool _isPressed;

        private void Reset()
        {
            _image = GetComponent<Image>();
            _selectable = GetComponent<Selectable>();
        }

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

                    ReturnToDefault();
                    EnableDebugMode();
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
                    ReturnToDefault();
                })
                .AddTo(this);
        }

        private void ReturnToDefault()
        {
            _isPressed = false;
            _image.fillAmount = 0f;
        }

        private void EnableDebugMode()
        {
            _debugManager.IsDebugMode.Value = true;
        }
    }
}
