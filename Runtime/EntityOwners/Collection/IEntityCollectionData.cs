using System.Collections.Generic;

namespace Innoactive.Hub.Training.EntityOwners
{
    public interface IEntityCollectionData<TEntity> : IEntityCollectionData where TEntity : IEntity
    {
        new IEnumerable<TEntity> GetChildren();
    }

    public interface IEntityCollectionData : IData
    {
        IEnumerable<IEntity> GetChildren();
    }
}
