using UnityEngine;
using System.Runtime.Serialization;
using Innoactive.Creator.Core.Configuration;
using Innoactive.Creator.Unity;

namespace Innoactive.Creator.Core.Behaviors
{
    /// <summary>
    /// Inherit from this abstract class when creating your own behaviors.
    /// </summary>
    /// <typeparam name="TData">The type of the behavior's data.</typeparam>
    [DataContract(IsReference = true)]
    public abstract class Behavior<TData> : Entity<TData>, IBehavior where TData : class, IBehaviorData, new()
    {
        /// <inheritdoc />
        IBehaviorData IDataOwner<IBehaviorData>.Data
        {
            get { return Data; }
        }

        protected Behavior()
        {
            if (RuntimeConfigurator.Configuration.EntityStateLoggerConfig.LogBehaviors)
            {
                LifeCycle.StageChanged += (sender, args) =>
                {
                    RuntimeConfigurator.Configuration.EntityStateLogger.LogFormat(LogType.Log, "{0}<b>Behavior</b> <i>'{1} ({2})'</i> is <b>{3}</b>.\n", ConsoleUtils.GetTabs(2), Data.Name, GetType().Name, LifeCycle.Stage);
                };
            }
        }
    }
}
