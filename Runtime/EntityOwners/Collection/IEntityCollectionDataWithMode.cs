using VPG.Creator.Core.Configuration.Modes;

namespace VPG.Creator.Core.EntityOwners
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
