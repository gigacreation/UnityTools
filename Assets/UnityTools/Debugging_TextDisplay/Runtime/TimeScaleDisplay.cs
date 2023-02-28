using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace GigaCreation.Tools.Debugging
{
    public class TimeScaleDisplay : DebuggingTextDisplay
    {
        protected override void Initialize()
        {
            base.Initialize();

            DebuggingCore
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
                        .AddTo(DebuggingCore.DebugDisposables);
                })
                .AddTo(this);
        }
    }
}
