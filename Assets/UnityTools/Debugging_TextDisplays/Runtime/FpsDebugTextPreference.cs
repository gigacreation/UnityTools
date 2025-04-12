// Original code from https://qiita.com/toRisouP/items/1d0682e7a35cdb04bc38

using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace GigaCreation.Tools.Debugging.TextDisplays
{
    public class FpsDebugTextPreference : DebugTextPreference
    {
        private const int BufferSize = 5;

        protected override void Initialize()
        {
            base.Initialize();

            DebugManager
                .IsDebugMode
                .Where(static x => x)
                .Subscribe(_ =>
                {
                    SetTextToLabel("- fps");

                    this
                        .UpdateAsObservable()
                        .Select(static _ => Time.deltaTime)
                        .Buffer(BufferSize, 1)
                        .Select(static y => 1f / y.Average())
                        .ToReadOnlyReactiveProperty()
                        .Subscribe(fps =>
                        {
                            SetTextToLabel($"{fps:F1} fps");
                        })
                        .AddTo(DebugManager.DebugDisposables);
                })
                .AddTo(this);
        }
    }
}
