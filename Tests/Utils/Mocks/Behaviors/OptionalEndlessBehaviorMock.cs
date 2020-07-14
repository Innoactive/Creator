using Innoactive.Creator.Core.Configuration.Modes;

namespace Innoactive.Creator.Tests.Utils.Mocks
{
    public class OptionalEndlessBehaviorMock : EndlessBehaviorMock, IOptional
    {
        public OptionalEndlessBehaviorMock(bool isBlocking = true) : base(isBlocking)
        {
        }
    }
}
