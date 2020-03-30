using System.Collections.Generic;

namespace Innoactive.CreatorEditor.UI.Graphics
{
    /// <summary>
    /// Base class for Entry and Exit nodes.
    /// </summary>
    public abstract class EditorNode : GraphicalElement
    {
        /// <summary>
        /// List of entry joints, to which incoming Transitions can connect. Since workflow is linear for now, there is only one joint per editor node.
        /// </summary>
        public List<EntryJoint> EntryJoints { get; private set; }

        /// <summary>
        /// List of exit joints, to which outcoming Transitions can connect. Since workflow is linear for now, there is only one joint per editor node.
        /// </summary>
        public List<ExitJoint> ExitJoints { get; private set; }

        /// <inheritdoc />
        public override int Layer
        {
            get
            {
                return 0;
            }
        }

        /// <inheritdoc />
        protected EditorNode(EditorGraphics owner, bool isReceivingEvents) : base(owner, isReceivingEvents)
        {
            EntryJoints = new List<EntryJoint>();
            ExitJoints = new List<ExitJoint>();
        }
    }
}
