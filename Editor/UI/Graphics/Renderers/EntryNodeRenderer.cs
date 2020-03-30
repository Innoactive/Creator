using Innoactive.CreatorEditor.UI.Graphics.Renderers;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Graphics
{
    public class EntryNodeRenderer : ColoredGraphicalElementRenderer<EntryNode>
    {
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
        }
    }
}
