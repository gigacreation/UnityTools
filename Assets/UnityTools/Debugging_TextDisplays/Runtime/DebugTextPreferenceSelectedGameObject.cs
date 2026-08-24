using System.Text;
using UnityEngine.EventSystems;

namespace GigaCreation.Tools.Debugging.TextDisplays
{
    public class DebugTextPreferenceSelectedGameObject : DebugTextPreferenceBase
    {
        private readonly StringBuilder _builder = new();

        protected override string LabelText
        {
            get
            {
                _builder.Clear();
                _builder.Append("Focus: ");

                var go = EventSystem.current.currentSelectedGameObject;

                if (go == null)
                {
                    _builder.Append("-");
                }
                else
                {
                    _builder.Append(go.name);
                    _builder.Append("[");
                    _builder.Append(go.transform.GetSiblingIndex());
                    _builder.Append("]");
                }

                return _builder.ToString();
            }
        }
    }
}
