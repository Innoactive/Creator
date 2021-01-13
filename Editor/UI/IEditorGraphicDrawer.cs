using UnityEngine;

namespace Innoactive.CreatorEditor.UI
{
    /// <summary>
    /// Allows to draws over the normal EditorGraphics.
    /// </summary>
    internal interface IEditorGraphicDrawer
    {
        /// <summary>
        /// Draw priority, lower numbers will be drawn first.
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Your draw call.
        /// </summary>
        void Draw(Rect windowRect);
    }
}
