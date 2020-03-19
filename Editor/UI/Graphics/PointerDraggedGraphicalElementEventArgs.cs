using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Graphics
{
    public class PointerDraggedGraphicalElementEventArgs : PointerGraphicalElementEventArgs
    {
        public Vector2 PointerDelta
        {
            get;
            private set;
        }

        public PointerDraggedGraphicalElementEventArgs(Vector2 pointerPosition, Vector2 pointerDelta) : base(pointerPosition)
        {
            PointerDelta = pointerDelta;
        }
    }
}
