using System.Collections;
using System.Linq;
using VPG.Core.Configuration.Modes;

namespace VPG.Core.EntityOwners.ParallelEntityCollection
{
    /// <summary>
    /// A process for a collection of entities which are activated and deactivated in parallel.
    /// </summary>
    internal class ParallelActiveProcess<TCollectionData> : Process<TCollectionData> where TCollectionData : class, IEntityCollectionData, IModeData
    {
        public ParallelActiveProcess(TCollectionData data) : base(data)
        {
        }

        /// <inheritdoc />
        public override void Start()
        {
        }

        /// <inheritdoc />
        public override IEnumerator Update()
        {
            int endlessIterationCheck = 0;
            while (endlessIterationCheck < 1000000)
            {
                yield return null;
                endlessIterationCheck++;
            }
        }

        /// <inheritdoc />
        public override void End()
        {
        }

        /// <inheritdoc />
        public override void FastForward()
        {
            foreach (IEntity child in Data.GetChildren().Where(child => child.LifeCycle.Stage == Stage.Activating))
            {
                child.LifeCycle.MarkToFastForwardStage(Stage.Activating);
            }
        }
    }
}
