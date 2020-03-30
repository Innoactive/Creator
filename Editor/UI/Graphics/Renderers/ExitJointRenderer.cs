using Innoactive.CreatorEditor.UI.Graphics.Renderers;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Graphics
{
    public class ExitJointRenderer : MulticoloredGraphicalElementRenderer<ExitJoint>
    {
        public ExitJointRenderer(ExitJoint owner, WorkflowEditorColorPalette colorPalette) : base(owner, colorPalette)
        {
        }

        public override void Draw()
        {
            EditorDrawingHelper.DrawCircle(Owner.Position, Owner.BoundingBox.width / 2f, CurrentColor);

            if (Owner.DragDelta.magnitude > Owner.BoundingBox.width / 2f)
            {
                EditorDrawingHelper.DrawArrow(Owner.Position, Owner.Position + Owner.DragDelta, CurrentColor, 40f, 10f);
            }
        }

        public override Color NormalColor
        {
            get
            {
                return ColorPalette.Transition;
            }
        }

        protected override Color PressedColor
        {
            get
            {
                return ColorPalette.Primary;
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
    }
}
