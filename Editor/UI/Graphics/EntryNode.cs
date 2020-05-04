using Innoactive.CreatorEditor.UI.Graphics.Renderers;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Graphics
{
    /// <summary>
    /// Represents the beginning of a training in the training workflow.
    /// </summary>
    internal class EntryNode : EditorNode
    {
        private static readonly Vector2 size = new Vector2(24f, 24f);
        private readonly GraphicalElementRenderer renderer;

        /// <inheritdoc />
        public override GraphicalElementRenderer Renderer
        {
            get
            {
                return renderer;
            }
        }

        /// <inheritdoc />
        public override Rect BoundingBox
        {
            get
            {
                return new Rect(Position - size / 2f, size);
            }
        }

        public EntryNode(EditorGraphics owner) : base(owner, true)
        {
            renderer = new EntryNodeRenderer(this, owner.ColorPalette);
        }
    }
}
