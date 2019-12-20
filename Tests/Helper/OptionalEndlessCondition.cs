#if UNITY_EDITOR

using Innoactive.Hub.Training.Configuration.Modes;

namespace Innoactive.Hub.Unity.Tests.Training
{
    /// <summary>
    /// Same as <see cref="EndlessCondition"/>, but it can be skipped.
    /// </summary>
    public class OptionalEndlessCondition : EndlessCondition, IOptional
    {
    }
}

#endif
