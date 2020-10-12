using Innoactive.Creator.Core.Behaviors;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Base context for objects of type <see cref="IBehaviorData"/>.
    /// </summary>
    public class BehaviorContext : EntityDataContext<IBehaviorData>
    {
        public BehaviorContext(IBehaviorData behavior, StepContext parent) : base(behavior, parent) { }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (Parent != null)
            {
                return $"{Parent.ToString()} > [{GetName()}]";
            }
            return $"[{GetName()}]";
        }

        private string GetName()
        {
            if (string.IsNullOrEmpty(EntityData.Name))
            {
                return EntityData.ToString();
            }
            return EntityData.Name;
        }
    }
}
