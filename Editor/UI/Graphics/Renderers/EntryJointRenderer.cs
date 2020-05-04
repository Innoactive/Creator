using Innoactive.CreatorEditor.UI.Graphics.Renderers;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Graphics
{
    internal class EntryJointRenderer : MulticoloredGraphicalElementRenderer<EntryJoint>
    {
        public EntryJointRenderer(EntryJoint owner, WorkflowEditorColorPalette colorPalette) : base(owner, colorPalette)
        {
        }

        public override void Draw()
        {
            Rect drawInRect = new Rect(Owner.Position - Owner.BoundingBox.size / 4f, Owner.BoundingBox.size / 2f);

            EditorDrawingHelper.DrawCircle(Owner.Position, drawInRect.width / 2f, CurrentColor);

            GUIStyle labelStyle = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = TextColor },
                wordWrap = false,
            };

            GUI.Label(drawInRect, ">", labelStyle);
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
