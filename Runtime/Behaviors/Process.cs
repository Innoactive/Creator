using System;

namespace Innoactive.Creator.Core
{
    public class Process<TData> : IProcess<TData> where TData : IData
    {
        private readonly IStageProcess<TData> activating;
        private readonly IStageProcess<TData> active;
        private readonly IStageProcess<TData> deactivating;
        private readonly IStageProcess<TData> inactive = new EmptyStageProcess<TData>();

        public IStageProcess<TData> GetStageProcess(Stage stage)
        {
            switch (stage)
            {
                case Stage.Inactive:
                    return inactive;
                case Stage.Activating:
                    return activating;
                case Stage.Active:
                    return active;
                case Stage.Deactivating:
                    return deactivating;
                default:
                    throw new ArgumentOutOfRangeException("stage", stage, null);
            }
        }

        public Process(IStageProcess<TData> activating, IStageProcess<TData> active, IStageProcess<TData> deactivating)
        {
            this.activating = activating;
            this.active = active;
            this.deactivating = deactivating;
        }
    }
}
