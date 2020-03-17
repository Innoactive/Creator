using System.Collections.Generic;
using System.Linq;
using Innoactive.Creator.Core.EntityOwners;

namespace Innoactive.Creator.Core.Configuration.Modes
{
    public sealed class BaseConfigurator<TData> : IConfigurator<TData> where TData : IData
    {
        private readonly List<IConfigurator<TData>> configurators = new List<IConfigurator<TData>>();

        public void Configure(TData data, IMode mode, Stage stage)
        {
            IEntityCollectionData collectionData = data as IEntityCollectionData;
            if (collectionData != null)
            {
                foreach (IEntity child in collectionData.GetChildren().Distinct())
                {
                    child.Configure(mode);
                }
            }

            foreach (IConfigurator<TData> configurator in configurators)
            {
                configurator.Configure(data, mode, stage);
            }

            IModeData modeData = data as IModeData;
            if (modeData != null)
            {
                modeData.Mode = mode;
            }
        }

        public BaseConfigurator<TData> Add(IConfigurator<TData> configurator)
        {
            configurators.Add(configurator);
            return this;
        }
    }
}
