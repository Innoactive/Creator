using System;
using System.Collections.Generic;

namespace Innoactive.Hub.Training.Editors
{
    public class MetadataWrapper
    {
        public Dictionary<string, object> Metadata { get; set; }
        public Type ValueDeclaredType { get; set; }
        public object Value { get; set; }
    }
}
