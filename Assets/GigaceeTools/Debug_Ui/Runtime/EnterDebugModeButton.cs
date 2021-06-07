using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace GigaceeTools
{
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    [RequireComponent(typeof(Image), typeof(Button))]
    public class EnterDebugModeButton : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Button _button;

        [SerializeField] private float _longPressDuration = 1f;

        private IDebugCore _debugCore;

        private void Reset()
        {
            _image = GetComponent<Image>();
            _button = GetComponent<Button>();
        }

        protected virtual void Start()
        {
            if (!ServiceLocator.TryGetInstance(out _debugCore))
            {
                return;
            }

            var pressed = false;
            var pressedTime = 0f;

            _image.fillAmount = 0f;

            this.UpdateAsObservable()
                .Where(_ => pressed)
                .Subscribe(_ =>
                {
                    _image.fillAmount = (Time.realtimeSinceStartup - pressedTime) / _longPressDuration;

                    if (Time.realtimeSinceStartup - pressedTime < _longPressDuration)
                    {
                        return;
                    }

                    ReturnToDefault();
                    EnableDebugMode();
                });

            _button.OnPointerDownAsObservable()
                .Where(_ => !_debugCore.IsDebugMode.Value)
                .Subscribe(_ =>
                {
                    pressed = true;
                    pressedTime = Time.realtimeSinceStartup;
                });

            _button.OnPointerUpAsObservable()
                .Where(_ => !_debugCore.IsDebugMode.Value)
                .Subscribe(_ =>
                {
                    ReturnToDefault();
                });

            void ReturnToDefault()
            {
                pressed = false;
                _image.fillAmount = 0f;
            }
        }

        protected virtual void EnableDebugMode()
        {
            _debugCore.IsDebugMode.Value = true;
        }
    }
}
