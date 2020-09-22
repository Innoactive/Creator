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
            message = "Target TrainingSceneObject is missing a collider!";
        }
    }
}
