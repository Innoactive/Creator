﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using Innoactive.Creator.Internationalization;

namespace Innoactive.Hub.Training.Utils
{
    /// <summary>
    /// Collection of localization utilities.
    /// </summary>
    public static class LocalizationUtils
    {
        private const string defaultIsoCode = "en";

        /// <summary>
        /// Returns new instance of LocalizedString with the same values.
        /// </summary>
        public static LocalizedString Clone(this LocalizedString original)
        {
            return original == null ? null : new LocalizedString(original.Key, original.DefaultText, original.FormatParams?.ToArray());
        }

        /// <summary>
        /// Convert natural language name to two-letters ISO code.
        /// </summary>
        /// <param name="language">
        /// String with natural language name or two-letters ISO code.
        /// </param>
        /// <param name="result">
        /// If <paramref name="language"/> is already in two-letters ISO code, simply returns it.
        /// If <paramref name="language"/> is a natural language name, returns two-symbol code.
        /// Otherwise, returns null.
        /// </param>
        /// <returns>
        /// Was operation successful or not.
        /// </returns>
        public static bool TryConvertToTwoLetterIsoCode(this string language, out string result)
        {
            if (IsTwoLettersIsoCode(language))
            {
                result = language.ToLower();
                return true;
            }

            try
            {
                result = ConvertNaturalLanguageNameToTwoLetterIsoCode(language);
                return true;
            }
            catch (ArgumentException)
            {
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Helps to convert Unity's Application.systemLanguage to a two-letter ISO language code.
        /// </summary>
        /// <returns>
        /// The two-letter ISO code from system language.
        /// If system language can't be converted, returns "en" instead.
        /// </returns>
        public static string GetSystemLanguageAsTwoLetterIsoCode()
        {
            try
            {
                return Application.systemLanguage.ConvertToTwoLetterIsoCode();
            }
            catch (ArgumentException)
            {
                return defaultIsoCode;
            }
        }

        /// <summary>
        /// Find localization file for language based on the filename.
        /// </summary>
        /// <returns>
        /// The first file which filename can be converted to the same two-letter ISO code as <paramref name="language"/>.
        /// </returns>
        public static bool TryFindLocalizationForLanguage(string language, IEnumerable<TextAsset> textAssetsToSearch, out string result)
        {
            foreach (TextAsset file in textAssetsToSearch)
            {
                string fileIso;
                string languageIso;

                if (file.name.TryConvertToTwoLetterIsoCode(out fileIso) == false)
                {
                    continue;
                }

                if (language.TryConvertToTwoLetterIsoCode(out languageIso) == false)
                {
                    continue;
                }

                if (fileIso != languageIso)
                {
                    continue;
                }

                result = file.text;
                return true;
            }

            result = null;
            return false;
        }

        /// <summary>
        /// Convert SystemLanguage to two-line ISO code.
        /// </summary>
        private static string ConvertToTwoLetterIsoCode(this SystemLanguage language)
        {
            return language.ToString().ConvertNaturalLanguageNameToTwoLetterIsoCode();
        }

        /// <summary>
        /// Helps to convert strings with full language names like "English" to a two-letter ISO language code.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="languageName"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="languageName"/> is not natural language name.</exception>
        /// <returns>The two-letter ISO code from the given language name. If it can not parse the string, it returns null.</returns>
        private static string ConvertNaturalLanguageNameToTwoLetterIsoCode(this string languageName)
        {
            if (languageName == null)
            {
                throw new ArgumentNullException("languageName", "languageName is null");
            }

            IEnumerable<CultureInfo> allCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

            CultureInfo languageCulture = allCultures.FirstOrDefault(culture =>
            {
                string preparedCultureName = culture.EnglishName.RemoveSymbols('(', ')', ' ');
                return string.Compare(preparedCultureName, languageName, StringComparison.OrdinalIgnoreCase) == 0;
            });

            if (languageCulture != null)
            {
                return languageCulture.TwoLetterISOLanguageName;
            }

            throw new ArgumentException("languageName is not a supported language name", "languageName");
        }

        /// <summary>
        /// Check if <paramref name="language"/> is two-letter ISO code.
        /// </summary>
        private static bool IsTwoLettersIsoCode(string language)
        {
            if (language == null)
            {
                return false;
            }

            // Some two-letter ISO codes are three letters long.
            if (language.Length < 2 || language.Length > 3)
            {
                return false;
            }

            try
            {
                // If CultureInfo constructor is able to parse the string, it's two-letter ISO code.
                // ReSharper disable once ObjectCreationAsStatement
                new CultureInfo(language);
                return true;
            }
            catch (ArgumentException)
            {
                // Otherwise, it isn't.
                return false;
            }
        }

        /// <summary>
        /// Remove <paramref name="symbolsToRemove"/> from <paramref name="input"/> string.
        /// </summary>
        private static string RemoveSymbols(this string input, params char[] symbolsToRemove)
        {
            string result = input;
            foreach (char symbol in symbolsToRemove)
            {
                result = result.Replace(symbol.ToString(), "");
            }

            return result;
        }
    }
}
