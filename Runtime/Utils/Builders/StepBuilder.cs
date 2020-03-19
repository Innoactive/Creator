namespace Innoactive.Creator.Core.Utils.Builders
{
    public abstract class StepBuilder<TStep> : BuilderWithResourcePath<TStep>
    {
        public StepBuilder(string name) : base(name)
        {
        }
    }
}
