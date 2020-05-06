using System;
using Innoactive.Creator.Core;
using Innoactive.CreatorEditor.UI.Graphics.Renderers;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Graphics
{
    /// <summary>
    /// Represents step node of the workflow editor.
    /// </summary>
    internal class StepNode : EditorNode
    {
        private static readonly Vector2 initialSize = new Vector2(100f, 24f);
        private Vector2 size = initialSize;

        private bool isLastSelectedStep;
        private IStep step;

        //The last step that was clicked on is selected.
        private readonly GraphicalElementRenderer renderer;

        public CreateTransitionButton CreateTransitionButton { get; private set; }

        /// <summary>
        /// Currently displayed step instance. If this step node is selected, the same step instance is used by a Step Inspector as well.
        /// </summary>
        public IStep Step
        {
            get
            {
                return step;
            }
            set
            {
                step = value;
                if (step != null)
                {
                    Position = step.StepMetadata.Position;
                }
            }
        }

        /// <summary>
        /// Indicates that this node represents last step selected by user.
        /// </summary>
        public bool IsLastSelectedStep
        {
            get
            {
                return isLastSelectedStep;
            }
            set
            {
                if (value != isLastSelectedStep)
                {
                    isLastSelectedStep = value;
                    if (SelectedChanged != null)
                    {
                        SelectedChanged.Invoke(value);
                    }
                }
            }
        }

        public event Action<bool> SelectedChanged;

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
                return new Rect(Position - initialSize / 2f, size);
            }
        }

        /// <inheritdoc />
        public StepNode(EditorGraphics graphics, IStep step) : base(graphics, true)
        {
            Step = step;
            renderer = new StepNodeRenderer(this, graphics.ColorPalette);

            EntryJoints.Add(new EntryJoint(graphics, this) { RelativePosition = new Vector2(-size.x / 2f, 0f) });

            CreateTransitionButton = new CreateTransitionButton(graphics, this) { RelativePosition = new Vector2(size.x / 2f, 0) };
        }

        public ExitJoint AddExitJoint()
        {
            ExitJoint toAdd = new ExitJoint(Graphics, this);
            toAdd.RelativePosition = new Vector2(size.x / 2f, ExitJoints.Count * 20f);
            size.y += 20f;
            ExitJoints.Add(toAdd);

            CreateTransitionButton.RelativePosition = new Vector2(size.x / 2f, (ExitJoints.Count) * 20f);

            return toAdd;
        }
    }
}
