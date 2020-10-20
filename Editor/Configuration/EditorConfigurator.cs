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
        private static readonly BaseEditorConfiguration editorConfiguration;

        public static BaseEditorConfiguration Instance
        {
            get { return editorConfiguration; }
        }

        static EditorConfigurator()
        {
            Type[] lowestPriorityTypes = { typeof(BaseEditorConfiguration) };
            Type[] definitions = ReflectionUtils.GetFinalImplementationsOf<IEditorConfiguration>(lowestPriorityTypes).ToArray();

            if (definitions.Except(lowestPriorityTypes).Count() > 1)
            {
                string listOfDefinitions = string.Join("', '", definitions.Select(definition => definition.FullName).ToArray());
                Debug.LogErrorFormat(
                    "There is more than one final implementation of training editor configurations in this Unity project: '{0}'."
                    + " Remove all editor configurations except for '{1}' and the one you want to use."
                    + " '{2}' was selected as current editor configuration.",
                    listOfDefinitions,
                    typeof(BaseEditorConfiguration).FullName,
                    definitions.First().FullName
                );
            }

            IEditorConfiguration config = (IEditorConfiguration)ReflectionUtils.CreateInstanceOfType(definitions.First());
            if (config is BaseEditorConfiguration configuration)
            {
                editorConfiguration = configuration;
            }
            else
            {
                editorConfiguration = new EditorConfigWrapper(config);
            }

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
