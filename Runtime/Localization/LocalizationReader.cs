using System.Collections.Generic;
using UnityEngine;

namespace VPG.Creator.Core.Internationalization
{
    /// <summary>
    /// Includes all logic which allows to load a localized dictionary configured by a <see cref="LocalizationConfig"/>.
    /// </summary>
    public static class LocalizationReader
    {
        /// <summary>
        /// Key which will be replaced for the current language.
        /// </summary>
        public const string KeyLanguage = "{language}";

        /// <summary>
        /// Key which will be replaced for the current fallback language.
        /// </summary>
        public const string KeyFallbackLanguage = "{fallback_language}";

        /// <summary>
        /// Key which will be replaced for the current selected course name.
        /// </summary>
        public const string KeyCourseName = "{course}";

        internal static Dictionary<string, string> Load(LocalizationConfig config, string language, string course, Dictionary<string, string> parameters = null)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, string>();
            }

            AddMissingParameter(KeyLanguage, language, parameters);
            AddMissingParameter(KeyCourseName, course, parameters);

            return Load(config, parameters);
        }

        internal static Dictionary<string, string> Load(LocalizationConfig config, string language, Dictionary<string, string> parameters = null)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, string>();
            }

            AddMissingParameter(KeyLanguage, language, parameters);

            return Load(config, parameters);
        }

        internal static Dictionary<string, string> Load(LocalizationConfig config, Dictionary<string, string> parameters)
        {
            AddMissingParameter(KeyFallbackLanguage, LanguageSettings.Instance.DefaultLanguage, parameters);

            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (LocalizationConfig.LocalizationSource source in config.Sources)
            {
                AddOrReplace(result, LoadSource(source, parameters));
            }

            return result;
        }

        private static Dictionary<string, string> LoadSource(LocalizationConfig.LocalizationSource source, Dictionary<string, string> parameters)
        {
            string path = ResolvePath(source.Path, parameters);
            if (source.ResourceType == LocalizationConfig.ResourceType.Resources)
            {
                return LocalizationUtils.LoadFromResource(path);
            }

            if (source.ResourceType == LocalizationConfig.ResourceType.StreamingAssets)
            {
                return LocalizationUtils.LoadFromStreamingAssets(path);
            }

            return new Dictionary<string, string>();
        }

        private static void AddOrReplace(Dictionary<string, string> result, Dictionary<string, string> dictionaryAddition)
        {
            foreach (string key in dictionaryAddition.Keys)
            {
                if (result.ContainsKey(key))
                {
                    result[key] = dictionaryAddition[key];
                }
                else
                {
                    result.Add(key, dictionaryAddition[key]);
                }
            }
        }

        /// <summary>
        /// Replaces the variables in the given path according to the dictionary.
        /// </summary>
        /// <param name="path">The path which will be resolved.</param>
        /// <param name="parameters">Key value pairs used for this.</param>
        internal static string ResolvePath(string path, Dictionary<string, string> parameters)
        {
            foreach (string key in parameters.Keys)
            {
                path = path.Replace(key, parameters[key]);
            }

            return path;
        }

        private static void AddMissingParameter(string key, string value, Dictionary<string, string> parameters)
        {
            if (parameters.ContainsKey(key) == false)
            {
                parameters.Add(key, value);
            }
        }
    }
}
