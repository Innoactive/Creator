using Innoactive.CreatorEditor.UI.Graphics.Renderers;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Graphics
{
    public class CreateTransitionButtonRenderer : MulticoloredGraphicalElementRenderer<CreateTransitionButton>
    {
        public CreateTransitionButtonRenderer(CreateTransitionButton owner, WorkflowEditorColorPalette colorPalette) : base(owner, colorPalette)
        {
        }

        public override void Draw()
        {
            EditorDrawingHelper.DrawCircle(Owner.Position, Owner.BoundingBox.width / 2f, CurrentColor);

            GUIStyle labelStyle = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = TextColor },
                wordWrap = false,
            };

            GUI.Label(Owner.BoundingBox, "+", labelStyle);
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
