namespace Innoactive.Hub.Training.Configuration.Modes
{
    /// <summary>
    /// An interface that indicates that it would make sense to skip this <see cref="Innoactive.Hub.Training.IEntity"/> via <see cref="Innoactive.Hub.Training.Configuration.Modes.IMode"/>.
    /// Makes it possible to include it to a list of entities to skip that is defined in a <see cref="Innoactive.Hub.Training.Configuration.Modes.IMode"/>.
    /// </summary>
    public interface IOptional : IEntity
    {
    }
}
