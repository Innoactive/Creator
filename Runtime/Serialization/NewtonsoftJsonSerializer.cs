using System;
using System.Collections.Generic;
using System.Text;
using Innoactive.Creator.Core.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Innoactive.Creator.Core.Serialization
{
    /// <summary>
    /// This serializer uses NewtonsoftJson to serialize data, the outcome is json.
    /// </summary>
    public class NewtonsoftJsonSerializer : ITrainingSerializer
    {
        public virtual int Version { get; } = 1;

        /// <summary>
        /// Returns the json serializer settings used by the training deserialization.
        /// </summary>
        public virtual JsonSerializerSettings SerializerSettings =>
            new JsonSerializerSettings
            {
                Converters = GetJsonConverter(),
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                Formatting = Formatting.Indented,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                TypeNameHandling = TypeNameHandling.All
            };

        /// <inheritdoc/>
        public string Name { get; } = "Newtonsoft Json Importer";

        /// <inheritdoc/>
        public bool CanDeserialize { get; } = true;

        /// <inheritdoc/>
        public bool CanSerialize { get; } = true;

        /// <inheritdoc/>
        public string FileFormat { get; } = "json";

        /// <inheritdoc/>
        public virtual byte[] ToByte(ICourse course)
        {
            JObject jObject = JObject.FromObject(course, JsonSerializer.Create(SerializerSettings));
            jObject.Add("$serializerVersion", Version);
            return new UTF8Encoding().GetBytes( jObject.ToString());
        }

        /// <inheritdoc/>
        public virtual ICourse ToCourse(byte[] data)
        {
            string stringData = new UTF8Encoding().GetString(data);
            return (ICourse)JsonConvert.DeserializeObject(stringData, SerializerSettings);
        }

        /// <summary>
        /// Creates a list of JsonConvert by reflection, all converter with the attribute NewtonsoftTrainingConverter
        /// will be added by default.
        /// </summary>
        /// <returns>A list of all found JsonConverter.</returns>
        protected virtual List<JsonConverter> GetJsonConverter()
        {
            List<JsonConverter> converters = new List<JsonConverter>();

            IEnumerable<Type> result = ReflectionUtils.GetConcreteImplementationsOf<JsonConverter>()
                .WhichHaveAttribute<NewtonsoftTrainingConverterAttribute>();

            foreach (Type type in result)
            {
                converters.Add(ReflectionUtils.CreateInstanceOfType(type) as JsonConverter);
            }

            return converters;
        }
    }
}
