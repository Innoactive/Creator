using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using VPG.Core.RestrictiveEnvironment;
using VPG.Core.Utils.Logging;
using VPG.Unity;

namespace VPG.Core.Conditions
{
    /// <summary>
    /// An implementation of <see cref="ICondition"/>. Use it as the base class for your custom conditions.
    /// </summary>
    [DataContract(IsReference = true)]
    public abstract class Condition<TData> : CompletableEntity<TData>, ICondition, ILockablePropertiesProvider where TData : class, IConditionData, new()
    {
        protected Condition()
        {
            if (LifeCycleLoggingConfig.Instance.LogConditions)
            {
                LifeCycle.StageChanged += (sender, args) =>
                {
                    Debug.LogFormat("{0}<b>Condition</b> <i>'{1} ({2})'</i> is <b>{3}</b>.\n", ConsoleUtils.GetTabs(2), Data.Name, GetType().Name, LifeCycle.Stage);
                };
            }
        }

        /// <inheritdoc />
        IConditionData IDataOwner<IConditionData>.Data
        {
            get
            {
                return Data;
            }
        }

        /// <inheritdoc />
        public virtual IEnumerable<LockablePropertyData> GetLockableProperties()
        {
            return PropertyReflectionHelper.ExtractLockablePropertiesFromConditions(Data);
        }
    }
}
