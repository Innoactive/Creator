namespace VPG.Creator.Core
{
    /// <summary>
    /// A collection of <see cref="Behaviors.IBehavior"/>s of a <see cref="IStep"/>.
    /// </summary>
    public interface IBehaviorCollection : IStepChild, IDataOwner<IBehaviorCollectionData>
    {
    }
}
