using System.Runtime.Serialization;
using Innoactive.Hub.Training.Configuration;
using Innoactive.Hub.Training.Unity.Utils;
using Newtonsoft.Json;

namespace Innoactive.Hub.Training.Behaviors
{
    [DataContract(IsReference = true)]
    public abstract class Behavior<TData> : Entity<TData>, IBehavior where TData : IBehaviorData
    {
        IBehaviorData IDataOwner<IBehaviorData>.Data
        {
            get
            {
                return Data;
            }
        }

        [JsonConstructor]
        protected Behavior()
        {
            if (RuntimeConfigurator.Configuration.EntityStateLoggerConfig.LogBehaviors)
            {
                LifeCycle.StageChanged += (sender, args) =>
                {
                    RuntimeConfigurator.Configuration.EntityStateLogger.InfoFormat("{0}<b>Behavior</b> <i>'{1} ({2})'</i> is <b>{3}</b>.\n", ConsoleUtils.GetTabs(2), Data.Name, GetType().Name, LifeCycle.Stage);
                };
            }
        }
    }
}
