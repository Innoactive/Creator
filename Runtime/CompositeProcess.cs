using System;
using System.Collections.Generic;
using System.Linq;

namespace Innoactive.Creator.Core
{
    public sealed class CompositeProcess<TData> : IProcess<TData> where TData : IData
    {
        private readonly Dictionary<Stage, CompositeStageProcess<TData>> stageProcesses;

        public IStageProcess<TData> GetStageProcess(Stage stage)
        {
            return stageProcesses[stage];
        }

        private readonly Stage[] stages = Enum.GetValues(typeof(Stage)).Cast<Stage>().ToArray();

        public CompositeProcess<TData> Add(IProcess<TData> process)
        {
            foreach (Stage stage in stages)
            {
                stageProcesses[stage].Add(process.GetStageProcess(stage));
            }

            return this;
        }

        public CompositeProcess<TData> AddOptional(IProcess<TData> process)
        {
            foreach (Stage stage in stages)
            {
                stageProcesses[stage].AddOptional(process.GetStageProcess(stage));
            }

            return this;
        }

        public CompositeProcess()
        {
            stageProcesses = Enum.GetValues(typeof(Stage))
                .Cast<Stage>()
                .ToDictionary(
                    stage => stage,
                    stage => new CompositeStageProcess<TData>());
        }
    }
}
