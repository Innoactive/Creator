using System.IO;
using UnityEditor;
using UnityEngine;

namespace Innoactive.Creator.Core.Runtime.Utils
{
    public class SettingsObject<T> : ScriptableObject where T : ScriptableObject, new()
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (EditorUtility.IsDirty(instance))
                {
                    instance = null;
                }

                if (instance == null)
                {
                    instance = Load();
                }

                return instance;
            }
        }

        private static T Load()
        {
            T settings = Resources.Load<T>(typeof(T).Name);
            if (settings == null)
            {
                if (!Directory.Exists("Assets/Resources"))
                {
                    Directory.CreateDirectory("Assets/Resources");
                }
                // Create an instance
                settings = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(settings, $"Assets/Resources/{typeof(T).Name}.asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                return Resources.Load<T>(typeof(T).Name);
            }
            return settings;
        }

        /// <summary>
        /// Saves the Creator settings.
        /// </summary>
        public void Save()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
