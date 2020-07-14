using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Graphics
{
    internal class PointerGraphicalElementEventArgs : GraphicalElementEventArgs
    {
        public Vector2 PointerPosition { get; private set; }

        public PointerGraphicalElementEventArgs(Vector2 pointerPosition)
        {
            PointerPosition = pointerPosition;
        }
    }
}
