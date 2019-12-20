using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Innoactive.Hub.Training
{
    public class CompositeStageProcess<TData> : IStageProcess<TData> where TData : IData
    {
        private readonly List<IStageProcess<TData>> requiredStageProcesses = new List<IStageProcess<TData>>();

        private readonly List<IStageProcess<TData>> optionalStageProcesses = new List<IStageProcess<TData>>();

        public void Start(TData data)
        {
            foreach (IStageProcess<TData> childProcess in requiredStageProcesses.Concat(optionalStageProcesses))
            {
                childProcess.Start(data);
            }
        }

        public IEnumerator Update(TData data)
        {
            IEnumerator[] requiredUpdates = requiredStageProcesses.Select(process => process.Update(data)).ToArray();
            IEnumerator[] optionalUpdates = optionalStageProcesses.Select(process => process.Update(data)).ToArray();

            bool isAnyRequiredUpdateRuns = true;

            while (isAnyRequiredUpdateRuns)
            {
                isAnyRequiredUpdateRuns = false;

                for (int i = 0; i < requiredUpdates.Length; i++)
                {
                    if (requiredUpdates[i] == null)
                    {
                        continue;
                    }

                    if (requiredUpdates[i].MoveNext())
                    {
                        isAnyRequiredUpdateRuns = true;
                    }
                    else
                    {
                        requiredUpdates[i] = null;
                    }
                }

                for (int i = 0; i < optionalUpdates.Length; i++)
                {
                    if (optionalUpdates[i] == null)
                    {
                        continue;
                    }

                    if (optionalUpdates[i].MoveNext() == false)
                    {
                        optionalUpdates[i] = null;
                    }
                }

                yield return null;
            }
        }

        public void End(TData data)
        {
            foreach (IStageProcess<TData> childProcess in requiredStageProcesses.Concat(optionalStageProcesses))
            {
                childProcess.End(data);
            }
        }

        public void FastForward(TData data)
        {
            foreach (IStageProcess<TData> childProcess in requiredStageProcesses.Concat(optionalStageProcesses))
            {
                childProcess.FastForward(data);
            }
        }

        public void Add(IStageProcess<TData> process)
        {
            requiredStageProcesses.Add(process);
        }

        public void AddOptional(IStageProcess<TData> process)
        {
            optionalStageProcesses.Add(process);
        }
    }
}
