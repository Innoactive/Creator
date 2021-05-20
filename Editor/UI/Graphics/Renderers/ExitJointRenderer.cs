using VPG.Editor.UI.Graphics.Renderers;
using UnityEngine;

namespace VPG.Editor.UI.Graphics
{
    internal class ExitJointRenderer : MulticoloredGraphicalElementRenderer<ExitJoint>
    {
        private static EditorIcon outgoingIcon = new EditorIcon("icon_arrow_right");
        private int iconSize = 15;

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

            Rect iconRect = new Rect(Owner.Position.x - iconSize / 2f, Owner.Position.y - iconSize / 2f, iconSize, iconSize);
            EditorDrawingHelper.DrawTexture(iconRect, outgoingIcon.Texture, Color.gray);
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
