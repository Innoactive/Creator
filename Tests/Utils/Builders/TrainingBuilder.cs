using VPG.Core;

namespace VPG.Tests.Builder
{
    public abstract class TrainingBuilder<TCourse> : BuilderWithResourcePath<TCourse> where TCourse : ICourse
    {
        public TrainingBuilder(string name) : base(name)
        {
        }
    }
}
