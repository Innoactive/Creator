#if UNITY_EDITOR

using Innoactive.Hub.Training.Configuration.Modes;

namespace Innoactive.Hub.Unity.Tests.Training
{
    public class OptionalEndlessBehavior : EndlessBehavior, IOptional
    {
        public OptionalEndlessBehavior(bool isBlocking = true) : base(isBlocking)
        {
        }
    }
}

#endif
