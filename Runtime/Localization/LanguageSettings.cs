using Innoactive.Creator.Core.Runtime.Utils;

namespace Innoactive.Creator.Core.Internationalization
{
    public class LanguageSettings : SettingsObject<LanguageSettings>
    {
        /// <summary>
        /// Language which should be used.
        /// </summary>
        public string DefaultLanguage = "En";

        /// <summary>
        /// Returns the active, will be stored for one session.
        /// </summary>
        public string ActiveLanguage { get; set; }
    }
}
