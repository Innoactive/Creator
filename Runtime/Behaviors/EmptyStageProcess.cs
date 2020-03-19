using System.Collections;
using Innoactive.Creator.Core;

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
