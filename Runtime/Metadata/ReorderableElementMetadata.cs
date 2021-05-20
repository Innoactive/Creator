namespace VPG.Core.UI.Drawers.Metadata
{
    /// <summary>
    /// Metadata to make <see cref="VPG.Core.Attributes.ReorderableListOfAttribute"/> reorderable.
    /// </summary>
    public class ReorderableElementMetadata
    {
        /// <summary>
        /// Determines, whether the entity must be moved up in the list.
        /// </summary>
        public bool MoveUp { get; set; }

        /// <summary>
        /// Determines, whether the entity must be moved down in the list.
        /// </summary>
        public bool MoveDown { get; set; }

        /// <summary>
        /// Determines, whether the entity is the first one in the list.
        /// </summary>
        public bool IsFirst { get; set; }

        /// <summary>
        /// Determines, whether the entity is the last one in the list.
        /// </summary>
        public bool IsLast { get; set; }
    }
}
