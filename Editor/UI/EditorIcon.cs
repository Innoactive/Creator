using System;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI
{
    /// <summary>
    /// Utility class to help with different unity color schemes (dark/light). Takes care about loading the given icon,
    /// will add _light or _dark to the given path. If only one icon is found it will be used for both skins. If no icon
    /// is found there will be an NullReferenceException thrown.
    ///
    /// DO NOT ADD FILE ENDINGS TO THE PATH!
    /// </summary>
    [Serializable]
    internal class EditorIcon
    {
        private const string LightTextureFileEnding = "_light";
        private const string DarkTextureFileEnding = "_dark";

        [SerializeField]
        private string path;

        private Texture2D iconLight;
        public Texture2D IconLight
        {
            get
            {
                if (iconLight == null)
                {
                    LoadIcons();
                }

                return iconLight;
            }
        }

        private Texture2D iconDark;
        public Texture2D IconDark
        {
            get
            {
                if (iconDark == null)
                {
                    LoadIcons();
                }

                return iconDark;
            }
        }

        /// <summary>
        /// Returns the texture of the icon, depending on the skin used.
        /// </summary>
        public Texture Texture
        {
            get { return EditorGUIUtility.isProSkin ? IconLight : IconDark; }
        }

        public EditorIcon()
        {
            // Used in serialization, dont use this.
        }

        public EditorIcon(string path)
        {
            this.path = path;
        }

        private void LoadIcons()
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new NullReferenceException("Given Path for EditorIcon is empty.");
            }

            iconLight = Resources.Load<Texture2D>(path + LightTextureFileEnding);
            iconDark = Resources.Load<Texture2D>(path + DarkTextureFileEnding);

            if (iconLight == null && iconDark == null)
            {
                string msg = string.Format("Texture with path: '{0}' couldn't be loaded. No {1} nor {2} version found!", path, LightTextureFileEnding, DarkTextureFileEnding);
                Debug.LogError(msg);
                throw new NullReferenceException(msg);
            }

            if (iconLight == null)
            {
                if (EditorGUIUtility.isProSkin)
                {
                    Debug.LogWarningFormat("No texture found for path: {0}", path + LightTextureFileEnding);
                }
                iconLight = iconDark;
            }
            else if (iconDark == null)
            {
                if (!EditorGUIUtility.isProSkin)
                {
                    Debug.LogWarningFormat("No texture found for path: {0}", path + DarkTextureFileEnding);
                }
                iconDark = iconLight;
            }
        }
    }
}
