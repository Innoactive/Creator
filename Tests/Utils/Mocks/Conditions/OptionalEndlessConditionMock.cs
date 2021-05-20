using VPG.Core.Configuration.Modes;

namespace VPG.Tests.Utils.Mocks
{
    /// <summary>
    /// Same as <see cref="EndlessConditionMock"/>, but it can be skipped.
    /// </summary>
    public class OptionalEndlessConditionMock : EndlessConditionMock, IOptional
    {
    }
}
