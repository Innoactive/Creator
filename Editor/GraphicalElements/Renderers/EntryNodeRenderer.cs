using Innoactive.Hub.Training.Editors.GraphicalElements.Renderers;
using Innoactive.Hub.Training.Editors.Utils;
using UnityEngine;

namespace Innoactive.Hub.Training.Editors.GraphicalElements
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
