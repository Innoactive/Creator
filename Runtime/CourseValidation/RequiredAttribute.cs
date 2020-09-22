using System;
using System.Linq;
using Innoactive.Creator.Core.Runtime.Properties;

namespace Innoactive.Creator.Core.Validation
{
    /// <summary>
    /// Checks if the variable this attribute is attached to, is not empty.
    /// </summary>
    public class RequiredAttribute : Attribute, IAttributeValidator
    {
        /// <inheritdoc/>
        public ValidationErrorLevel ErrorLevel { get; }

        public RequiredAttribute(ValidationErrorLevel errorLevel = ValidationErrorLevel.ERROR)
        {
            ErrorLevel = errorLevel;
        }

        /// <inheritdoc/>
        public bool Validate(object value, out string message)
        {
            message = "";

            if (value == null)
            {
                message = "This variable is required";
                return true;
            }

            if (value is Boolean)
            {
                return false;
            }

            if (value is string && string.IsNullOrEmpty((string) value))
            {
                message = "This variable is required";
                return true;
            }

            if (value.Equals(GetDefault(value.GetType())) && IsNumeric(value.GetType()))
            {
                message = "This number should not be zero.";
                return true;
            }

            if (value.GetType().GetInterfaces().Contains(typeof(ICanBeEmpty)) && ((ICanBeEmpty)value).IsEmpty())
            {
                message = "This variable is required";
                return true;
            }

            return false;
        }

        private object GetDefault(Type type)
        {
            if(type.IsValueType)
            {
                object instance = Activator.CreateInstance(type);
                return instance;
            }
            return null;
        }

        private bool IsNumeric(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = type.GetGenericArguments()[0];
            }

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
    }
}
