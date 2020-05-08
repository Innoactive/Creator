using Microsoft.Win32;
using System;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.Analytics
{
    internal static class EditorPrefExtensions
    {
        public static T GetEnum<T>(string key, T defaultValue)
        {
            string value = EditorPrefs.GetString(key, null);
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            return (T)Enum.Parse(typeof(T), value);
        }

        public static void SetEnum<T>(string key, T value)
        {
            EditorPrefs.SetString(key, value.ToString());
        }
    }
}
