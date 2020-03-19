using System.Collections;
using Innoactive.Creator.Core.Configuration.Modes;

namespace Innoactive.Creator.Core
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
