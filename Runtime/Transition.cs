using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Innoactive.Creator.Core.Attributes;
using Innoactive.Creator.Core.Conditions;
using Innoactive.Creator.Core.Configuration.Modes;
using Innoactive.Creator.Core.EntityOwners;
using Innoactive.Creator.Core.RestrictiveEnvironment;
using Innoactive.Creator.Core.Utils.Logging;
using Innoactive.Creator.Unity;
using UnityEngine;

namespace Innoactive.Creator.Core
{
    /// <summary>
    /// A class for a transition from one step to another.
    /// </summary>
    [DataContract(IsReference = true)]
    public class Transition : CompletableEntity<Transition.EntityData>, ITransition, ILockablePropertiesProvider
    {
        /// <summary>
        /// The transition's data class.
        /// </summary>
        [DisplayName("Transition")]
        public class EntityData : EntityCollectionData<ICondition>, ITransitionData
        {
            ///<inheritdoc />
            [DataMember]
            [DisplayName("Conditions"), Foldable, ListOf(typeof(FoldableAttribute), typeof(DeletableAttribute)), ExtendableList]
            public IList<ICondition> Conditions { get; set; }

            ///<inheritdoc />
            public override IEnumerable<ICondition> GetChildren()
            {
                return Conditions.ToArray();
            }

            ///<inheritdoc />
            [HideInTrainingInspector]
            [DataMember]
            public IStep TargetStep { get; set; }

            ///<inheritdoc />
            public IMode Mode { get; set; }

            ///<inheritdoc />
            public bool IsCompleted { get; set; }
        }

        private class ActivatingProcess : InstantProcess<EntityData>
        {
            public ActivatingProcess(EntityData data) : base(data)
            {
            }

            ///<inheritdoc />
            public override void Start()
            {
                Data.IsCompleted = false;
            }
        }

        private class ActiveProcess : BaseActiveProcessOverCompletable<EntityData>
        {
            public ActiveProcess(EntityData data) : base(data)
            {
            }

            ///<inheritdoc />
            protected override bool CheckIfCompleted()
            {
                return Data.Conditions
                    .Where(condition => Data.Mode.CheckIfSkipped(condition.GetType()) == false)
                    .All(condition => condition.IsCompleted);
            }
        }

        private class EntityAutocompleter : Autocompleter<EntityData>
        {
            public EntityAutocompleter(EntityData data) : base(data)
            {
            }

            ///<inheritdoc />
            public override void Complete()
            {
                foreach (ICondition condition in Data.Conditions.Where(condition => Data.Mode.CheckIfSkipped(condition.GetType()) == false))
                {
                    condition.Autocomplete();
                }
            }
        }

        ///<inheritdoc />
        ITransitionData IDataOwner<ITransitionData>.Data
        {
            get { return Data; }
        }

        ///<inheritdoc />
        public override IProcess GetActivatingProcess()
        {
            return new CompositeProcess(new EntityOwners.ParallelEntityCollection.ParallelActivatingProcess<EntityData>(Data), new ActivatingProcess(Data));
        }

        ///<inheritdoc />
        public override IProcess GetActiveProcess()
        {
            return new CompositeProcess(new EntityOwners.ParallelEntityCollection.ParallelActiveProcess<EntityData>(Data), new ActiveProcess(Data));
        }

        ///<inheritdoc />
        public override IProcess GetDeactivatingProcess()
        {
            return new EntityOwners.ParallelEntityCollection.ParallelDeactivatingProcess<EntityData>(Data);
        }

        ///<inheritdoc />
        protected override IConfigurator GetConfigurator()
        {
            return new ParallelConfigurator<ICondition>(Data);
        }

        ///<inheritdoc />
        protected override IAutocompleter GetAutocompleter()
        {
            return new EntityAutocompleter(Data);
        }

        /// <inheritdoc />
        public Transition()
        {
            Data.Conditions = new List<ICondition>();
            Data.TargetStep = null;

            if (LifeCycleLoggingConfig.Instance.LogTransitions)
            {
                LifeCycle.StageChanged += (sender, args) =>
                {
                    Debug.LogFormat("{0}<b>Transition to</b> <i>{1}</i> is <b>{2}</b>.\n", ConsoleUtils.GetTabs(3), Data.TargetStep != null ? Data.TargetStep.Data.Name + " (Step)" : "chapter's end", LifeCycle.Stage);
                };
            }
        }

        public IEnumerable<LockablePropertyData> GetLockableProperties()
        {
            IEnumerable<LockablePropertyData> lockable = new List<LockablePropertyData>();
            foreach (ICondition condition in Data.Conditions)
            {
                if (condition is ILockablePropertiesProvider lockableCondition)
                {
                    lockable = lockable.Union(lockableCondition.GetLockableProperties());
                }
            }
            return lockable;
        }
    }
}
