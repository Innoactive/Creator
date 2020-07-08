using Innoactive.Creator.Core.Properties;

namespace Innoactive.Creator.Tests.Utils.Mocks
{
    public class LockablePropertyMock : LockableProperty, ILockablePropertyMock
    {
        protected override void InternalSetLocked(bool lockState)
        {
        }
    }
}
