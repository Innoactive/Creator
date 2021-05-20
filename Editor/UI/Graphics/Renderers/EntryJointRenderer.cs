using VPG.Editor.UI.Graphics.Renderers;
using UnityEngine;

namespace VPG.Editor.UI.Graphics
{
    internal class EntryJointRenderer : MulticoloredGraphicalElementRenderer<EntryJoint>
    {
        private static EditorIcon ingoingIcon = new EditorIcon("icon_arrow_right");
        private int iconSize = 15;

        public EntryJointRenderer(EntryJoint owner, WorkflowEditorColorPalette colorPalette) : base(owner, colorPalette)
        {
        }

        public override void Draw()
        {
            EditorDrawingHelper.DrawCircle(Owner.Position, Owner.BoundingBox.width / 4f, CurrentColor);
            Rect iconRect = new Rect(Owner.Position.x - iconSize / 2f, Owner.Position.y - iconSize / 2f, iconSize, iconSize);
            EditorDrawingHelper.DrawTexture(iconRect, ingoingIcon.Texture, Color.gray);
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
                return ColorPalette.ElementBackground;
            }
        }
    }
}
