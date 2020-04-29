using Microsoft.Win32;
using System;
using UnityEngine;

namespace Innoactive.CreatorEditor.Analytics
{
    internal static class RegistryUtils
    {
        public const string CreatorBaseEntry = "HKEY_CURRENT_USER\\Software\\Innoactive\\Creator";
        public const string AnalyticsEntry = CreatorBaseEntry + "\\Analytics";

        /// <summary>
        /// Tries to fetch a registry entry from HKCU (current user), if not found the default value will be returned.
        /// </summary>
        private static T GetEntry<T>(string key, string entry, T defaultValue)
        {
            try
            {
                object rawValue = Registry.GetValue(key, entry, defaultValue);
                if (rawValue != null)
                {
                    return (T) rawValue;

                }
            }
            catch (Exception)
            {
                Debug.LogFormat("No read access to registry, not able to fetch entry: '{0}'", entry);
            }

            return defaultValue;
        }

        /// <summary>
        /// Tries to fetch a registry entry from the given key, if not found the default value will be returned.
        /// <typeparam name="T">Type of the entry, there are only string, enum, int, long or unknown.</typeparam>
        /// <param name="key">Name of key</param>
        /// <param name="valueName">Name of the value</param>
        /// <param name="defaultValue">Value which is returned if no entry was found.</param>
        /// </summary>
        public static T GetRegistryEntry<T>(string key, string valueName, T defaultValue)
        {
            if (typeof(T).IsEnum)
            {
                string value = GetEntry<string>(key, valueName, null);
                if (string.IsNullOrEmpty(value))
                {
                    return defaultValue;
                }

                return (T)Enum.Parse(typeof(T), value);
            }

            return GetEntry<T>(key, valueName, defaultValue);
        }

        /// <summary>
        /// Adds given value to the given entry
        /// </summary>
        /// <typeparam name="T">Type of the entry, there are only string, enum, int, long or unknown.</typeparam>
        /// <param name="key">Name of key</param>
        /// <param name="valueName">Name of the value</param>
        public static void SetRegistryEntry<T>(string key, string valueName, T value)
        {
            try
            {
                if (value is string)
                {
                    Registry.SetValue(key, valueName, value, RegistryValueKind.String);
                }
                else if (value is Enum)
                {
                    Registry.SetValue(key, valueName, value.ToString(), RegistryValueKind.String);
                }
                else if (value is int)
                {
                    Registry.SetValue(key, valueName, value, RegistryValueKind.DWord);
                }
                else if (value is long)
                {
                    Registry.SetValue(key, valueName, value, RegistryValueKind.QWord);
                }
                else
                {
                    Registry.SetValue(key, valueName, value, RegistryValueKind.Unknown);
                }
            }
            catch (Exception)
            {
                Debug.LogFormat("No write access to registry, not able to write entry: '{0}'", valueName);
            }
        }
    }
}
