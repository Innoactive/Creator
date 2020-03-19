using System.Collections;

namespace Innoactive.Creator.Core
{
    public interface IStageProcess<in TData> where TData : IData
    {
        void Start(TData data);
        IEnumerator Update(TData data);
        void End(TData data);
        void FastForward(TData data);
    }
}
