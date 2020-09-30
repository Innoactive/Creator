using System;
using System.Linq;
using System.Collections.Generic;
using Innoactive.Creator.Core.SceneObjects;
using Innoactive.Creator.Core.Configuration;
using UnityEngine;

namespace Innoactive.Creator.Core.Validation
{
    /// <summary>
    /// Checks if the referenced GameObject has at least one of the listed Types added as Component.
    /// </summary>
    public class CheckForComponentAttribute : Attribute, IAttributeValidator
    {
        /// <inheritdoc/>
        public virtual ValidationErrorLevel ErrorLevel { get; }

        /// <summary>
        /// Detailed description of the issue.
        /// </summary>
        protected string message = "One Component of {0} is missing";

        /// <summary>
        /// List of required components.
        /// </summary>
        protected List<Type> components;

        public CheckForComponentAttribute(ValidationErrorLevel errorLevel, params Type[] components)
        {
            ErrorLevel = errorLevel;
            this.components = components.ToList();
        }

        public CheckForComponentAttribute(params Type[] components) : this (ValidationErrorLevel.ERROR, components) { }

        /// <inheritdoc/>
        public virtual bool Validate(object value, out string message)
        {
            message = "";
            if (IsMissingComponent(FetchGameObject(value)))
            {
                message = String.Format(this.message, string.Join(", ", components));
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if required component is missing.
        /// </summary>
        protected virtual bool IsMissingComponent(GameObject gameObject)
        {
            if (gameObject == null)
            {
                return false;
            }

            return components.All(component => gameObject.GetComponent(component) == null);
        }

        /// <summary>
        /// Retrieves GameObject from provided reference value.
        /// </summary>
        protected virtual GameObject FetchGameObject(object value)
        {
            if (value != null && value is UniqueNameReference reference)
            {
                if (RuntimeConfigurator.Configuration.SceneObjectRegistry.ContainsName(reference.UniqueName))
                {
                    return RuntimeConfigurator.Configuration.SceneObjectRegistry.GetByName(reference.UniqueName).GameObject;
                }
            }

            return null;
        }
    }
}
