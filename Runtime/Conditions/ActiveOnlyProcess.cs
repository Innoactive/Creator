namespace Innoactive.Creator.Core.Conditions
{
    public class ActiveOnlyProcess<TData> : Process<TData> where TData : IData
    {
        public ActiveOnlyProcess(IStageProcess<TData> active) : base(new EmptyStageProcess<TData>(), active, new EmptyStageProcess<TData>())
        {
        }
    }
}
