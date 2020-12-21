using Innoactive.CreatorEditor.UI.Graphics.Renderers;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Graphics
{
    internal class EntryNodeRenderer : MulticoloredGraphicalElementRenderer<EntryNode>
    {
        /// <inheritdoc />
        public override Color NormalColor
        {
            get
            {
                if (Owner.IsDragging)
                {
                    return SelectedColor;
                }
                return ColorPalette.ElementBackground;
            }
        }

        protected override Color PressedColor
        {
            get
            {
                return SelectedColor;
            }
        }

        protected override Color HoveredColor
        {
            get
            {
                return ColorPalette.Secondary;
            }
        }

        protected override Color TextColor
        {
            get
            {
                return ColorPalette.Text;
            }
        }

        protected virtual Color SelectedColor
        {
            get
            {
                return ColorPalette.Primary;
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
