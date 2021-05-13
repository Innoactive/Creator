using System.Linq;
using VPG.Creator.Core.Properties;
using VPG.Creator.Unity;
using UnityEngine;

namespace VPG.Creator.Core.Utils
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
