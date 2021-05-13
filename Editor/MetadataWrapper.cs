using System;
using System.Collections.Generic;

namespace VPG.CreatorEditor
{
    /// <summary>
    /// Data structure used to draw properties in the 'Step Inspector'.
    /// </summary>
    public class MetadataWrapper
    {
        /// <summary>
        /// Collection of data from a 'System.Reflection.MemberInfo'.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; }

        /// <summary>
        /// Type of an object reference.
        /// </summary>
        public Type ValueDeclaredType { get; set; }

        /// <summary>
        /// Value of an object reference.
        /// </summary>
        public object Value { get; set; }
    }
}
