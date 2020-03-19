using System;
using System.Linq;
using Innoactive.Creator.Core.Utils;
using UnityEngine;
using UnityEditor.Callbacks;

namespace Innoactive.CreatorEditor.Configuration
{
    /// <summary>
    /// Configurator to set the training editor configuration which is used by the training creation editor tools (like Step Inspector).
    /// </summary>
    public static class EditorConfigurator
    {
        private static readonly IEditorConfiguration editorConfiguration;

        public static IEditorConfiguration Instance
        {
            get { return editorConfiguration; }
        }

        static EditorConfigurator()
        {
            Type[] lowestPriorityTypes = new Type[] { typeof(DefaultEditorConfiguration) };
            Type[] definitions = ReflectionUtils.GetFinalImplementationsOf<IEditorConfiguration>(lowestPriorityTypes).ToArray();

            if (definitions.Except(lowestPriorityTypes).Count() > 1)
            {
                string listOfDefinitions = string.Join("', '", definitions.Select(definition => definition.FullName).ToArray());
                Debug.LogErrorFormat(
                    "There is more than one final implementation of training editor configurations in this Unity project: '{0}'."
                    + " Remove all editor configurations except for '{1}' and the one you want to use."
                    + " '{2}' was selected as current editor configuration.",
                    listOfDefinitions,
                    typeof(DefaultEditorConfiguration).FullName,
                    definitions.First().FullName
                );
            }

            editorConfiguration = (IEditorConfiguration)ReflectionUtils.CreateInstanceOfType(definitions.First());

            LoadAllowedMenuItems();
        }

        [DidReloadScripts]
        private static void LoadAllowedMenuItems()
        {
            if (string.IsNullOrEmpty(Instance.AllowedMenuItemsSettingsAssetPath))
            {
                Instance.AllowedMenuItemsSettings = new AllowedMenuItemsSettings();
            }
            else
            {
                Instance.AllowedMenuItemsSettings = AllowedMenuItemsSettings.Load();
            }
        }
    }
}
