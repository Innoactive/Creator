using System;

namespace Innoactive.Hub.Training.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DrawingPriorityAttribute : Attribute
    {
        /// <summary>
        /// Lower goes first.
        /// </summary>
        public int Priority { get; private set; }

        public DrawingPriorityAttribute(int priority)
        {
            Priority = priority;
        }
    }
}
