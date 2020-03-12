using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Innoactive.Hub.Training.Attributes;

namespace Innoactive.Hub.Training
{
    [DataContract(IsReference = true)]
    public class Metadata : IMetadata
    {
        [DataMember]
        private Dictionary<string, Dictionary<string, object>> values = new Dictionary<string, Dictionary<string, object>>();

        public void SetMetadata(MemberInfo member, string attributeName, object data)
        {
            if (values.ContainsKey(member.Name) == false)
            {
                values[member.Name] = new Dictionary<string, object>();
            }

            values[member.Name][attributeName] = data;
        }

        public object GetMetadata(MemberInfo member, MetadataAttribute attribute)
        {
            if (values.ContainsKey(member.Name) && values[member.Name].ContainsKey(attribute.Name))
            {
                return values[member.Name][attribute.Name];
            }

            return null;
        }

        public Dictionary<string, object> GetMetadata(MemberInfo member)
        {
            if (values.ContainsKey(member.Name))
            {
                return values[member.Name].ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            }

            return new Dictionary<string, object>();
        }
    }
}
