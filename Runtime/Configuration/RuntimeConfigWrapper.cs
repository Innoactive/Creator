using Innoactive.Creator.Core.SceneObjects;
using JetBrains.Annotations;
using UnityEngine;

namespace Innoactive.Creator.Core.Configuration
{
    public class RuntimeConfigWrapper : BaseRuntimeConfiguration
    {
#pragma warning disable 0618
        public readonly IRuntimeConfiguration Configuration;

        public RuntimeConfigWrapper(IRuntimeConfiguration configuration)
        {
            Configuration = configuration;
        }
#pragma warning restore 0618
        public override TrainingSceneObject Trainee => Configuration.Trainee;

        public override AudioSource InstructionPlayer => Configuration.InstructionPlayer;

        public override ISceneObjectRegistry SceneObjectRegistry => Configuration.SceneObjectRegistry;

        public override ICourse LoadCourse(string path)
        {
            return Configuration.LoadCourse(path);
        }
    }
}
