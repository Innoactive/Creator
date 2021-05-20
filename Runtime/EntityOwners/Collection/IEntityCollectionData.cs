using System.Collections.Generic;

namespace VPG.Core.EntityOwners
{
    /// <summary>
    /// A generic version of <see cref="IEntityCollectionData"/>
    /// </summary>
    public interface IEntityCollectionData<out TEntity> : IEntityCollectionData where TEntity : IEntity
    {
        new IEnumerable<TEntity> GetChildren();
    }

    /// <summary>
    /// An entity's data which represents a collection of other entities.
    /// </summary>
    public interface IEntityCollectionData : IData
    {
        IEnumerable<IEntity> GetChildren();
    }
}
