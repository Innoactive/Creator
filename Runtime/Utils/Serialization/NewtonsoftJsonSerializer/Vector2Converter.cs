using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Innoactive.Hub.Training.Utils.Serialization
{
    /// <summary>
    /// Converts Vector2 into json and back.
    /// </summary>
    [NewtonsoftConverter]
    public class Vector2Converter : JsonConverter
    {
        /// <inheritDoc/>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Vector2 vec = (Vector2) value;
            JObject data = new JObject();

            data.Add("x", vec.x);
            data.Add("y", vec.y);

            data.WriteTo(writer);
        }

        /// <inheritDoc/>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                JObject data = (JObject)JToken.ReadFrom(reader);
                return new Vector2(data["x"].Value<float>(), data["y"].Value<float>());
            }

            return Vector2.zero;
        }

        /// <inheritDoc/>
        public override bool CanConvert(Type objectType)
        {
            return typeof(Vector2) == objectType;
        }
    }
}
