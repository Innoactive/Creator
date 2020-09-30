using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Behaviors;
using Innoactive.Creator.Core.Conditions;
using UnityEngine;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <inheritdoc/>
    public class ContextResolver : IContextResolver
    {
        /// <inheritdoc/>
        public IContext FindContext(IEntity entity, ICourse course)
        {
            if (entity is ICourse)
            {
                return new CourseContext(course);
            }

            if (entity is IChapter)
            {
                return new ChapterContext((IChapter)entity, new CourseContext(course));
            }

            if (entity is IStep)
            {
                IStep step = (IStep) entity;
                foreach (IChapter chapter in course.Data.Chapters)
                {
                    if (chapter.Data.Steps.Contains(step))
                    {
                        return new StepContext(step,
                            new ChapterContext(chapter,
                                new CourseContext(course)));
                    }
                }
            }

            if (entity is IBehavior behavior)
            {
                foreach (IChapter chapter in course.Data.Chapters)
                {
                    foreach (IStep step in chapter.Data.Steps)
                    {
                        if (step.Data.Behaviors.Data.Behaviors.Contains(behavior))
                        {
                            return new BehaviorContext(behavior,
                                new StepContext(step,
                                    new ChapterContext(chapter,
                                        new CourseContext(course))));
                        }
                    }
                }
            }

            if (entity is ITransition)
            {
                foreach (IChapter chapter in course.Data.Chapters)
                {
                    foreach (IStep step in chapter.Data.Steps)
                    {
                        foreach (ITransition transition in step.Data.Transitions.Data.Transitions)
                        {
                            if (step.Data.Transitions.Data.Transitions.Contains(entity as ITransition))
                            {
                                return new TransitionContext(transition,
                                    new StepContext(step,
                                        new ChapterContext(chapter,
                                            new CourseContext(course))));
                            }
                        }
                    }
                }
            }

            if (entity is ICondition condition)
            {
                foreach (IChapter chapter in course.Data.Chapters)
                {
                    foreach (IStep step in chapter.Data.Steps)
                    {
                        foreach (ITransition transition in step.Data.Transitions.Data.Transitions)
                        {
                            if (transition.Data.Conditions.Contains(condition))
                            {
                                return new ConditionContext(condition,
                                    new TransitionContext(transition,
                                        new StepContext(step,
                                            new ChapterContext(chapter,
                                                new CourseContext(course)))));
                            }
                        }
                    }
                }
            }

            Debug.LogError($"Could not resolve the context of Entity: {entity.GetType().Name}");
            return null;
        }
    }
}
