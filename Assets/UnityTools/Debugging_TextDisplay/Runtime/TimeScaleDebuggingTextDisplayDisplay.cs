using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace GigaCreation.Tools
{
    public class TimeScaleDebuggingTextDisplayDisplay : DebuggingTextDisplay
    {
        protected override void Initialize()
        {
            base.Initialize();

            DebugCore
                .IsDebugMode
                .Where(x => x)
                .Subscribe(_ =>
                {
                    this
                        .UpdateAsObservable()
                        .Subscribe(__ =>
                        {
                            Label.SetText($"TimeScale: {Time.timeScale}");
                        })
                        .AddTo(DebugCore.DebugDisposables);
                })
                .AddTo(this);
        }
    }
}
