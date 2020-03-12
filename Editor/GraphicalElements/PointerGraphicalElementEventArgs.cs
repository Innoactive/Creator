using UnityEngine;

namespace Innoactive.Hub.Training.Editors.GraphicalElements
{
    public class PointerGraphicalElementEventArgs : GraphicalElementEventArgs
    {
        public Vector2 PointerPosition { get; private set; }

        public PointerGraphicalElementEventArgs(Vector2 pointerPosition)
        {
            PointerPosition = pointerPosition;
        }
    }
}