using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace GigaCreation.Tools
{
    public class TimeScalePrinter : DebugTextPrinter
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
