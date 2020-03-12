using System;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Innoactive.Creator.Internationalization
{
    /// <summary>
    /// References a localized string.
    /// </summary>
    [Serializable, DataContract]
    public class LocalizedString
    {
        [SerializeField]
        private string key;
        [SerializeField]
        private string defaultText;
        [SerializeField]
        private string[] formatParams;

        /// <summary>
        /// Localization's identifier.
        /// </summary>
        public string Key
        {
            get
            {
                return key;
            }
        }

        /// <summary>
        /// Text used in case <see cref="Key"/> does not have a valid <see cref="Value"/>
        /// </summary>
        public string DefaultText
        {
            get
            {
                return defaultText;
            }
        }

        /// <summary>
        /// Format arguments.
        /// </summary>
        public string[] FormatParams
        {
            get
            {
                return formatParams;
            }
        }

        /// <summary>
        /// Current localized text.
        /// </summary>
        public string Value
        {
            get
            {
                if (!string.IsNullOrEmpty(key))
                {
                    if (formatParams == null || formatParams.Length == 0)
                    {
                        return Localization.Get(key, defaultText);
                    }
                    else
                    {
                        return Localization.GetFormat(key, defaultText, formatParams);
                    }
                }
                else
                {
                    if (formatParams == null || formatParams.Length == 0)
                    {
                        return defaultText;
                    }
                    else
                    {
                        return string.Format(defaultText, formatParams);
                    }
                }
            }
        }

        public LocalizedString(string key = "", string defaultText = "") : this(key, defaultText, new string[] { }) { }

        public LocalizedString(string key = "", string defaultText = "", params string[] formatParams)
        {
            this.key = key;
            this.defaultText = defaultText;
            this.formatParams = formatParams;
        }

        /// <summary>
        /// Current localized text.
        /// </summary>
        public override string ToString()
        {
            return Value;
        }

        /// <summary>
        /// Tries to parse the input string. If parsing succeeded, the result can be found in the output parameter.
        /// </summary>
        /// <remarks>Expected syntax for input string is "%1|%2" where %1 can be a i18-key and %2 a arbitrary fallback text, both separated by a pipe char '|'.</remarks>
        /// <returns>True, if parsing succeeded, false otherwise.</returns>
        public static bool TryParse(string str, out LocalizedString output)
        {
            Regex parseRegex = new Regex("^([a-zA-Z0-9.]+)\\|(.+)$");
            Match match = parseRegex.Match(str);

            if (match.Success == false)
            {
                output = new LocalizedString("Localization.ParseError", "Unable to parse localized string: '" + str + "'");
                return false;
            }

            output = new LocalizedString(match.Groups[1].Value, match.Groups[2].Value);
            return true;
        }

        /// <summary>
        /// Parses the input string.
        /// </summary>
        public static LocalizedString Parse(string str, bool allowNull = false)
        {
            if (TryParse(str, out LocalizedString output))
            {
                return output;
            }

            return allowNull ? null : output;
        }
    }
}
