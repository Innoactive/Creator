using System;
using Innoactive.Creator.Core.SceneObjects;
using UnityEngine;

namespace Innoactive.Creator.Core.Configuration
{
    /// <summary>
    /// This wrapper for <see cref="IRuntimeConfiguration"/> configurations ensures
    /// that old interface based configurations can still be used.
    /// </summary>
    [Obsolete("Helper class to ensure backwards compatibility.")]
    public class RuntimeConfigWrapper : BaseRuntimeConfiguration
    {
        /// <summary>
        /// Wrapped IRuntimeConfiguration.
        /// </summary>
        public readonly IRuntimeConfiguration Configuration;

        public RuntimeConfigWrapper(IRuntimeConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <inheritdoc />
        public override TrainingSceneObject Trainee => Configuration.Trainee;

        /// <inheritdoc />
        public override AudioSource InstructionPlayer => Configuration.InstructionPlayer;

        /// <inheritdoc />
        public override ISceneObjectRegistry SceneObjectRegistry => Configuration.SceneObjectRegistry;

        /// <inheritdoc />
        public override ICourse LoadCourse(string path)
        {
            return Configuration.LoadCourse(path);
        }
    }
}
