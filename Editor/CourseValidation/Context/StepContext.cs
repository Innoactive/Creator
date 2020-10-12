﻿using Innoactive.Creator.Core;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Base context for objects of type <see cref="IStep"/>.
    /// </summary>
    public class StepContext : EntityDataContext<IStepData>
    {
        public StepContext(IStepData step, ChapterContext parent) : base(step, parent) { }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (Parent != null)
            {
                return $"{Parent.ToString()} > [{EntityData.Name}]";
            }
            return $"[{EntityData.Name}]";
        }
    }
}
