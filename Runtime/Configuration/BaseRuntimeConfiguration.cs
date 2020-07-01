using System;
using Innoactive.Creator.Core.Configuration.Modes;
using Innoactive.Creator.Core.IO;
using Innoactive.Creator.Core.RestrictiveEnvironment;
using Innoactive.Creator.Core.SceneObjects;
using Innoactive.Creator.Core.Serialization;
using Innoactive.Creator.Core.Serialization.NewtonsoftJson;
using UnityEngine;

namespace Innoactive.Creator.Core.Configuration
{
#pragma warning disable 0618
    public abstract class BaseRuntimeConfiguration : IRuntimeConfiguration
    {
#pragma warning restore 0618
        private ISceneObjectRegistry sceneObjectRegistry;

        /// <inheritdoc />
        public virtual ISceneObjectRegistry SceneObjectRegistry
        {
            get
            {
                if (sceneObjectRegistry == null)
                {
                    sceneObjectRegistry = new SceneObjectRegistry();
                }

                return sceneObjectRegistry;
            }
        }

        /// <inheritdoc />
        public ICourseSerializer Serializer { get; set; } = new NewtonsoftJsonCourseSerializer();

        /// <inheritdoc />
        public IModeHandler Modes { get; protected set; }

        public abstract TrainingSceneObject Trainee { get; }
        public abstract AudioSource InstructionPlayer { get; }

        public StepLockHandlingStrategy StepLockHandling { get; set; } = new DefaultStepLockHandling();

        /// <inheritdoc />
        public virtual ICourse LoadCourse(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Given path is null or empty!");
            }

            byte[] serialized = FileManager.Read(path);
            return Serializer.CourseFromByteArray(serialized);
        }
    }
}
