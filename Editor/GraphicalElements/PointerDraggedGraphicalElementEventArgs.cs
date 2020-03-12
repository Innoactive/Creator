using UnityEngine;

namespace Innoactive.Hub.Training.Editors.GraphicalElements
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
