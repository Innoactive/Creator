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
        /// List of required components.
        /// </summary>
        protected List<Type> components;

        public CheckForComponentAttribute(ValidationErrorLevel errorLevel, params Type[] components)
        {
            ErrorLevel = errorLevel;
            this.components = components.ToList();
        }

        public CheckForComponentAttribute(params Type[] components) : this (ValidationErrorLevel.ERROR, components) { }

        public List<ReportEntry> Validate(object value)
        {
            GameObject gameObject = FetchGameObject(value);
            if (IsMissingComponent(gameObject))
            {
                return new List<ReportEntry>()
                {
                    CreateErrorMessage(gameObject, components)
                };
            }

            return new List<ReportEntry>();
        }

        protected virtual ReportEntry CreateErrorMessage(GameObject gameObject, List<Type> components)
        {
            return ReportEntryGenerator.MissingComponent(gameObject, string.Join(", ", components));
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
