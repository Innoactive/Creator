using UnityEngine;

namespace Innoactive.Creator.Core
{
    /// <summary>
    /// Implementation of <see cref="IMetadata"/> adapted for <see cref="IStep"/> data.
    /// </summary>
    public class StepMetadata : IMetadata
    {
        /// <summary>
        /// Graphical position of current <see cref="IStep"/> on the 'Workflow Editor'.
        /// </summary>
        public Vector2 Position { get; set; }
    }
}
