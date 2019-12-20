using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Common.Logging;

namespace Innoactive.Hub.Training.Attributes
{
    /// <summary>
    /// Declares that children of this list have metadata attributes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ListOfAttribute : MetadataAttribute
    {
        [DataContract(IsReference = true)]
        public class Metadata
        {
            [DataMember]
            public List<MetadataAttribute> ChildAttributes { get; set; }

            [DataMember]
            public List<Dictionary<string, object>> ChildMetadata { get; set; }
        }

        private static readonly ILog logger = LogManager.GetLogger<ListOfAttribute>();

        private readonly List<MetadataAttribute> childAttributes;

        /// <inheritdoc />
        public ListOfAttribute(params Type[] childAttributes)
        {
            Type[] uniqueTypes = childAttributes.Distinct().ToArray();

            if (uniqueTypes.Length != childAttributes.Length)
            {
                logger.Error("Child attributes of ListOf attribute have to be unique. Duplicates are omitted.");
            }

            this.childAttributes = new List<MetadataAttribute>(uniqueTypes.Where(attribute => (typeof(MetadataAttribute).IsAssignableFrom(attribute)))
                .Where(attribute => (typeof(ListOfAttribute).IsAssignableFrom(attribute) == false))
                .Where(attribute => attribute.GetConstructor(new Type[0]) != null)
                .Select(Activator.CreateInstance)
                .Cast<MetadataAttribute>());
        }

        /// <inheritdoc />
        public override object GetDefaultMetadata(MemberInfo owner)
        {
            return new Metadata
            {
                ChildAttributes = new List<MetadataAttribute>(childAttributes),
                ChildMetadata = new List<Dictionary<string, object>>(),
            };
        }

        /// <summary>
        /// <inheritdoc />
        /// ListOf attribute checks that metadata of all children is valid, too.
        /// </summary>
        public override bool IsMetadataValid(object metadata)
        {
            Metadata listOfMetadata = (Metadata)metadata;

            if (AreSetsTheSame(childAttributes, listOfMetadata.ChildAttributes, attribute => attribute.Name) == false)
            {
                return false;
            }

            return listOfMetadata.ChildMetadata.All(entryMetadata => listOfMetadata.ChildAttributes.All(childAttribute => childAttributes.Any(attribute => attribute.Name == childAttribute.Name) && entryMetadata.ContainsKey(childAttribute.Name) && childAttribute.IsMetadataValid(entryMetadata[childAttribute.Name])));
        }

        private static bool AreSetsTheSame<T>(IEnumerable<T> first, IEnumerable<T> second, Func<T, IComparable> toComparable)
        {
            if (first == null)
            {
                return second == null;
            }

            if (second == null)
            {
                return false;
            }

            return first.OrderBy(toComparable).SequenceEqual(second.OrderBy(toComparable));
        }
    }
}
