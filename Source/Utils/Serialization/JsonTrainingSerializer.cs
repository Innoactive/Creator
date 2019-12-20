using System;
using System.Collections.Generic;
using Common.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LogManager = Innoactive.Hub.Logging.LogManager;

namespace Innoactive.Hub.Training.Utils.Serialization
{
    public static class JsonTrainingSerializer
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(JsonTrainingSerializer));

        public const int Version = 1;

        /// <summary>
        /// Returns the json serializer settings used by the training deserialization.
        /// </summary>
        public static JsonSerializerSettings SerializerSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    Converters = GetJsonConverter(),
                    PreserveReferencesHandling = PreserveReferencesHandling.All,
                    Formatting = Formatting.Indented,
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                    TypeNameHandling = TypeNameHandling.All
                };
            }
        }

        private static List<JsonConverter> GetJsonConverter()
        {
            List<JsonConverter> converters = new List<JsonConverter>();

            IEnumerable<Type> result = ReflectionUtils.GetConcreteImplementationsOf<JsonConverter>()
                .WhichHaveAttribute<TrainingConverterAttribute>();

            foreach (Type type in result)
            {
                converters.Add(ReflectionUtils.CreateInstanceOfType(type) as JsonConverter);
            }

            return converters;
        }

        public static string Serialize(ICourse deserialized)
        {
            JObject jObject = JObject.FromObject(deserialized, JsonSerializer.Create(SerializerSettings));
            jObject.Add("$serializerVersion", Version);
            return jObject.ToString();
        }

        private static int RetrieveSerializerVersion(string serialized)
        {
            return (int)JObject.Parse(serialized)["$serializerVersion"].ToObject(typeof(int));
        }

        public static ICourse Deserialize(string serialized)
        {
            return (ICourse)JsonConvert.DeserializeObject(serialized, SerializerSettings);
        }
    }
}
