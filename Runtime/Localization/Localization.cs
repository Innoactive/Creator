using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Innoactive.Creator.Core.IO;

namespace Innoactive.Creator.Core.Internationalization
{
    /// <summary>
    /// This class handles retrieving and managing localization files.
    /// </summary>
    public static class Localization
    {
        public class LocalizationEventArgs : EventArgs
        {
            public readonly Dictionary<string, string> Entries;

            public readonly string Language;
            public readonly string DefaultLanguage;

            public LocalizationEventArgs(Dictionary<string, string> entries, string language, string defaultLanguage)
            {
                Entries = entries;
                Language = language;
                DefaultLanguage = defaultLanguage;
            }
        }

        public static EventHandler<LocalizationEventArgs> LanguageLoaded;

        private const string MissingTranslationText = "[Missing Translation]";

        /// <summary>
        /// Dictionary with all registered localizations.
        /// </summary>
        public static Dictionary<string, string> entries = new Dictionary<string, string>();

        static Localization()
        {
            LanguageLoaded += ArrClass.LanguageLoaded;
        }

        /// <summary>
        /// Loads the given config into the localization database, will overwrite everything.
        /// </summary>
        /// <param name="config">Localization config which should be used.</param>
        /// <param name="language">Language which should be loaded.</param>
        /// <param name="course">Name of the course which is ran.</param>
        public static void LoadLocalization(LocalizationConfig config, string language, string course)
        {
            entries = LocalizationReader.Load(config, language, course);
            EmitLanguageLoaded(language, config.FallbackLanguage);
        }

        /// <summary>
        /// Loads the given config into the localization, will overwrite in there.
        /// </summary>
        /// <param name="config">Localization config which should be used.</param>
        /// <param name="parameters">Additional parameters used for resolving the path.</param>
        public static void LoadLocalization(LocalizationConfig config, Dictionary<string, string> parameters)
        {
            entries = LocalizationReader.Load(config, parameters);

            string language = "Unknown";
            if (parameters.ContainsKey(LocalizationReader.KeyLanguage))
            {
                language = parameters[LocalizationReader.KeyLanguage];
            }

            EmitLanguageLoaded(language, config.FallbackLanguage);
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
                EmitLanguageLoaded("Unknown", "");
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

        private static void EmitLanguageLoaded(string language, string fallbackLanguage)
        {
            LanguageLoaded?.Invoke(entries, new LocalizationEventArgs(entries, language, fallbackLanguage));
        }

        private static class ArrClass
        {
            private static readonly string[] multiUseTokenList = new[]
            {
                "{0}, Ar!",
                "{0}, argh!",
                "Aye Mate! {0}",
                "Listen cabin boy! {0}",
                "Yo! . . {0}",
                "Hey Lad . . {0}"
            };

            private static readonly List<string> singleUseTokenList = new List<string>()
            {
                "Did I ever tell you the story of . . . oh wait . . mate . . . . . . . . . {0}",
                "Sixteen men on a dead man's chest yo-ho-ho and a bottle of rum. And now {0}",
                "Sorry Matey, i forgot what you have to do, too much rum.",
                "What should we do with the drunken sailor? {0}",
                "Not all treasure is silver and gold, mate. But now {0}",
                "Before we continue, I have a very important question: Why is the rum gone? {0}"
            };

            private static readonly Dictionary<string, string> replacementWords = new Dictionary<string, string>()
            {
                {"hello", "Ahoy! lets trouble the water!"},
                {" hi ", "Ahoy, Matey"},
                {"good bye", "Feed the Fish"},
                {" bye ", "Feed the Fish"},
                {" my ", "Me"},
                {" you ", "Ye"},
                {"friend", "mate"},
                {"helmet", "hat"},
                {"tool", "saber"},
                {"point", "shoot"},
                {"grab", "loot"},
                {"floor", "deck"},
                {"bed", "hammock"},
                {"water", "rum"},
                {"screw", "nail thing"},
                {"control", "take the helm"},
                {"machine", "construct"},
                {"yes", "aye"},
                {"reward", "bounty"}
            };

            public static void LanguageLoaded(object sender, LocalizationEventArgs args)
            {
                if (args.DefaultLanguage.ToLower() == "ar" && args.Language.ToLower() == "en")
                {
                    AddSentences(args.Entries);
                    ReplaceWords(args.Entries);
                }
            }

            private static void AddSentences(Dictionary<string, string> entries)
            {
                List<string> singleUseTokens = new List<string>(singleUseTokenList);

                string[] keys = entries.Keys.ToArray();
                foreach (string key in keys)
                {
                    if (key.Contains(".") == false && UnityEngine.Random.value < 0.70f)
                    {
                        string text;
                        if (UnityEngine.Random.value > 0.5 && singleUseTokens.Count > 0)
                        {
                            int position = UnityEngine.Random.Range(0, singleUseTokens.Count - 1);
                            text = singleUseTokens[position];
                            singleUseTokens.RemoveAt(position);
                        }
                        else
                        {
                            text = multiUseTokenList[UnityEngine.Random.Range(0, multiUseTokenList.Length - 1)];
                        }
                        entries[key] = string.Format(text, entries[key]);
                    }
                }
            }

            private static void ReplaceWords(Dictionary<string, string> entries)
            {
                string[] keys = entries.Keys.ToArray();
                foreach (string key in keys)
                {
                    if (key.Contains(".") == false)
                    {
                        string value = entries[key].ToLower();
                        foreach (string targetWord in replacementWords.Keys)
                        {
                            value = value.Replace(targetWord, replacementWords[targetWord]);
                        }

                        entries[key] = value;
                    }
                }
            }
        }
    }
}
