using VPG.Core.Configuration.Modes;

namespace VPG.Core.EntityOwners
{
    /// <summary>
    /// An <seealso cref="IEntitySequenceData{TEntity}"/> with <seealso cref="IModeData"/>.
    /// </summary>
    public interface IEntitySequenceDataWithMode<TEntity> : IEntitySequenceData<TEntity>, IModeData where TEntity : IEntity
    {
    }
}
