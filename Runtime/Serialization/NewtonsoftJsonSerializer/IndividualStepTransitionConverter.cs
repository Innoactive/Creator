using System;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace VPG.Creator.Core.Serialization
{
    /// <summary>
    /// A `JsonConverter` for transitions which serializes the transition's Target as null. It is used to serialize individual steps (for example, for copy/paste feature).
    /// </summary>
    internal class IndividualStepTransitionConverter : JsonConverter
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            ITransitionData currentValue = (ITransitionData)value;

            if (currentValue == null)
            {
                return;
            }

            writer.WriteStartObject();
            {
                writer.WritePropertyName("Target");
                writer.WriteValue((IStep)null);

                writer.WritePropertyName("Conditions");
                serializer.Serialize(writer, currentValue.Conditions);
            }
            writer.WriteEndObject();
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new InvalidProgramException("It should never be called as the `CanRead` property is set to false.");
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return typeof(ITransitionData).IsAssignableFrom(objectType);
        }

        /// <inheritdoc />
        public override bool CanRead { get { return false; } }

        /// <inheritdoc />
        public override bool CanWrite { get { return true; } }
    }
}
