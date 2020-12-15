using System.Linq;
using Innoactive.Creator.Core.Properties;
using Innoactive.Creator.Unity;
using UnityEngine;

namespace Innoactive.Creator.Core.Utils
{
    /// <summary>
    /// Handles locking of all training scene objects in the scene and makes them non-interactable before the training is started.
    /// </summary>
    public class LockObjectsOnSceneStart : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Lock all training scene objects in the scene and makes them non-interactable before the training is started.")]
        private bool lockSceneObjectsOnSceneStart = true;

        // Start is called before the first frame update
        void Start()
        {
            SceneUtils.GetActiveAndInactiveComponents<LockableProperty>().ToList()
                .ForEach(lockable => lockable.SetLocked(lockSceneObjectsOnSceneStart));
        }
    }
}
