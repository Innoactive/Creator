using VPG.Creator.Core.Properties;

namespace VPG.Creator.Tests.Utils.Mocks
{
    public class LockablePropertyMock : LockableProperty, ILockablePropertyMock
    {
        protected override void InternalSetLocked(bool lockState)
        {
        }
    }
}
