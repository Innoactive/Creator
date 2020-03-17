using Innoactive.Creator.Core.SceneObjects;
using Innoactive.Creator.Core.SceneObjects.Properties;

namespace Innoactive.Creator.Core.Utils
{
    public static class TrainingReferenceUtils
    {
        public static string GetNameFrom(ISceneObjectProperty property)
        {
            if (property == null)
            {
                return null;
            }

            if (property.SceneObject == null)
            {
                return null;
            }

            return property.SceneObject.UniqueName;
        }

        public static string GetNameFrom(ISceneObject sceneObject)
        {
            if (sceneObject == null)
            {
                return null;
            }

            return sceneObject.UniqueName;
        }
    }
}
