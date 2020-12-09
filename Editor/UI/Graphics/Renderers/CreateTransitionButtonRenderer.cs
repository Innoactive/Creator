using Innoactive.CreatorEditor.UI.Graphics.Renderers;
using UnityEngine;
using UnityEditor;

namespace Innoactive.CreatorEditor.UI.Graphics
{
    internal class CreateTransitionButtonRenderer : MulticoloredGraphicalElementRenderer<CreateTransitionButton>
    {
        private static EditorIcon plusIcon = new EditorIcon("icon_add");
        private int iconSize = 14;

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
            GUI.color = Color.gray;
            Rect iconBoundingBox = new Rect(Owner.Position.x - (iconSize / 2f), Owner.Position.y - (iconSize / 2f), iconSize, iconSize);
            GUI.DrawTexture(iconBoundingBox, plusIcon.Texture);
            GUI.color = CurrentColor;
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
