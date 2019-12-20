using System;
using Common.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Innoactive.Hub.Training.Utils.Serialization
{
    /// <summary>
    /// Converts Unity color into json and back.
    /// </summary>
    [TrainingConverter]
    public class UnityColorConverter : JsonConverter
    {
        private static readonly ILog logger = LogManager.GetLogger<UnityColorConverter>();

        /// <inheritDoc/>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Color color = (Color) value;
            JObject data = new JObject();

            data.Add("r",color.r);
            data.Add("g",color.g);
            data.Add("b",color.b);
            data.Add("a",color.a);

            data.WriteTo(writer);
        }

        /// <inheritDoc/>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                try
                {
                    JObject data = (JObject) JToken.ReadFrom(reader);

                    float r = data["r"].Value<float>();
                    float g = data["g"].Value<float>();
                    float b = data["b"].Value<float>();
                    float a = 1.0f;
                    if (data.Count == 4)
                    {
                        a = data["a"].Value<float>();
                    }

                    return new Color(r, g, b, a);
                }
                catch (Exception ex)
                {
                    logger.Error("Exception occured while trying to parse a color.", ex);
                    return Color.magenta;
                }
            }
            logger.Warn("Can't read/parse color from JSON.");
            return Color.magenta;
        }


        /// <inheritDoc/>
        public override bool CanConvert(Type objectType)
        {
            return typeof(Color) == objectType;
        }
    }
}
