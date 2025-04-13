// Original code from https://qiita.com/toRisouP/items/1d0682e7a35cdb04bc38

using System;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace GigaCreation.Tools.Debugging.TextDisplays
{
    public class DebugTextPreferenceFps : DebugTextPreferenceBase
    {
        private const int BufferSize = 5;

        private string _fpsText;

        protected override string LabelText => _fpsText;

        protected override IDisposable ActivateLabelUpdate()
        {
            _fpsText = "- fps";
            UpdateLabel();

            return this
                .UpdateAsObservable()
                .Select(static _ => Time.deltaTime)
                .Buffer(BufferSize, 1)
                .Select(static y => 1f / y.Average())
                .ToReadOnlyReactiveProperty()
                .Subscribe(fps =>
                {
                    _fpsText = $"{fps:F1} fps";
                    UpdateLabel();
                })
                .AddTo(this);
        }
    }
}
