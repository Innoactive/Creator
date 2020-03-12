using System;
using System.Collections.Generic;

namespace Innoactive.Hub.Training.EntityOwners
{
    [Obsolete("This is a part of the responsibility of the Entity class now.")]
    public class UpdateChildrenProcess<TData> : IProcess<TData> where TData : IEntityCollectionData
    {
        private readonly Dictionary<Stage, IStageProcess<TData>> processes = new Dictionary<Stage, IStageProcess<TData>>
        {
            { Stage.Inactive, new EmptyStageProcess<TData>() },
            { Stage.Activating, new EmptyStageProcess<TData>() },
            { Stage.Active, new EmptyStageProcess<TData>() },
            { Stage.Deactivating, new EmptyStageProcess<TData>() },
        };

        public IStageProcess<TData> GetStageProcess(Stage stage)
        {
            return processes[stage];
        }
    }
}
