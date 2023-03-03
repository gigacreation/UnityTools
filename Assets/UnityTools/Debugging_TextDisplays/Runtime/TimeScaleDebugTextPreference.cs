using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace GigaCreation.Tools.Debugging.TextDisplays
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
                            SetTextToLabel($"TimeScale: {Time.timeScale}");
                        })
                        .AddTo(DebugService.DebugDisposables);
                })
                .AddTo(this);
        }
    }
}
