using VPG.Creator.Core.SceneObjects;

namespace VPG.Creator.Core.Exceptions
{
    public class NameNotUniqueException : TrainingException
    {
        public NameNotUniqueException(ISceneObject entity) : base(string.Format("Could not register Item with name '{0}', name already in use", entity.UniqueName))
        {
        }
    }
}
