using System.Linq;
using Innoactive.Creator.Core;
using UnityEngine;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <inheritdoc/>
    public class ContextResolver : IContextResolver
    {
        /// <inheritdoc/>
        public IContext FindContext(IData data, ICourse course)
        {
            if (data is ICourseData)
            {
                return new CourseContext(course.Data);
            }

            if (data is IChapterData)
            {
                return new ChapterContext((IChapterData)data, new CourseContext(course.Data));
            }

            if (data is IStepData)
            {
                IStepData stepData = (IStepData) data;
                foreach (IChapter chapter in course.Data.Chapters)
                {
                    IStep parentStep = chapter.Data.Steps.FirstOrDefault(step => step.Data == stepData);
                    if (parentStep != null)
                    {
                        return new StepContext(stepData,
                            new ChapterContext(chapter.Data,
                                new CourseContext(course.Data)));
                    }
                }
            }

            Debug.LogError($"Could not resolve the context of Entity: {data.GetType().Name}");
            return null;
        }
    }
}
