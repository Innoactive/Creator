using Innoactive.Creator.Core.Configuration.Modes;
using Innoactive.Creator.Core.EntityOwners;

namespace Innoactive.Creator.Core
{
    public interface IStepData : INamedData, IDescribedData, IEntitySequenceData<IStepChild>, IModeData
    {
        IBehaviorCollection Behaviors { get; set; }

        ITransitionCollection Transitions { get; set; }
    }
}
