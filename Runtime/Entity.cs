using System.Linq;
using System.Runtime.Serialization;
using VPG.Creator.Core.Configuration.Modes;
using VPG.Creator.Core.EntityOwners;

namespace VPG.Creator.Core
{
    /// <summary>
    /// Abstract helper class that can be used for instances that implement <see cref="IEntity"/>. Provides implementation of the events and properties, and also
    /// offers member functions to trigger state changes.
    /// </summary>
    [DataContract(IsReference = true)]
    public abstract class Entity<TData> : IEntity, IDataOwner<TData> where TData : class, IData, new()
    {
        /// <inheritdoc />
        [DataMember]
        public TData Data { get; private set; }

        /// <inheritdoc />
        IData IDataOwner.Data
        {
            get { return ((IDataOwner<TData>)this).Data; }
        }

        /// <inheritdoc />
        public ILifeCycle LifeCycle { get; }


        protected Entity()
        {
            LifeCycle = new LifeCycle(this);
            Data = new TData();
        }

        /// <inheritdoc />
        public virtual IProcess GetActivatingProcess()
        {
            return new EmptyProcess();
        }

        /// <inheritdoc />
        public virtual IProcess GetActiveProcess()
        {
            return new EmptyProcess();
        }

        /// <inheritdoc />
        public virtual IProcess GetDeactivatingProcess()
        {
            return new EmptyProcess();
        }

        /// <summary>
        /// Override this method if your behavior or condition supports changing between training modes (<see cref="IMode"/>).
        /// By default returns an empty configurator that does nothing.
        /// </summary>
        protected virtual IConfigurator GetConfigurator()
        {
            return new EmptyConfigurator();
        }

        /// <inheritdoc />
        public void Configure(IMode mode)
        {
            if (Data is IEntityCollectionData collectionData)
            {
                foreach (IEntity child in collectionData.GetChildren().Distinct())
                {
                    child.Configure(mode);
                }
            }

            GetConfigurator().Configure(mode, LifeCycle.Stage);

            if (Data is IModeData modeData)
            {
                modeData.Mode = mode;
            }
        }

        /// <inheritdoc />
        public void Update()
        {
            LifeCycle.Update();

            if (Data is IEntityCollectionData collectionData)
            {
                foreach (IEntity child in collectionData.GetChildren().Distinct())
                {
                    child.Update();
                }
            }
        }
    }
}
