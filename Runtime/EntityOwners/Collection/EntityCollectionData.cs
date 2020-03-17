using System.Collections.Generic;
using System.Linq;
using Innoactive.Creator.Core.EntityOwners;

namespace Innoactive.Creator.Core.EntityOwners
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
