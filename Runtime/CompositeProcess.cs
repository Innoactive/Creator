using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Innoactive.Creator.Core
{
    /// <summary>
    /// A process which consists of multiple processes which execute at the same time. It ends when all its child processes end.
    /// </summary>
    public class CompositeProcess : IProcess
    {
        private readonly IEnumerable<IProcess> stageProcesses;

        /// <param name="processes">Child processes which are united into this composite process.</param>
        public CompositeProcess(params IProcess[] processes)
        {
            stageProcesses = processes;
        }

        /// <inheritdoc />
        public void Start()
        {
            foreach (IProcess childProcess in stageProcesses)
            {
                childProcess.Start();
            }
        }

        /// <inheritdoc />
        public IEnumerator Update()
        {
            IEnumerator[] updates = stageProcesses.Select(process => process.Update()).ToArray();

            bool isAnyRequiredUpdateRuns = true;

            while (isAnyRequiredUpdateRuns)
            {
                isAnyRequiredUpdateRuns = false;

                for (int i = 0; i < updates.Length; i++)
                {
                    if (updates[i] == null)
                    {
                        continue;
                    }

                    if (updates[i].MoveNext())
                    {
                        isAnyRequiredUpdateRuns = true;
                    }
                    else
                    {
                        updates[i] = null;
                    }
                }

                yield return null;
            }
        }

        /// <inheritdoc />
        public void End()
        {
            foreach (IProcess childProcess in stageProcesses)
            {
                childProcess.End();
            }
        }

        /// <inheritdoc />
        public void FastForward()
        {
            foreach (IProcess childProcess in stageProcesses)
            {
                childProcess.FastForward();
            }
        }
    }
}
