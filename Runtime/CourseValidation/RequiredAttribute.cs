using System;
using System.Collections.Generic;
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

        public List<ReportEntry> Validate(object value)
        {
            if (IsEmpty(value))
            {
                return new List<ReportEntry>() { new ReportEntry()
                {
                    Code = 3003,
                    Message = "This variable is required",
                    ErrorLevel = ErrorLevel,
                }};
            }

            if (IsNumeric(value.GetType()) && value.Equals(GetDefault(value.GetType())))
            {
                return new List<ReportEntry>() { new ReportEntry()
                {
                    Code = 3003,
                    Message = "This number should not be zero.",
                    ErrorLevel = ErrorLevel,
                }};
            }

            return new List<ReportEntry>();
        }

        private bool IsEmpty(object value)
        {
            if (value == null)
            {
                return true;
            }

            if (value is string && string.IsNullOrEmpty((string) value))
            {
                return true;
            }

            if (value.GetType().GetInterfaces().Contains(typeof(ICanBeEmpty)) && ((ICanBeEmpty)value).IsEmpty())
            {
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
