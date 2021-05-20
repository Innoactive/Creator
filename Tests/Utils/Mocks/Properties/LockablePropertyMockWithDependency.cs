using VPG.Core.Properties;
using UnityEngine;

namespace VPG.Tests.Utils.Mocks
{
    /// <summary>
    /// Property requiring a <see cref="LockablePropertyMock"/>.
    /// </summary>
    [RequireComponent(typeof(LockablePropertyMock))]
    public class LockablePropertyMockWithDependency : LockableProperty
    {
        protected override void InternalSetLocked(bool lockState)
        {
            GetComponent<LockablePropertyMock>().SetLocked(lockState);
        }
    }
}
