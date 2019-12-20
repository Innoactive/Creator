using System.Collections;
using Innoactive.Hub.Training;

public sealed class EmptyStageProcess<TData> : IStageProcess<TData> where TData : IData
{
    public void Start(TData data)
    {
    }

    public IEnumerator Update(TData data)
    {
        yield break;
    }

    public void End(TData data)
    {
    }

    public void FastForward(TData data)
    {
    }
}
