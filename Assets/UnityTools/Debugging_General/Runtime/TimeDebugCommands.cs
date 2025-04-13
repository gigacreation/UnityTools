using GigaCreation.Tools.Debugging.Core;
using GigaCreation.Tools.Service;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace GigaCreation.Tools.Debugging.General
{
    public class TimeDebugCommands : MonoBehaviour
    {
        [SerializeField] private FloatReactiveProperty _timeScale = new(1f);
        [SerializeField] private bool _withCommandKey;
        [SerializeField] private bool _withShiftKey;
        [SerializeField] private bool _withOptionKey;
        [SerializeField] private bool _withControlKey;

        private readonly CompositeDisposable _disposables = new();

        private float _storedTimeScale = -1f;

        private void Start()
        {
            if (!ServiceLocator.TryGet(out IDebugManager debugManager))
            {
                return;
            }

            debugManager
                .IsDebugMode
                .Subscribe(isOn =>
                {
                    if (isOn)
                    {
                        _timeScale
                            .Subscribe(static x =>
                            {
                                Time.timeScale = x;
                            })
                            .AddTo(_disposables);

                        this
                            .UpdateAsObservable()
                            .Where(_ => AreModifierKeysPressed())
                            .Subscribe(_ =>
                            {
                                OnUpdateInDebugMode();
                            })
                            .AddTo(_disposables);
                    }
                    else
                    {
                        _disposables.Clear();
                    }
                })
                .AddTo(this);
        }

        private void OnUpdateInDebugMode()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            {
                SpeedUp(0.2f);
            }

            if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            {
                SlowDown(0.2f);
            }

            if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            {
                SpeedUp(1f);
            }

            if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            {
                SlowDown(1f);
            }

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                TogglePause();
            }
#else
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
#endif
        }

        private bool AreModifierKeysPressed()
        {
#if ENABLE_INPUT_SYSTEM
            return (!_withCommandKey
                    || Keyboard.current.leftCommandKey.wasPressedThisFrame
                    || Keyboard.current.rightCommandKey.wasPressedThisFrame)
                && (!_withShiftKey
                    || Keyboard.current.leftShiftKey.wasPressedThisFrame
                    || Keyboard.current.rightShiftKey.wasPressedThisFrame)
                && (!_withOptionKey
                    || Keyboard.current.leftAltKey.wasPressedThisFrame
                    || Keyboard.current.rightAltKey.wasPressedThisFrame)
                && (!_withControlKey
                    || Keyboard.current.leftCtrlKey.wasPressedThisFrame
                    || Keyboard.current.rightCtrlKey.wasPressedThisFrame);
#else
            return (!_withCommandKey || Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand))
                && (!_withShiftKey || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                && (!_withOptionKey || Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
                && (!_withControlKey || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl));
#endif
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
