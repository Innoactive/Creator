using VPG.Core.Configuration.Modes;

namespace VPG.Core.EntityOwners
{
    /// <summary>
    /// A generic version of <seealso cref="IEntityCollectionDataWithMode"/>
    /// </summary>
    public interface IEntityCollectionDataWithMode<out TEntity> : IEntityCollectionData<TEntity>, IEntityCollectionDataWithMode where TEntity : IEntity
    {
    }

    /// <summary>
    /// A composition interface of <seealso cref="IEntityCollectionData"/> and <seealso cref="IModeData"/>.
    /// </summary>
    public interface IEntityCollectionDataWithMode : IEntityCollectionData, IModeData
    {
    }
}
