﻿using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Innoactive.Creator.Core.Internationalization
{
    /// <summary>
    /// This class handles retrieving and managing localization files.
    /// </summary>
    public static class Localization
    {
        /// <summary>
        /// Dictionary with all registered localizations.
        /// </summary>
        public static Dictionary<string, string> entries = new Dictionary<string, string>();

        private const string missingTranslationText = "[Missing Translation]";

        /// <summary>
        /// Loads a localization file from given <paramref name="path"/>.
        /// </summary>
        /// <returns>True if the localization file was loaded successfully.</returns>
        public static bool LoadLocalization(string path)
        {
            if (File.Exists(path) == false)
            {
                return false;
            }

            try
            {
                entries = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path));
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
            }

            return false;
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
        public static string Get(string nameWithScope, string defaultString = missingTranslationText)
        {
            if (entries.TryGetValue(nameWithScope, out string res))
            {
                return res;
            }

            if (defaultString == missingTranslationText)
            {
                Debug.LogWarningFormat("Missing Translation for Key \"{0}\"", nameWithScope);
            }
            return defaultString;
        }

        /// <summary>
        /// Retrieves a localized string from loaded <see cref="entries"/>.
        /// </summary>
        public static string GetFormat(string nameWithScope, params object[] args)
        {
            return string.Format(Get(nameWithScope), args);
        }

        /// <summary>
        /// Retrieves a formatted localized string from loaded <see cref="entries"/>.
        /// </summary>
        public static string GetFormat(string nameWithScope, string defaultString, params object[] args)
        {
            return string.Format(Get(nameWithScope, defaultString), args);
        }

        /// <summary>
        /// Retrieves a formatted localized string from loaded <see cref="entries"/>.
        /// </summary>
        public static string GetFormat(string nameWithScope, string[] args)
        {
            return string.Format(Get(nameWithScope), args);
        }

        /// <summary>
        /// Retrieves a formatted localized string from loaded <see cref="entries"/>.
        /// </summary>
        public static string GetFormat(string nameWithScope, string defaultString, string[] args)
        {
            return string.Format(Get(nameWithScope, defaultString), args);
        }
    }
}
