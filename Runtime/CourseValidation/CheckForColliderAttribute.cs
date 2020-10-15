using System;
using System.Collections.Generic;
using UnityEngine;

namespace Innoactive.Creator.Core.Validation
{
    /// <summary>
    /// Checks if the GameObject referenced contains at least one collider.
    /// </summary>
    public class CheckForColliderAttribute : CheckForComponentAttribute
    {
        public CheckForColliderAttribute() : base(typeof(BoxCollider), typeof(SphereCollider), typeof(CapsuleCollider), typeof(MeshCollider), typeof(Collider))
        {
        }

        /// <inheritdoc />
        protected override ReportEntry CreateErrorMessage(GameObject gameObject, List<Type> components)
        {
            return ReportEntryGenerator.MissingCollider(gameObject);
        }
    }
}
