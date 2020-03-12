using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Innoactive.Hub.Training.Behaviors;
using Innoactive.Hub.Training.Conditions;
using Innoactive.Hub.Training.Editors.Utils.Serialization;
using Innoactive.Hub.Training.Utils;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace Innoactive.Hub.Training.Editors.Configuration
{
    /// <summary>
    /// Configuration class for menu items.
    /// It manages whether a behavior or condition should be displayed in the Step Inspector or not.
    /// Can be serialized.
    /// </summary>
    [DataContract(IsReference = true)]
    public class AllowedMenuItemsSettings
    {
        private IList<Menu.Item<IBehavior>> behaviorMenuItems;
        private IList<Menu.Item<ICondition>> conditionMenuItems;

        [DataMember]
        public IDictionary<string, bool> SerializedBehaviorSelections;

        [DataMember]
        public IDictionary<string, bool> SerializedConditionSelections;

        [JsonConstructor]
        public AllowedMenuItemsSettings() : this (new Dictionary<string, bool>(), new Dictionary<string, bool>())
        {
        }

        public AllowedMenuItemsSettings(IDictionary<string, bool> behaviors, IDictionary<string, bool> serializedConditions)
        {
            SerializedBehaviorSelections = behaviors;
            SerializedConditionSelections = serializedConditions;

            UpdateWithAllBehaviorsAndConditionsInProject();
        }

        /// <summary>
        /// Returns all active behavior menu items.
        /// </summary>
        public IEnumerable<Menu.Item<IBehavior>> GetBehaviorMenuOptions()
        {
            if (behaviorMenuItems == null)
            {
                behaviorMenuItems = SetupItemList<Menu.Item<IBehavior>>(SerializedBehaviorSelections)
                    .OrderByAlphaNumericNaturalSort(item => item.DisplayedName.text)
                    .ToList();
            }

            // ReSharper disable once AssignNullToNotNullAttribute
            return behaviorMenuItems.Where(item => SerializedBehaviorSelections[item.GetType().AssemblyQualifiedName]);
        }

        /// <summary>
        /// Returns all active condition menu items.
        /// </summary>
        public IEnumerable<Menu.Item<ICondition>> GetConditionMenuOptions()
        {
            if (conditionMenuItems == null)
            {
                conditionMenuItems = SetupItemList<Menu.Item<ICondition>>(SerializedConditionSelections)
                    .OrderByAlphaNumericNaturalSort(item => item.DisplayedName.text)
                    .ToList();
            }

            // ReSharper disable once AssignNullToNotNullAttribute
            return conditionMenuItems.Where(item => SerializedConditionSelections[item.GetType().AssemblyQualifiedName]);
        }

        /// <summary>
        /// Serializes the <paramref name="settings"/> object and saves it into a configuration file at a default path.
        /// </summary>
        /// <exception cref="NullReferenceException">Thrown when parameter is null.</exception>
        public static bool Save(AllowedMenuItemsSettings settings)
        {
            if (string.IsNullOrEmpty(EditorConfigurator.Instance.AllowedMenuItemsSettingsAssetPath))
            {
                Debug.LogFormat("The property \"AllowedMenuItemsSettingsAssetPath\" of the " +
                    "current editor configuration is not set. Thus, the AllowedMenuItemsSettings cannot be saved.");
                return false;
            }

            const string assets = "Assets/";
            string path = EditorConfigurator.Instance.AllowedMenuItemsSettingsAssetPath;

            if (path.StartsWith(assets) == false)
            {
                Debug.LogErrorFormat("The property \"AllowedMenuItemsSettingsAssetPath\" of the current editor configuration" +
                    " is invalid. It has to start with \"{0}\". Current value: \"{1}\"", assets, path);
                return false;
            }

            try
            {
                if (settings == null)
                {
                    throw new NullReferenceException("The allowed menu items settings file cannot be saved "
                        + "because the settings are null.");
                }

                string serialized = JsonEditorConfigurationSerializer.Serialize(settings);

                string fullPath = string.Format("{0}/{1}", Application.dataPath, path.Remove(0, assets.Length));
                string directoryPath = Path.GetDirectoryName(fullPath);

                if (string.IsNullOrEmpty(directoryPath))
                {
                    Debug.LogErrorFormat("No valid directory path found in path \"{0}\". The property \"AllowedMenuItemSettingsAssetPath\"" +
                        " of the current editor configuration is invalid. Current value: \"{1}\"", fullPath, path);
                    return false;
                }

                if (Directory.Exists(directoryPath) == false)
                {
                    Directory.CreateDirectory(directoryPath);
                }

                StreamWriter writer = new StreamWriter(fullPath, false);
                writer.Write(serialized);
                writer.Close();

                AssetDatabase.ImportAsset(path);

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// Loads and returns the <see cref="AllowedMenuItemsSettings"/> object from the default configuration file location.
        /// If the <see cref="AllowedMenuItemsSettingsAssetPath"/> in the editor configuration is empty or null,
        /// it returns an empty <see cref="AllowedMenuItemsSettings"/> object.
        /// </summary>
        public static AllowedMenuItemsSettings Load()
        {
            string path = EditorConfigurator.Instance.AllowedMenuItemsSettingsAssetPath;
            if (string.IsNullOrEmpty(path))
            {
                Debug.Log("The property \"AllowedMenuItemsSettingsAssetPath\" of the current editor " +
                    "configuration is not set. Therefore, it cannot be loaded. A new \"AllowedMenuItemsSettings\" " +
                    "object with all found conditions and behaviors was returned.");
                return new AllowedMenuItemsSettings();
            }

            TextAsset settings = (TextAsset)AssetDatabase.LoadAssetAtPath(path, typeof(TextAsset));

            if (settings != null)
            {
                return JsonEditorConfigurationSerializer.Deserialize(settings.text);
            }

            Debug.LogFormat("There is no serialized \"AllowedMenuItemsSettings\" object saved at \"{0}\". A new " +
                "\"AllowedMenuItemsSettings\" object with all found conditions and behaviors was returned.", path);
            return new AllowedMenuItemsSettings();
        }

        private IList<T> SetupItemList<T>(IDictionary<string, bool> userSelections)
        {
            if (userSelections == null)
            {
                return null;
            }

            IList<T> itemList = new List<T>();

            foreach (KeyValuePair<string, bool> keyValuePair in userSelections)
            {
                Type type = ReflectionUtils.GetTypeFromAssemblyQualifiedName(keyValuePair.Key);

                if (type == null)
                {
                    continue;
                }

                try
                {
                    T instance = (T)ReflectionUtils.CreateInstanceOfType(type);
                    itemList.Add(instance);
                }
                catch (Exception)
                {
                    // Type is abstract or has no parameterless constructor, ignore it.
                }
            }

            return itemList;
        }

        private void UpdateWithAllBehaviorsAndConditionsInProject()
        {
            IEnumerable<Type> implementedBehaviors = ReflectionUtils.GetConcreteImplementationsOf<Menu.Item<IBehavior>>();

            foreach (Type type in implementedBehaviors)
            {
                if (type.AssemblyQualifiedName == null || SerializedBehaviorSelections.ContainsKey(type.AssemblyQualifiedName))
                {
                    continue;
                }

                SerializedBehaviorSelections.Add(type.AssemblyQualifiedName, true);
            }

            IEnumerable<Type> implementedConditions = ReflectionUtils.GetConcreteImplementationsOf<Menu.Item<ICondition>>();

            foreach (Type type in implementedConditions)
            {
                if (type.AssemblyQualifiedName == null || SerializedConditionSelections.ContainsKey(type.AssemblyQualifiedName))
                {
                    continue;
                }

                SerializedConditionSelections.Add(type.AssemblyQualifiedName, true);
            }
        }
    }
}
