using Innoactive.Creator.Core;
using Innoactive.CreatorEditor.UI.Graphics.Renderers;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Graphics
{
    public class CreateTransitionButton : GraphicalElement
    {
        private readonly Vector2 size = new Vector2(16f, 16f);

        private readonly GraphicalElementRenderer renderer;
        private Rect boundingBox;
        private int layer;

        public CreateTransitionButton(EditorGraphics editorGraphics, GraphicalElement parent = null) : base(editorGraphics, true, parent)
        {
            renderer = new CreateTransitionButtonRenderer(this, editorGraphics.ColorPalette);
        }

        public override GraphicalElementRenderer Renderer
        {
            get
            {
                return renderer;
            }
        }

        public override Rect BoundingBox
        {
            get
            {
                return new Rect(Position - size / 2f, size);
            }
        }

        public override int Layer
        {
            get
            {
                return 90;
            }
        }
    }
}
