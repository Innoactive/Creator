using System;
using Innoactive.Creator.Core.Runtime.Utils;

namespace Innoactive.Creator.Core.Internationalization
{
    public class LanguageSettings : SettingsObject<LanguageSettings>
    {
        /// <summary>
        /// Language which should be used.
        /// </summary>
        public string DefaultLanguage = "En";

        [NonSerialized]
        private string activeLanguage = null;

        public string ActiveLanguage
        {
            get
            {
                if (string.IsNullOrEmpty(activeLanguage))
                {
                    return DefaultLanguage;
                }

                return activeLanguage;
            }

            set
            {
                activeLanguage = value;
            }
        }
    }
}
