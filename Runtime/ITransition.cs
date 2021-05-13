namespace VPG.Creator.Core
{
    /// <summary>
    /// An interface for a transition that determines when a <see cref="IStep"/> is completed and what is the next <see cref="IStep"/>.
    /// </summary>
    public interface ITransition : IEntity, ICompletable, IDataOwner<ITransitionData>
    {
    }
}
