using Innoactive.Hub.Training.Configuration.Modes;
using Innoactive.Hub.Training.EntityOwners;

namespace Innoactive.Hub.Training
{
    public interface IStepData : INamedData, IDescribedData, IEntitySequenceData<IStepChild>, IModeData
    {
        IBehaviorCollection Behaviors { get; set; }

        ITransitionCollection Transitions { get; set; }
    }
}
