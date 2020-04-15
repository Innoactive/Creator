using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI
{
    /// <summary>
    /// Helper editor class that allows retrieving or drawing a Innoactive Logo that
    /// fits the current Unity color theme.
    /// </summary>
    internal static class InnoactiveLogoEditorHelper
    {
        /// <summary>
        /// Filename of light Innoactive Hub logo (used for dark Unity theme)
        /// </summary>
        private const string InnoactiveHubLogoDarkFilename = "innoactive-hub-logo-claim-black";

        /// <summary>
        /// Filename of dark Innoactive Hub logo (used for light Unity theme)
        /// </summary>
        private const string InnoactiveHubLogoLightFilename = "innoactive-hub-logo-claim-white";

        private static Texture2D textureCache = null;

        /// <summary>
        /// Returns a common texture containing the correct Hub Logo
        /// </summary>
        public static Texture2D GetLogoTexture()
        {
            if (textureCache == null)
            {
                textureCache = AssetDatabase.LoadAssetAtPath<Texture2D>(GetLogoAssetPath());
            }
            return textureCache;
        }

        /// <summary>
        /// Draws the Innoactive logo with the specified width in the current GUI context
        /// </summary>
        public static void DrawLogo(float width)
        {
            Texture2D logo = GetLogoTexture();
            if (logo)
            {
                Rect rect = GUILayoutUtility.GetRect(width, 150, GUI.skin.box);
                GUI.DrawTexture(rect, logo, ScaleMode.ScaleToFit);
            }
        }

        /// <summary>
        /// Returns the file name of the correct logo
        /// </summary>
        public static string GetFilename()
        {
            if (EditorGUIUtility.isProSkin)
            {
                return InnoactiveHubLogoLightFilename;
            }
            else
            {
                return InnoactiveHubLogoDarkFilename;
            }
        }

        /// <summary>
        /// Returns the asset path of the correct logo
        /// </summary>
        public static string GetLogoAssetPath()
        {
            string[] results = AssetDatabase.FindAssets(GetFilename());
            if (results != null && results.Length > 0)
            {
                return AssetDatabase.GUIDToAssetPath(results.First());
            }
            return null;
        }
    }
}
