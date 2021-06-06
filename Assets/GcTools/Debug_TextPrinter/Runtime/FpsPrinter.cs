// Original code from https://qiita.com/toRisouP/items/1d0682e7a35cdb04bc38

using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace GcTools
{
    public class FpsPrinter : DebugTextPrinter
    {
        private const int BufferSize = 5;

        protected override void Initialize()
        {
            base.Initialize();

            DebugCore.IsDebugMode
                .Where(x => x)
                .Subscribe(_ =>
                {
                    Label.SetText("- fps");

                    this.UpdateAsObservable()
                        .Select(__ => Time.deltaTime)
                        .Buffer(BufferSize, 1)
                        .Select(y => 1f / y.Average())
                        .ToReadOnlyReactiveProperty()
                        .Subscribe(fps =>
                        {
                            Label.SetText($"{fps:F1} fps");
                        })
                        .AddTo(DebugCore.Disposables);
                })
                .AddTo(this);
        }
    }
}
