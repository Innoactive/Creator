using System;
using UnityEngine;
using VPG.Creator.Core.Utils;

namespace VPG.Creator.Core
{
    /// <summary>
    /// Base factory interface for <see cref="IEntity"/> objects.
    /// </summary>
    public static class EntityFactory
    {
        /// <summary>
        /// Creates a new <see cref="IStep"/> with given <paramref name="name"/>, <paramref name="position"/> and, if there is any valid <see cref="PostProcessEntity{T}"/>, executes corresponding post processing.
        /// </summary>
        public static IStep CreateStep(string name, Vector2 position = default)
        {
            IStep step = StepFactory.Instance.Create(name);
            step.StepMetadata.Position = position;
            PostProcessEntity<IStep>(step);

            return step;
        }

        /// <summary>
        /// Creates a new <see cref="ITransition"/> and, if there is any valid <see cref="PostProcessEntity{T}"/>, executes corresponding post processing.
        /// </summary>
        public static ITransition CreateTransition()
        {
            ITransition transition = TransitionFactory.Instance.Create();
            PostProcessEntity<ITransition>(transition);

            return transition;
        }

        /// <summary>
        /// Creates a new <see cref="IChapter"/> with given <paramref name="name"/> and, if there is any valid <see cref="PostProcessEntity{T}"/>, executes corresponding post processing.
        /// </summary>
        public static IChapter CreateChapter(string name)
        {
            IChapter chapter = ChapterFactory.Instance.Create(name);
            PostProcessEntity<IChapter>(chapter);

            return chapter;
        }

        /// <summary>
        /// Creates a new <see cref="ICourse"/> with given <paramref name="name"/> and, if there is any valid <see cref="PostProcessEntity{T}"/>, executes corresponding post processing.
        /// </summary>
        public static ICourse CreateCourse(string name)
        {
            ICourse course = CourseFactory.Instance.Create(name);
            PostProcessEntity<ICourse>(course);

            return course;
        }

        private static void PostProcessEntity<T>(IEntity entity) where T : IEntity
        {
            foreach (Type postprocessingType in ReflectionUtils.GetConcreteImplementationsOf<EntityPostProcessing<T>>())
            {
                if (ReflectionUtils.CreateInstanceOfType(postprocessingType) is EntityPostProcessing<T> postProcessing)
                {
                    postProcessing.Execute((T) entity);
                }
            }
        }
    }
}
