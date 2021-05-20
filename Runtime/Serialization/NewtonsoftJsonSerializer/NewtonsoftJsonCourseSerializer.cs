using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VPG.Core.UI.Drawers.Metadata;
using VPG.Core.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace VPG.Core.Serialization.NewtonsoftJson
{
    /// <summary>
    /// This serializer uses NewtonsoftJson to serialize data, the outcome is a json file in the UTF-8 encoding.
    /// </summary>
    public class NewtonsoftJsonCourseSerializer : ICourseSerializer
    {
        protected virtual int Version { get; } = 1;

        private static JsonSerializerSettings CreateSettings(IList<JsonConverter> converters)
        {
            return new JsonSerializerSettings
            {
                Converters = converters,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                Formatting = Formatting.Indented,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                SerializationBinder = new CourseSerializationBinder(),
                TypeNameHandling = TypeNameHandling.All
            };
        }

        /// <summary>
        /// Returns the json serializer settings used by the training deserialization.
        /// </summary>
        public static JsonSerializerSettings CourseSerializerSettings
        {
            get { return CreateSettings(GetJsonConverters()); }
        }

        private static JsonSerializerSettings StepSerializerSettings
        {
            get
            {
                List<JsonConverter> converters = new List<JsonConverter> { new IndividualStepTransitionConverter() };

                converters.AddRange(GetJsonConverters());

                return CreateSettings(converters);
            }
        }

        /// <summary>
        /// Creates a list of JsonConverters via reflection. It adds all JsonConverters with the <seealso cref="NewtonsoftConverterAttribute"/>
        /// will be added by default.
        /// </summary>
        /// <returns>A list of all found JsonConverters.</returns>
        private static List<JsonConverter> GetJsonConverters()
        {
            return ReflectionUtils.GetConcreteImplementationsOf<JsonConverter>()
                .WhichHaveAttribute<NewtonsoftConverterAttribute>()
                .OrderBy(type => type.GetAttribute<NewtonsoftConverterAttribute>().Priority)
                .Select(type => ReflectionUtils.CreateInstanceOfType(type) as JsonConverter)
                .ToList();
        }

        /// <inheritdoc/>
        public virtual string Name { get; } = "Newtonsoft Json Importer";

        /// <inheritdoc/>
        public virtual string FileFormat { get; } = "json";

        protected  byte[] Serialize(IEntity entity, JsonSerializerSettings settings)
        {
            JObject jObject = JObject.FromObject(entity, JsonSerializer.Create(settings));
            jObject.Add("$serializerVersion", Version);
            return new UTF8Encoding().GetBytes(jObject.ToString());
        }

        protected T Deserialize<T>(byte[] data, JsonSerializerSettings settings)
        {
            string stringData = new UTF8Encoding().GetString(data);
            return (T)JsonConvert.DeserializeObject(stringData, settings);
        }

        /// <inheritdoc/>
        public virtual byte[] CourseToByteArray(ICourse course)
        {
            return Serialize(course, CourseSerializerSettings);
        }

        /// <inheritdoc/>
        public virtual ICourse CourseFromByteArray(byte[] data)
        {
            JObject dataObject = JsonConvert.DeserializeObject<JObject>(new UTF8Encoding().GetString(data), CourseSerializerSettings);

            // Check if course was serialized with version 1
            int version = dataObject.GetValue("$serializerVersion").ToObject<int>();
            if (version != 1)
            {
                throw new Exception($"The loaded course is serialized with a serializer version {version}, which in compatible with this serializer.");
            }

            return Deserialize<ICourse>(data, CourseSerializerSettings);
        }

        /// <inheritdoc/>
        public virtual byte[] StepToByteArray(IStep step)
        {
            return Serialize(step, StepSerializerSettings);
        }

        /// <inheritdoc/>
        public virtual IStep StepFromByteArray(byte[] data)
        {
            return Deserialize<IStep>(data, StepSerializerSettings);
        }

        internal class CourseSerializationBinder : DefaultSerializationBinder
        {
            public override Type BindToType(string assemblyName, string typeName)
            {
                if (typeName == "VPG.CreatorEditor.UI.Drawers.Metadata.ReorderableElementMetadata")
                {
                    return typeof(ReorderableElementMetadata);
                }

                return base.BindToType(assemblyName, typeName);
            }
        }
    }
}
