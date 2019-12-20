using System;
using System.Collections.ObjectModel;
using Innoactive.Hub.Training.Editors.GraphicalElements.Renderers;
using Innoactive.Hub.Training.Editors.Utils;
using UnityEngine;

namespace Innoactive.Hub.Training.Editors.GraphicalElements
{
    /// <summary>
    /// Represents transition arrow between two steps.
    /// </summary>
    public class TransitionElement : GraphicalElement
    {
        private readonly TransitionRenderer renderer;

        private Rect boundingBox;
        private ExitJoint start;

        /// <summary>
        /// Amount of segments a bezier curve consists of.
        /// </summary>
        public static int CurveSegmentCount = 33;

        /// <summary>
        /// Points forming the bezier curve.
        /// </summary>
        public ReadOnlyCollection<Vector2> PolylinePoints { get; private set; }

        /// <summary>
        /// Joint that arrow is pointing at.
        /// </summary>
        public EntryJoint Destination { get; private set; }

        /// <summary>
        /// Joint from which transition starts.
        /// </summary>
        public ExitJoint Start
        {
            get { return start; }

            private set
            {
                start = value;
                Parent = start;
            }
        }

        /// <inheritdoc />
        public override Rect BoundingBox
        {
            get { return boundingBox; }
        }

        /// <inheritdoc />
        public override int Layer
        {
            get { return 80; }
        }

        /// <inheritdoc />
        public override GraphicalElementRenderer Renderer
        {
            get { return renderer; }
        }

        /// <inheritdoc />
        public TransitionElement(EditorGraphics editorGraphics, ExitJoint start, EntryJoint destination) : base(editorGraphics, false, start)
        {
            Destination = destination;
            Start = start;
            renderer = new TransitionRenderer(this, editorGraphics.ColorPalette);
        }

        public override void HandleDeregistration()
        {
            base.HandleDeregistration();
            Start = null;
            Destination = null;
        }

        public override void Layout()
        {
            base.Layout();

            RelativePosition = (Destination.Position - Start.Position) / 2f;

            Vector2[] controlPoints = BezierCurveHelper.CalculateControlPointsForTransition(Start.Position, Destination.Position, Start.Parent.BoundingBox, Destination.Parent.BoundingBox);
            PolylinePoints = Array.AsReadOnly(BezierCurveHelper.CalculateDeCastejauCurve(Start.Position, controlPoints[0], controlPoints[1], Destination.Position, CurveSegmentCount));

            boundingBox = BezierCurveHelper.CalculateBoundingBoxForPolyline(PolylinePoints);
        }
    }
}
