using VPG.Core.EntityOwners;

namespace VPG.Core
{
    /// <summary>
    /// The interface for a step's data.
    /// </summary>
    public interface IStepData : INamedData, IDescribedData, IEntitySequenceDataWithMode<IStepChild>
    {
        /// <summary>
        /// The list of the step's behaviors.
        /// </summary>
        IBehaviorCollection Behaviors { get; set; }

        /// <summary>
        /// The list of the step's transitions.
        /// </summary>
        ITransitionCollection Transitions { get; set; }
    }
}
