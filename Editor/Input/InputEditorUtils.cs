using Innoactive.Creator.Core.Configuration;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Innoactive.CreatorEditor.Input
{
    /// <summary>
    /// Static utility class which provides methods to help managing assets and functionalities of the new input system.
    /// </summary>
    public static class InputEditorUtils
    {
        /// <summary>
        /// Copies the custom key bindings into the project by using the default one.
        /// </summary>
        public static void CopyCustomKeyBindingAsset()
        {
            InputActionAsset defaultBindings = Resources.Load<InputActionAsset>(RuntimeConfigurator.Configuration.DefaultInputActionAssetPath);

            AssetDatabase.CreateFolder("Assets", "Resources");
            AssetDatabase.CreateFolder("Assets/Resources", "KeyBindings");

            AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(defaultBindings),
                $"Assets/Resources/{RuntimeConfigurator.Configuration.CustomInputActionAssetPath}.inputactions");

            AssetDatabase.Refresh();

            RuntimeConfigurator.Configuration.CurrentInputActionAsset =
                Resources.Load<InputActionAsset>(RuntimeConfigurator.Configuration.CustomInputActionAssetPath);
        }

        /// <summary>
        /// Checks if the custom key bindings are loaded.
        /// </summary>
        public static bool UsesCustomKeyBindingAsset()
        {
            return AssetDatabase.GetAssetPath(RuntimeConfigurator.Configuration.CurrentInputActionAsset).Equals("Assets/Resources" + RuntimeConfigurator.Configuration.CustomInputActionAssetPath);
        }
    }
}
