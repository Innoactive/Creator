using System;

namespace VPG.Editor.UI.Drawers
{
    /// <summary>
    /// Marks a training drawer as a default drawer for a given type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    internal class DefaultTrainingDrawerAttribute : Attribute
    {
        /// <summary>
        /// Objects of which type can be processed  by this training drawer.
        /// </summary>
        public Type DrawableType { get; private set; }

        public DefaultTrainingDrawerAttribute(Type type)
        {
            DrawableType = type;
        }
    }
}
