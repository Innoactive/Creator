using Innoactive.Hub.Training.Configuration.Modes;

namespace Innoactive.Creator.Core.Tests.Utils.Mocks
{
    public class OptionalEndlessBehaviorMock : EndlessBehaviorMock, IOptional
    {
        public OptionalEndlessBehaviorMock(bool isBlocking = true) : base(isBlocking)
        {
        }
    }
}
