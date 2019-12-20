using System.Collections;

namespace Innoactive.Hub.Training
{
    public interface IStageProcess<in TData> where TData : IData
    {
        void Start(TData data);
        IEnumerator Update(TData data);
        void End(TData data);
        void FastForward(TData data);
    }
}
