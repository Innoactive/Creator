using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using VPG.Creator.Core.IO;

namespace VPG.Creator.Core.Internationalization
{
    /// <summary>
    /// This class handles retrieving and managing localization files.
    /// </summary>
    public static class Localization
    {
        private const string MissingTranslationText = "[Missing Translation]";

        /// <summary>
        /// Dictionary with all registered localizations.
        /// </summary>
        public static Dictionary<string, string> entries = new Dictionary<string, string>();

        /// <summary>
        /// Loads the given config into the localization database, will overwrite everything.
        /// </summary>
        /// <param name="config">Localization config which should be used.</param>
        /// <param name="language">Language which should be loaded.</param>
        /// <param name="course">Name of the course which is ran.</param>
        public static void LoadLocalization(LocalizationConfig config, string language, string course)
        {
            entries = LocalizationReader.Load(config, language, course);
        }

        /// <summary>
        /// Loads the given config into the localization, will overwrite in there.
        /// </summary>
        /// <param name="config">Localization config which should be used.</param>
        /// <param name="parameters">Additional parameters used for resolving the path.</param>
        public static void LoadLocalization(LocalizationConfig config, Dictionary<string, string> parameters)
        {
            entries = LocalizationReader.Load(config, parameters);
        }

        /// <summary>
        /// Loads a localization file from given <paramref name="path"/>.
        /// </summary>
        /// <param name="path">File path, relative to the StreamingAssets or the platform persistent data folder.</param>
        /// <returns>True if the localization file was loaded successfully.</returns>
        [Obsolete("Please load localization with the LocalizationConfig.")]
        public static bool LoadLocalization(string path)
        {
            if (FileManager.Exists(path) == false)
            {
                return false;
            }

            try
            {
                string jsonValue = FileManager.ReadAllText(path);
                entries = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonValue);
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
            }

            return false;
        }

        /// <summary>
        /// Returns true when the key exists in the dictionary, empty values will still return true.
        /// </summary>
        public static bool ContainsTranslationFor(string key)
        {
            return entries.ContainsKey(key);
        }

        /// <summary>
        /// Retrieves a localized string from loaded <see cref="entries"/>.
        /// </summary>
        public static string Get(LocalizedString query)
        {
            return GetFormat(query.Key, query.DefaultText, query.FormatParams);
        }

        /// <summary>
        /// Retrieves a localized string from loaded <see cref="entries"/>.
        /// </summary>
        public static string Get(string key, string defaultString = MissingTranslationText)
        {
            if (entries.TryGetValue(key, out string res))
            {
                return res;
            }

            if (defaultString == MissingTranslationText)
            {
                Debug.LogWarningFormat("Missing Translation for Key \"{0}\"", key);
            }
            return defaultString;
        }

        /// <summary>
        /// Retrieves a localized string from loaded <see cref="entries"/>.
        /// </summary>
        public static string GetFormat(string key, params object[] args)
        {
            return string.Format(Get(key, MissingTranslationText), args);
        }

        /// <summary>
        /// Retrieves a formatted localized string from loaded <see cref="entries"/>.
        /// </summary>
        public static string GetFormat(string key, string defaultString, params object[] args)
        {
            return string.Format(Get(key, defaultString), args);
        }

        /// <summary>
        /// Retrieves a formatted localized string from loaded <see cref="entries"/>.
        /// </summary>
        public static string GetFormat(string key, string[] args)
        {
            return string.Format(Get(key, MissingTranslationText), args);
        }

        /// <summary>
        /// Retrieves a formatted localized string from loaded <see cref="entries"/>.
        /// </summary>
        public static string GetFormat(string key, string defaultString, string[] args)
        {
            return string.Format(Get(key, defaultString), args);
        }
    }
}
