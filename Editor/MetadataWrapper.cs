using System;
using System.Collections.Generic;

namespace Innoactive.CreatorEditor
{
    public class MetadataWrapper
    {
        public Dictionary<string, object> Metadata { get; set; }
        public Type ValueDeclaredType { get; set; }
        public object Value { get; set; }
    }
}
