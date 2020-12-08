using Innoactive.CreatorEditor.UI.Graphics.Renderers;
using UnityEngine;
using UnityEditor;

namespace Innoactive.CreatorEditor.UI.Graphics
{
    internal class CreateTransitionButtonRenderer : MulticoloredGraphicalElementRenderer<CreateTransitionButton>
    {
        private Texture2D plusIcon = EditorGUIUtility.IconContent("CreateAddNew@2x").image as Texture2D;
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
            GUI.DrawTexture(Owner.BoundingBox, plusIcon);
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
