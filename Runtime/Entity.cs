using System.Collections;
using System.Linq;
using System.Runtime.Serialization;
using Innoactive.Creator.Core.Configuration;
using Innoactive.Creator.Core.Configuration.Modes;
using Innoactive.Creator.Core.EntityOwners;

namespace Innoactive.Creator.Core
{
    /// <summary>
    /// Abstract helper class that can be used for instances that implement <see cref="IEntity"/>. Provides implementation of the events and properties, and also
    /// offers member functions to trigger state changes.
    /// </summary>
    [DataContract(IsReference = true)]
    public abstract class Entity<TData> : IEntity, IDataOwner<TData> where TData : IData
    {
        private readonly IConfigurator<TData> defaultConfigurator = new BaseConfigurator<TData>();

        [DataMember]
        public TData Data { get; protected set; }

        IData IDataOwner.Data
        {
            get
            {
                return ((IDataOwner<TData>)this).Data;
            }
        }

        protected abstract IProcess<TData> Process { get; }

        protected virtual IConfigurator<TData> Configurator
        {
            get
            {
                return defaultConfigurator;
            }
        }

        public ILifeCycle LifeCycle { get; private set; }

        public void InvokeProcessStart()
        {
            Process.GetStageProcess(LifeCycle.Stage).Start(Data);
        }

        public IEnumerator InvokeProcessUpdate()
        {
            return Process.GetStageProcess(LifeCycle.Stage).Update(Data);
        }

        public void InvokeProcessEnd()
        {
            Process.GetStageProcess(LifeCycle.Stage).End(Data);
        }

        public void InvokeProcessFastForward()
        {
            Process.GetStageProcess(LifeCycle.Stage).FastForward(Data);
        }

        public void Configure(IMode mode)
        {
            Configurator.Configure(Data, mode, LifeCycle.Stage);
        }

        public void Update()
        {
            LifeCycle.Update();

            IEntityCollectionData collectionData = Data as IEntityCollectionData;

            if (collectionData == null)
            {
                return;
            }

            foreach (IEntity child in collectionData.GetChildren().Distinct())
            {
                child.Update();
            }
        }

        protected Entity()
        {
            LifeCycle = new LifeCycle(this);
        }
    }
}
