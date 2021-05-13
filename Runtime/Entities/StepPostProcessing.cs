namespace VPG.Creator.Core
{
    /// <summary>
    /// <see cref="IStep"/> implementation of <see cref="EntityPostProcessing{T}"/>.
    /// </summary>
    public class StepPostProcessing : EntityPostProcessing<IStep>
    {
        /// <inheritdoc />
        public override void Execute(IStep entity)
        {
            ITransition transition = EntityFactory.CreateTransition();
            entity.Data.Transitions.Data.Transitions.Add(transition);
        }
    }
}
