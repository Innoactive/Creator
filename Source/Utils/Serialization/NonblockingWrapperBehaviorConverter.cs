using System;
using Innoactive.Hub.Training.Behaviors;
using Newtonsoft.Json;

namespace Innoactive.Hub.Training.Utils.Serialization
{
    /// <summary>
    /// Internal class, dont use this.
    ///
    /// Removes obsolete NonblockingWrapperBehavior.
    /// </summary>
    [NewtonsoftTrainingConverter]
    internal class NonblockingWrapperBehaviorConverter : JsonConverter
    {
#pragma warning disable 618
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            object deserialized = serializer.Deserialize(reader);

            NonblockingWrapperBehavior obsoleteBehavior = deserialized as NonblockingWrapperBehavior;

            if (obsoleteBehavior == null)
            {
                return deserialized;
            }
            else
            {
                IBehavior properBehavior = obsoleteBehavior.Data.Behavior;
                IBackgroundBehaviorData blockingData = properBehavior.Data as IBackgroundBehaviorData;
                if (blockingData != null)
                {
                    blockingData.IsBlocking = obsoleteBehavior.Data.IsBlocking;
                }

                return properBehavior;
            }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(IBehavior).IsAssignableFrom(objectType);
        }
#pragma warning restore 618
    }
}
