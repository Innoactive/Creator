using Innoactive.Creator.Core.Configuration;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.Input
{
    /// <summary>
    /// Static utility class which provides methods to help managing assets and functionalities of the new input system.
    /// </summary>
    public static class InputEditorUtils
    {
#if ENABLE_INPUT_SYSTEM
        /// <summary>
        /// Copies the custom key bindings into the project by using the default one.
        /// </summary>
        public static void CopyCustomKeyBindingAsset()
        {
            UnityEngine.InputSystem.InputActionAsset defaultBindings = Resources.Load<UnityEngine.InputSystem.InputActionAsset>(RuntimeConfigurator.Configuration.DefaultInputActionAssetPath);

            if (AssetDatabase.IsValidFolder("Assets/Resources") == false)
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }

            if (AssetDatabase.IsValidFolder("Assets/Resources/KeyBindings") == false)
            {
                AssetDatabase.CreateFolder("Assets/Resources", "KeyBindings");
            }

            AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(defaultBindings),
                $"Assets/Resources/{RuntimeConfigurator.Configuration.CustomInputActionAssetPath}.inputactions");

            AssetDatabase.Refresh();

            RuntimeConfigurator.Configuration.CurrentInputActionAsset =
                Resources.Load<UnityEngine.InputSystem.InputActionAsset>(RuntimeConfigurator.Configuration.CustomInputActionAssetPath);
        }

        /// <summary>
        /// Checks if the custom key bindings are loaded.
        /// </summary>
        public static bool UsesCustomKeyBindingAsset()
        {
            return AssetDatabase.GetAssetPath(RuntimeConfigurator.Configuration.CurrentInputActionAsset)
                .Equals("Assets/Resources" + RuntimeConfigurator.Configuration.CustomInputActionAssetPath);
        }

        /// <summary>
        /// Opens the key binding editor.
        /// </summary>
        public static void OpenKeyBindingEditor()
        {
            if (UsesCustomKeyBindingAsset() == false)
            {
                CopyCustomKeyBindingAsset();
            }
            AssetDatabase.OpenAsset(RuntimeConfigurator.Configuration.CurrentInputActionAsset);
        }
#else
        /// <summary>
        /// Copies the custom key bindings into the project by using the default one.
        /// </summary>
        public static void CopyCustomKeyBindingAsset()
        {
            Debug.LogError("Error, no implementation for the old input system");
        }

        /// <summary>
        /// Checks if the custom key bindings are loaded.
        /// </summary>
        public static bool UsesCustomKeyBindingAsset()
        {
            Debug.LogError("Error, no implementation for the old input system");
            return false;
        }

        /// <summary>
        /// Opens the key binding editor.
        /// </summary>
        public static void OpenKeyBindingEditor()
        {
            Debug.LogError("Error, no implementation for the old input system");
        }
#endif
    }
}
