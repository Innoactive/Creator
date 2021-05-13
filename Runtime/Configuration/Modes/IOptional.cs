namespace VPG.Creator.Core.Configuration.Modes
{
    /// <summary>
    /// An interface that indicates that it would make sense to skip this <see cref="IEntity"/> via <see cref="IMode"/>.
    /// Makes it possible to include it to a list of entities to skip that is defined in a <see cref="IMode"/>.
    /// </summary>
    public interface IOptional : IEntity
    {
    }
}
