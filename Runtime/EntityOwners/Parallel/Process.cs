using System.Collections.Generic;
using System.Linq;
using VPG.Core.Configuration.Modes;

namespace VPG.Core.EntityOwners.ParallelEntityCollection
{
    /// <summary>
    /// A base process for entity collection.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    internal abstract class Process<TData> : Core.Process<TData> where TData : class, IEntityCollectionData, IModeData
    {
        /// <summary>
        /// Takes a <paramref name="collection"/> of entities and filters out the ones that must be skipped due to <paramref name="mode"/>
        /// or contains a <seealso cref="IBackgroundBehaviorData"/> with `IsBlocking` set to false.
        /// </summary>
        protected IEnumerable<IEntity> GetBlockingChildren(IEntityCollectionData collection, IMode mode)
        {
            return collection.GetChildren()
                .Where(child => mode.CheckIfSkipped(child.GetType()) == false)
                .Where(child =>
                {
                    IDataOwner dataOwner = child as IDataOwner;
                    if (dataOwner == null)
                    {
                        return true;
                    }

                    IBackgroundBehaviorData blockingData = dataOwner.Data as IBackgroundBehaviorData;
                    return blockingData == null || blockingData.IsBlocking;
                });
        }

        protected Process(TData data) : base(data)
        {
        }
    }
}
