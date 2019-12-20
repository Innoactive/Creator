using System.Collections.Generic;
using Innoactive.Hub.Training.Editors.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Innoactive.Hub.Training.Editors.Utils.Serialization
{
    public static class JsonEditorConfigurationSerializer
    {
        private const int version = 0;
        /// <summary>
        /// Returns the json serializer settings used by the training editor configuration deserialization.
        /// </summary>
        public static JsonSerializerSettings SerializerSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter>(),
                    PreserveReferencesHandling = PreserveReferencesHandling.All,
                    Formatting = Formatting.Indented,
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                    TypeNameHandling = TypeNameHandling.All
                };
            }
        }

        public static string Serialize(AllowedMenuItemsSettings deserialized)
        {
            JObject jObject = JObject.FromObject(deserialized, JsonSerializer.Create(SerializerSettings));
            jObject.Add("$serializerVersion", version);
            return jObject.ToString();
        }

        private static int RetrieveSerializerVersion(string serialized)
        {
            return (int)JObject.Parse(serialized)["$serializerVersion"].ToObject(typeof(int));
        }

        public static AllowedMenuItemsSettings Deserialize(string serialized)
        {
            return (AllowedMenuItemsSettings)JsonConvert.DeserializeObject(serialized, SerializerSettings);
        }
    }
}
