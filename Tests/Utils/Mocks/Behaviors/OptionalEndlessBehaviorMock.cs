using VPG.Creator.Core.Configuration.Modes;

namespace VPG.Creator.Tests.Utils.Mocks
{
    public class OptionalEndlessBehaviorMock : EndlessBehaviorMock, IOptional
    {
        public OptionalEndlessBehaviorMock(bool isBlocking = true) : base(isBlocking)
        {
        }
    }
}
