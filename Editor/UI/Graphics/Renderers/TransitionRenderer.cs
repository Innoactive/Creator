using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Graphics.Renderers
{
    /// <summary>
    /// Renderer for transition between editor nodes.
    /// </summary>
    internal class TransitionRenderer : ColoredGraphicalElementRenderer<TransitionElement>
    {
        ///<inheritdoc />
        public override Color NormalColor
        {
            get
            {
                return ColorPalette.Transition;
            }
        }

        public TransitionRenderer(TransitionElement owner, WorkflowEditorColorPalette colorPalette) : base(owner, colorPalette)
        {
        }

        ///<inheritdoc />
        public override void Draw()
        {
            EditorDrawingHelper.DrawPolyline(Owner.PolylinePoints, CurrentColor);
        }
    }
}
