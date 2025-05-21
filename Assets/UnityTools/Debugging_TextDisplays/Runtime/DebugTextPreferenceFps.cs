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
        [Space]
        [SerializeField] private int _bufferSize = 5;
        [SerializeField] private bool _ignoreTimeScale;

        private string _fpsText;

        protected override string LabelText => _fpsText;

        protected override IDisposable ActivateLabelUpdate()
        {
            _fpsText = "- fps";
            UpdateLabel();

            return this
                .UpdateAsObservable()
                .Select(_ => _ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime)
                .Buffer(_bufferSize, 1)
                .Select(static buffer => 1f / buffer.Average())
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
