using System.Linq;
using UnityEditor;
using UnityEngine;

namespace VPG.Editor.UI
{
    /// <summary>
    /// Helper editor class that allows retrieving or drawing a logo that
    /// fits the current Unity color theme.
    /// </summary>
    internal static class LogoEditorHelper
    {
        /// <summary>
        /// Filename of light logo (used for dark Unity theme)
        /// </summary>
        private const string LogoDarkFilename = "logo-claim-black";

        /// <summary>
        /// Filename of dark logo (used for light Unity theme)
        /// </summary>
        private const string LogoLightFilename = "logo-claim-white";

        private static Texture2D textureCache = null;

        /// <summary>
        /// Returns a common texture containing the correct logo
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
        /// Draws the logo with the specified width in the current GUI context
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
                return LogoLightFilename;
            }
            else
            {
                return LogoDarkFilename;
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
