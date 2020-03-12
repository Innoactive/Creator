using Innoactive.Hub.Training.Configuration.Modes;

namespace Innoactive.Creator.Core.Tests.Utils.Mocks
{
    /// <summary>
    /// Same as <see cref="EndlessConditionMock"/>, but it can be skipped.
    /// </summary>
    public class OptionalEndlessConditionMock : EndlessConditionMock, IOptional
    {
    }
}
