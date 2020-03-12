using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Innoactive.Hub.Training.EntityOwners;
using Innoactive.Hub.Training.Attributes;
using Innoactive.Hub.Training.Conditions;
using Innoactive.Hub.Training.Configuration;
using Innoactive.Hub.Training.Configuration.Modes;
using Innoactive.Hub.Training.Unity.Utils;
using UnityEngine;

namespace Innoactive.Hub.Training
{
    [DataContract(IsReference = true)]
    public class Transition : CompletableEntity<Transition.EntityData>, ITransition
    {
        [DisplayName("Transition")]
        public class EntityData : EntityCollectionData<ICondition>, ITransitionData
        {
            [DataMember]
            [DisplayName("Conditions"), Foldable, Separated, ListOf(typeof(FoldableAttribute), typeof(DeletableAttribute), typeof(SeparatedAttribute)), ExtendableList]
            public IList<ICondition> Conditions { get; set; }

            public override IEnumerable<ICondition> GetChildren()
            {
                return Conditions.ToArray();
            }

            [HideInTrainingInspector]
            [DataMember]
            public IStep TargetStep { get; set; }

            public IMode Mode { get; set; }
            public bool IsCompleted { get; set; }
        }

        private class ActivatingProcess : InstantStageProcess<EntityData>
        {
            public override void Start(EntityData data)
            {
                data.IsCompleted = false;
            }
        }

        private class ActiveProcess : BaseStageProcessOverCompletable<EntityData>
        {
            protected override bool CheckIfCompleted(EntityData data)
            {
                return data.Conditions
                    .Where(condition => data.Mode.CheckIfSkipped(condition.GetType()) == false)
                    .All(condition => condition.IsCompleted);
            }
        }

        private class EntityAutocompleter : BaseAutocompleter<EntityData>
        {
            public override void Complete(EntityData data)
            {
                foreach (ICondition condition in data.Conditions.Where(condition => data.Mode.CheckIfSkipped(condition.GetType()) == false))
                {
                    condition.Autocomplete();
                }

                base.Complete(data);
            }
        }

        ITransitionData IDataOwner<ITransitionData>.Data
        {
            get
            {
                return Data;
            }
        }

        private readonly IProcess<EntityData> process = new CompositeProcess<EntityData>()
            .Add(new ParallelLifeCycleProcess<EntityData, ICondition>())
            .Add(new Process<EntityData>(new ActivatingProcess(), new ActiveProcess(), new EmptyStageProcess<EntityData>()));

        protected override IProcess<EntityData> Process
        {
            get
            {
                return process;
            }
        }

        private readonly IConfigurator<EntityData> configurator = new BaseConfigurator<EntityData>().Add(new ParallelLifeCycleConfigurator<EntityData, ICondition>());

        protected override IConfigurator<EntityData> Configurator
        {
            get
            {
                return configurator;
            }
        }

        private readonly IAutocompleter<EntityData> autocompleter = new EntityAutocompleter();

        protected override IAutocompleter<EntityData> Autocompleter
        {
            get
            {
                return autocompleter;
            }
        }

        public Transition()
        {
            Data = new EntityData()
            {
                Conditions = new List<ICondition>(),
                TargetStep = null,
            };

            if (RuntimeConfigurator.Configuration.EntityStateLoggerConfig.LogTransitions)
            {
                LifeCycle.StageChanged += (sender, args) =>
                {
                    RuntimeConfigurator.Configuration.EntityStateLogger.LogFormat(LogType.Log, "{0}<b>Transition to</b> <i>{1}</i> is <b>{2}</b>.\n", ConsoleUtils.GetTabs(3), Data.TargetStep != null ? Data.TargetStep.Data.Name + " (Step)" : "chapter's end", LifeCycle.Stage);
                };
            }
        }
    }
}
