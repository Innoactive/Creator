using UnityEngine;
using System.Runtime.Serialization;
using Innoactive.Creator.Core.Configuration;
using Innoactive.Creator.Unity;

namespace Innoactive.Creator.Core.Conditions
{
    public interface IConditionData : ICompletableData, INamedData
    {
    }

    /// <summary>
    /// An implementation of <see cref="ICondition"/>.
    /// </summary>
    [DataContract(IsReference = true)]
    public abstract class Condition<TData> : CompletableEntity<TData>, ICondition where TData : IConditionData
    {
        protected Condition()
        {
            if (RuntimeConfigurator.Configuration.EntityStateLoggerConfig.LogConditions)
            {
                LifeCycle.StageChanged += (sender, args) =>
                {
                    RuntimeConfigurator.Configuration.EntityStateLogger.LogFormat(LogType.Log, "{0}<b>Condition</b> <i>'{1} ({2})'</i> is <b>{3}</b>.\n", ConsoleUtils.GetTabs(2), Data.Name, GetType().Name, LifeCycle.Stage);
                };
            }
        }

        IConditionData IDataOwner<IConditionData>.Data
        {
            get
            {
                return Data;
            }
        }
    }
}
