using System;
using VPG.Creator.Core.SceneObjects;
using UnityEngine;

namespace VPG.Creator.Core.Configuration
{
    /// <summary>
    /// This wrapper is used for <see cref="IRuntimeConfiguration"/> configurations, which
    /// ensures that the old interface based configurations can still be used.
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
