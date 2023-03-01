using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace GigaCreation.Tools.Debugging
{
    public class TimeScaleDebugTextPreference : DebugTextPreference
    {
        protected override void Initialize()
        {
            base.Initialize();

            DebugService
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
                        .AddTo(DebugService.DebuggingDisposables);
                })
                .AddTo(this);
        }
    }
}
