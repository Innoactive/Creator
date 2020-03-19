namespace Innoactive.Creator.Core.Utils.Builders
{
    public abstract class TrainingBuilder<TCourse> : BuilderWithResourcePath<TCourse> where TCourse : ICourse
    {
        public TrainingBuilder(string name) : base(name)
        {
        }
    }
}
