using System;

namespace Innoactive.Creator.Core.Serialization
{
    /// <summary>
    /// Every class with this attribute which also extends JsonConverter will be added as converter to the <see cref="NewtonsoftJsonSerializer"/>
    /// </summary>
    public class NewtonsoftTrainingConverterAttribute : Attribute
    {

    }
}
