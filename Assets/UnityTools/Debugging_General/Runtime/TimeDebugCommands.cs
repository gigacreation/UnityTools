using GigaCreation.Tools.Debugging.Core;
using GigaCreation.Tools.Service;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace GigaCreation.Tools.Debugging.General
{
    public class TimeDebugCommands : MonoBehaviour
    {
        [SerializeField] private FloatReactiveProperty _timeScale = new(1f);

        [Space]
        [SerializeField] private bool _withCommandKey;
        [SerializeField] private bool _withShiftKey;
        [SerializeField] private bool _withOptionKey;
        [SerializeField] private bool _withControlKey;

        private float _storedTimeScale = -1f;

        private void Start()
        {
            if (!ServiceLocator.TryGet(out IDebuggingService debuggingService))
            {
                return;
            }

            debuggingService
                .IsDebugMode
                .Where(x => x)
                .Subscribe(_ =>
                {
                    _timeScale
                        .Subscribe(x =>
                        {
                            Time.timeScale = x;
                        })
                        .AddTo(debuggingService.DebuggingDisposables);

                    this
                        .UpdateAsObservable()
                        .Where(_ => AreModifierKeysPressed())
                        .Subscribe(_ =>
                        {
                            if (Input.GetKeyDown(KeyCode.RightArrow))
                            {
                                SpeedUp(0.2f);
                            }

                            if (Input.GetKeyDown(KeyCode.LeftArrow))
                            {
                                SlowDown(0.2f);
                            }

                            if (Input.GetKeyDown(KeyCode.UpArrow))
                            {
                                SpeedUp(1f);
                            }

                            if (Input.GetKeyDown(KeyCode.DownArrow))
                            {
                                SlowDown(1f);
                            }

                            if (Input.GetKeyDown(KeyCode.Space))
                            {
                                TogglePause();
                            }
                        })
                        .AddTo(debuggingService.DebuggingDisposables);
                })
                .AddTo(this);
        }

        private bool AreModifierKeysPressed()
        {
            return (!_withCommandKey || Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand))
                && (!_withShiftKey || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                && (!_withOptionKey || Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
                && (!_withControlKey || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl));
        }

        public void SpeedUp(float num)
        {
            _timeScale.Value = Time.timeScale + num;
            _storedTimeScale = -1f;
        }

        public void SlowDown(float num)
        {
            _timeScale.Value = Mathf.Max(0f, Time.timeScale - num);

            if (Time.timeScale > 0f)
            {
                _storedTimeScale = -1f;
            }
        }

        public void TogglePause()
        {
            if (_storedTimeScale <= -1f)
            {
                if (Time.timeScale > 0f)
                {
                    _storedTimeScale = Time.timeScale;
                    _timeScale.Value = 0f;
                }

                return;
            }

            _timeScale.Value = _storedTimeScale;
            _storedTimeScale = -1;
        }
    }
}
