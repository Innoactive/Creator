using VPG.Core.Runtime.Utils;

namespace VPG.Core.Internationalization
{
    /// <summary>
    /// Language settings for the VR Process Gizmo.
    /// </summary>
    public class LanguageSettings : SettingsObject<LanguageSettings>
    {
        /// <summary>
        /// Language which should be used.
        /// </summary>
        public string DefaultLanguage = "En";

        /// <summary>
        /// Returns the currently active language, will be stored for one session.
        /// </summary>
        public string ActiveLanguage { get; set; }

        /// <summary>
        /// Returns the active or default language.
        /// </summary>
        public string ActiveOrDefaultLanguage
        {
            get
            {
                if (string.IsNullOrEmpty(ActiveLanguage))
                {
                    return DefaultLanguage;
                }

                return ActiveLanguage;
            }
        }
    }
}
