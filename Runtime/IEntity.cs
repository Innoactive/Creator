using System.Collections;
using Innoactive.Hub.Training.Configuration.Modes;

namespace Innoactive.Hub.Training
{
    public interface IEntity
    {
        ILifeCycle LifeCycle { get; }

        void InvokeProcessStart();
        IEnumerator InvokeProcessUpdate();
        void InvokeProcessFastForward();
        void InvokeProcessEnd();

        void Configure(IMode mode);

        void Update();
    }
}
