using Innoactive.CreatorEditor.UI.Graphics.Renderers;
using UnityEngine;
using UnityEditor;

namespace Innoactive.CreatorEditor.UI.Graphics
{
    internal class EntryNodeRenderer : ColoredGraphicalElementRenderer<EntryNode>
    {
        private static int LabelWidth = 30;
        private static int LabelHight = 50;
    
        /// <inheritdoc />
        public override Color NormalColor
        {
            get
            {
                return ColorPalette.ElementBackground;
            }
        }

        public EntryNodeRenderer(EntryNode owner, WorkflowEditorColorPalette colorPalette) : base(owner, colorPalette)
        {
        }

        /// <inheritdoc />
        public override void Draw()
        {
            EditorDrawingHelper.DrawCircle(Owner.BoundingBox.center, Owner.BoundingBox.size.x / 2f, CurrentColor);
            Rect StartLabelRect = new Rect(Owner.BoundingBox.center.x - LabelWidth / 2f, Owner.BoundingBox.center.y, LabelWidth, LabelHight);
            EditorGUI.LabelField(StartLabelRect, "Start", EditorStyles.label);
        }
    }
}
