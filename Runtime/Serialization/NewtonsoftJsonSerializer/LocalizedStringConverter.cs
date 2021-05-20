using System;
using System.Linq;
using VPG.Core.Internationalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VPG.Core.Serialization
{
    /// <summary>
    /// Internal class, dont use this.
    ///
    /// Handles the proper conversion to and from LocalizedString.
    /// </summary>
    [NewtonsoftConverter]
    internal class LocalizedStringConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            LocalizedString currentValue = (LocalizedString)value;

            if (currentValue == null)
            {
                return;
            }

            JObject jObject = new JObject
            {
                { "key", currentValue.Key },
                { "defaultText", currentValue.Value },
                { "formatParams", new JArray(currentValue.FormatParams) }
            };
            jObject.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            JObject jObject = JObject.Load(reader);
            string key = jObject.Property("key").Value.ToString();
            string defaultText = jObject.Property("defaultText").Value.ToString();
            string[] formatParams = ((JArray)jObject["formatParams"]).Values<string>().ToArray();

            return new LocalizedString(key, defaultText, formatParams);
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(LocalizedString) == objectType;
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override bool CanRead
        {
            get { return true; }
        }
    }
}
