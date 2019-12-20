namespace Innoactive.Hub.Training.EntityOwners
{
    public interface IEntitySequenceData<TEntity> : IEntityCollectionData<TEntity> where TEntity : IEntity
    {
        TEntity Current { get; set; }
    }
}
