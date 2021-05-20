using System.Collections.Generic;
using System.Linq;
using VPG.Core.Behaviors;
using VPG.Core.Properties;
using VPG.Core.RestrictiveEnvironment;
using VPG.Core.SceneObjects;

namespace VPG.Core
{
    /// <summary>
    /// Collection of <see cref="ISceneObject"/>s that can be locked and unlocked during a step.
    /// Additionally, checks if objects are automatically or manually unlocked.
    /// </summary>
    internal class LockableObjectsCollection
    {
        private List<LockablePropertyData> toUnlock;

        private Step.EntityData data;

        public List<ISceneObject> SceneObjects { get; set; } = new List<ISceneObject>();

        public LockableObjectsCollection(Step.EntityData entityData)
        {
            toUnlock = PropertyReflectionHelper.ExtractLockablePropertiesFromStep(entityData).ToList();
            data = entityData;

            CreateSceneObjects();
        }

        private void CreateSceneObjects()
        {
            CleanProperties();

            foreach (LockablePropertyReference propertyReference in data.ToUnlock)
            {
                AddSceneObject(propertyReference.Target.Value);
            }

            foreach (LockablePropertyData propertyData in toUnlock)
            {
                AddSceneObject(propertyData.Property.SceneObject);
            }
        }

        public void AddSceneObject(ISceneObject sceneObject)
        {
            if (SceneObjects.Contains(sceneObject) == false)
            {
                SceneObjects.Add(sceneObject);
                SortSceneObjectList();
            }
        }

        private void SortSceneObjectList()
        {
            SceneObjects.Sort((obj1, obj2) => obj1.GameObject.ToString().CompareTo(obj2.GameObject.ToString()));
        }

        public void RemoveSceneObject(ISceneObject sceneObject)
        {
            if (SceneObjects.Remove(sceneObject))
            {
                data.ToUnlock = data.ToUnlock.Where(property =>
                {
                    if (property.GetProperty() == null)
                    {
                        return false;
                    }

                    return property.GetProperty().SceneObject != sceneObject;
                }).ToList();
            }
        }

        public bool IsInManualUnlockList(LockableProperty property)
        {
            foreach (LockablePropertyReference lockableProperty in data.ToUnlock)
            {
                if (property == lockableProperty.GetProperty())
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsUsedInAutoUnlock(ISceneObject sceneObject)
        {
            return toUnlock.Any(propertyData => propertyData.Property.SceneObject == sceneObject);
        }

        public bool IsInAutoUnlockList(LockableProperty property)
        {
            foreach (LockablePropertyData lockableProperty in toUnlock)
            {
                if (property == lockableProperty.Property)
                {
                    return true;
                }
            }

            return false;
        }

        public void Remove(LockableProperty property)
        {
            data.ToUnlock = data.ToUnlock.Where(reference => reference.GetProperty() != property).ToList();
        }

        public void Add(LockableProperty property)
        {
            data.ToUnlock = data.ToUnlock.Union(new [] {new LockablePropertyReference(property), }).ToList();
        }

        private void CleanProperties()
        {
            data.ToUnlock = data.ToUnlock.Where(reference => reference.Target.IsEmpty() == false).ToList();
        }
    }
}
