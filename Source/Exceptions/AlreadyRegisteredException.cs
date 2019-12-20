using System;
using Innoactive.Hub.Training.SceneObjects;

namespace Innoactive.Hub.Training.Exceptions
{
    public class AlreadyRegisteredException : TrainingException
    {
        public AlreadyRegisteredException(ISceneObject obj) : base(string.Format("Could not register SceneObject {0}, it's already registered!", obj))
        {
        }
    }
}
