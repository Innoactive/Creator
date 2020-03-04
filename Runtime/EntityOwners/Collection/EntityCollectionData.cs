using System.Collections.Generic;
using System.Linq;
using Innoactive.Hub.Training.EntityOwners;

namespace Innoactive.Hub.Training.EntityOwners
{
    public abstract class EntityCollectionData<TEntity> : IEntityCollectionData<TEntity> where TEntity : IEntity
    {
        public abstract IEnumerable<TEntity> GetChildren();

        IEnumerable<IEntity> IEntityCollectionData.GetChildren()
        {
            return GetChildren().Cast<IEntity>();
        }

        public Metadata Metadata { get; set; }
    }
}