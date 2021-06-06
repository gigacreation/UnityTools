using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;

namespace GcTools.Editor
{
    [CustomGridBrush(true, true, true, "GcBrush")]
    public class GcBrush : GridBrush
    {
    }

    [CustomEditor(typeof(GcBrush))]
    public class GcBrushEditor : GridBrushEditor
    {
        private readonly Color _textColor = new Color(1f, 0.5f, 0.75f);
        private readonly Color _backgroundColor = new Color(0.13f, 0.13f, 0.17f, 0.5f);
        private readonly Vector3Int _textOffset = new Vector3Int(2, 2, 0);

        public override void OnPaintSceneGUI(
            GridLayout grid, GameObject brushTarget, BoundsInt position, GridBrushBase.Tool tool, bool executing
        )
        {
            base.OnPaintSceneGUI(grid, brushTarget, position, tool, executing);

            string labelText = "Pos: " + position.position;

            if ((position.size.x > 1) || (position.size.y > 1))
            {
                labelText += ", Size: " + position.size;
            }

            var rectOffset = new RectOffset(10, 0, 5, 0);

            var style = new GUIStyle
            {
                normal =
                {
                    textColor = _textColor,
                    background = Texture2D.whiteTexture
                },
                fontStyle = FontStyle.BoldAndItalic,
                margin = rectOffset,
                padding = rectOffset
            };

            Color storedBackgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = _backgroundColor;

            Handles.Label(grid.CellToWorld(position.position + _textOffset), labelText, style);

            GUI.backgroundColor = storedBackgroundColor;
        }
    }
}
