using BuildTimestampDisplay;
using UnityEngine;

namespace GigaCreation.Tools
{
    public class BuildTimestampPrinter : DebugTextPrinter
    {
        [SerializeField] private BuildTimestamp _buildTimestamp;
        [SerializeField] private string _format = "yyyy/MM/dd HH:mm:ss";
        [SerializeField] private float _utcOffsetHours;

        protected override void Initialize()
        {
            base.Initialize();

            Label.SetText(
                _buildTimestamp
                    ? _buildTimestamp.ToString(_format, _utcOffsetHours)
                    : "BuildTimestamp が見つかりません。"
            );
        }
    }
}
