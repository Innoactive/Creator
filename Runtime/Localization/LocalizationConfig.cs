using System;
using UnityEngine;

namespace Innoactive.Creator.Core.Internationalization
{
    /// <summary>
    /// Localization configuration data structure. Will load all sources in an ordered way overwriting all
    /// keys already existing.
    /// </summary>
    [CreateAssetMenu(fileName = "LocalizationConfig", menuName = "Innoactive/Localization Config", order = 0)]
    public class LocalizationConfig : ScriptableObject
    {
        /// <summary>
        /// Default configuration path for courses running on a computer.
        /// </summary>
        public const string DefaultLocalizationConfig = "Localization/DefaultLocalization";

        /// <summary>
        /// Default configuration path for courses running on standalone device.
        /// </summary>
        public const string StandaloneDefaultLocalizationConfig = "Localization/StandaloneLocalization";

        [SerializeField]
        [Tooltip("Will be replacing the {fallback_language} token in paths.")]
        public string FallbackLanguage = "en";

        [SerializeField]
        public LocalizationSource[] Sources;

        [Serializable]
        public struct LocalizationSource
        {
            [SerializeField]
            [Tooltip("Path to the localization file, out of the box the following tokens are supported: \n{language} = active language\n{fallback_language} = fall back language used\n{course} = course name, only works in course scope")]
            public string Path;

            [SerializeField]
            [Tooltip("Decides how the resource file is loaded.")]
            public ResourceType ResourceType;

            [SerializeField]
            [Tooltip("If set true, this source will be used determine which languages are supported (selectable) right now.")]
            public bool DefinesAvailableLanguages;
        }

        [Serializable]
        public enum ResourceType
        {
            StreamingAssets,
            Resources,
        }
    }
}
