using System.Diagnostics.CodeAnalysis;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace GigaceeTools
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class TimeDebugCommands : MonoBehaviour
    {
        [SerializeField] private FloatReactiveProperty _timeScale = new FloatReactiveProperty(1f);

        [SerializeField] private bool _withCtrlKey;
        [SerializeField] private bool _withShiftKey;
        [SerializeField] private bool _withAltKey;

        private float _storedTimeScale;

        private void Start()
        {
            if (!ServiceLocator.TryGet(out IDebugCore debugCore))
            {
                return;
            }

            debugCore
                .IsDebugMode
                .Where(x => x)
                .Subscribe(_ =>
                {
                    _timeScale
                        .Subscribe(x =>
                        {
                            Time.timeScale = x;
                        })
                        .AddTo(debugCore.DebugDisposables);

                    this
                        .UpdateAsObservable()
                        .Where(_ => AreModifierKeysPressed())
                        .Subscribe(_ =>
                        {
                            if (Input.GetKeyDown(KeyCode.Space))
                            {
                                TogglePause();
                            }

                            if (Input.GetKeyDown(KeyCode.RightArrow))
                            {
                                SpeedUp(0.05f);
                            }

                            if (Input.GetKeyDown(KeyCode.LeftArrow))
                            {
                                SlowDown(0.05f);
                            }

                            if (Input.GetKeyDown(KeyCode.UpArrow))
                            {
                                SpeedUp(0.2f);
                            }

                            if (Input.GetKeyDown(KeyCode.DownArrow))
                            {
                                SlowDown(0.2f);
                            }
                        })
                        .AddTo(debugCore.DebugDisposables);
                })
                .AddTo(this);
        }

        private bool AreModifierKeysPressed()
        {
            return (!_withCtrlKey || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                   && (!_withShiftKey || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                   && (!_withAltKey || Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt));
        }

        public void TogglePause()
        {
            if (Mathf.Approximately(Time.timeScale, 0f))
            {
                _timeScale.Value = _storedTimeScale;
                return;
            }

            _storedTimeScale = Time.timeScale;
            _timeScale.Value = 0f;
        }

        public void SpeedUp(float num)
        {
            _timeScale.Value = Time.timeScale + num;
        }

        public void SlowDown(float num)
        {
            _timeScale.Value = Mathf.Max(0f, Time.timeScale - num);
        }

        private void OnClickSlowDownButtonInInspector()
        {
            SlowDown(0.2f);
        }

        private void OnClickPauseButtonInInspector()
        {
            TogglePause();
        }

        private void OnClickSpeedUpButtonInInspector()
        {
            SpeedUp(0.2f);
        }
    }
}
