using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Innoactive.Hub
{
    public static class Localization
    {
        private static readonly Common.Logging.ILog logger = Logging.LogManager.GetLogger("Localization");

        public delegate void LocaleChanged(string newLocale, string oldLocale);
        public static LocaleChanged OnLocaleChanged;

        public static Dictionary<string, string> entries = new Dictionary<string, string>();

        private const string missingTranslationText = "[Missing Translation]";

        public static void LoadLocalization(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    entries = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path));
                }
                catch (Exception ex)
                {
                    logger.Warn(ex);
                }
            }

            if (entries == null)
            {
                entries = new Dictionary<string, string>();
            }
        }

        public static string Get(LocalizedString query)
        {
            return GetFormat(query.Key, query.DefaultText, query.FormatParams);
        }

        public static string Get(string nameWithScope, string defaultString = missingTranslationText)
        {
            string res;
            if (entries.TryGetValue(nameWithScope, out res))
            {
                return res;
            }

            if (defaultString == missingTranslationText)
            {
                logger.WarnFormat("Missing Translation for Key \"{0}\"", nameWithScope);
            }
            return defaultString;
        }

        public static string GetFormat(string nameWithScope, params object[] args)
        {
            return string.Format(Get(nameWithScope), args);
        }

        public static string GetFormat(string nameWithScope, string defaultString, params object[] args)
        {
            return string.Format(Get(nameWithScope, defaultString), args);
        }

        public static string GetFormat(string nameWithScope, string[] args)
        {
            return string.Format(Get(nameWithScope), args);
        }

        public static string GetFormat(string nameWithScope, string defaultString, string[] args)
        {
            return string.Format(Get(nameWithScope, defaultString), args);
        }
    }
}
