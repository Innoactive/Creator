using System;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Innoactive.Hub
{
    [Serializable, DataContract]
    public class LocalizedString
    {
        [SerializeField]
        private string key;
        [SerializeField]
        private string defaultText;
        [SerializeField]
        private string[] formatParams;

        public string Key
        {
            get
            {
                return key;
            }
        }

        public string DefaultText
        {
            get
            {
                return defaultText;
            }
        }

        public string[] FormatParams
        {
            get
            {
                return formatParams;
            }
        }

        public string Value
        {
            get
            {
                if (!String.IsNullOrEmpty(key))
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
        /// Tries to parse the input string. If parsing succeeded, the result can be found in the output parameter. 
        /// 
        /// Expected syntax for input string is "%1|%2" where %1 can be a i18-key and %2 a arbitrary fallback text, both separated by a pipe char '|'.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="output"></param>
        /// <returns>true, if parsing succeeded, false otherwise.</returns>
        public static bool TryParse(string str, out LocalizedString output)
        {
            Regex parseRegex = new Regex("^([a-zA-Z0-9.]+)\\|(.+)$");
            Match m = parseRegex.Match(str);
            if (!m.Success)
            {
                output = new LocalizedString("Hub.ParseError", "Unable to parse localized string: '" + str + "'");
                return false;
            }
            output = new LocalizedString(m.Groups[1].Value, m.Groups[2].Value);
            return true;
        }

        public static LocalizedString Parse(string str, bool allowNull = false)
        {
            LocalizedString output = null;
            if (TryParse(str, out output))
            {
                return output;
            }
            if (allowNull)
            {
                return null;
            }
            return output;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
