using System.Runtime.Serialization;
using Innoactive.Hub.Training.Attributes;
using Innoactive.Hub.Training.Configuration.Modes;
using Innoactive.Hub.Training.SceneObjects;
using Innoactive.Hub.Training.SceneObjects.Properties;
using Innoactive.Hub.Training.Utils;

namespace Innoactive.Hub.Training.Conditions
{
    /// <summary>
    /// Condition which is completed when `Target` is snapped into `ZoneToSnapInto`.
    /// </summary>
    [DataContract(IsReference = true)]
    public class SnappedCondition : Condition<SnappedCondition.EntityData>
    {
        [DisplayName("Snap Object")]
        [DataContract(IsReference = true)]
        public class EntityData : IConditionData
        {
            [DataMember]
            [DisplayName("Object to snap")]
            public ScenePropertyReference<SnappableProperty> Target { get; set; }

            [DataMember]
            [DisplayNameAttribute("Zone to snap into")]
            public ScenePropertyReference<SnapZoneProperty> ZoneToSnapInto { get; set; }

            public bool IsCompleted { get; set; }

            [DataMember]
            [HideInTrainingInspector]
            public string Name { get; set; }

            public Metadata Metadata { get; set; }
        }

        private class ActiveProcess : BaseStageProcessOverCompletable<EntityData>
        {
            protected override bool CheckIfCompleted(EntityData data)
            {
                return data.Target.Value.IsSnapped && (data.ZoneToSnapInto.Value == null || data.ZoneToSnapInto.Value == data.Target.Value.SnappedZone);
            }
        }

        private class EntityAutocompleter : BaseAutocompleter<EntityData>
        {
            public override void Complete(EntityData data)
            {
                data.Target.Value.FastForwardSnapInto(data.ZoneToSnapInto.Value);
                base.Complete(data);
            }
        }

        private class EntityConfigurator : IConfigurator<EntityData>
        {
            public void Configure(EntityData data, IMode mode, Stage stage)
            {
                data.ZoneToSnapInto.Value.Configure(mode);
            }
        }

        public SnappedCondition() : this("", "")
        {
        }

        public SnappedCondition(SnappableProperty target, SnapZoneProperty snapZone = null, string name = null) : this(TrainingReferenceUtils.GetNameFrom(target), TrainingReferenceUtils.GetNameFrom(snapZone), name)
        {
        }

        public SnappedCondition(string target, string snapZone, string name = "Snap Object")
        {
            Data = new EntityData()
            {
                Target = new ScenePropertyReference<SnappableProperty>(target),
                ZoneToSnapInto = new ScenePropertyReference<SnapZoneProperty>(snapZone),
                Name = name
            };
        }

        private readonly IAutocompleter<EntityData> autocompleter = new EntityAutocompleter();

        protected override IAutocompleter<EntityData> Autocompleter
        {
            get
            {
                return autocompleter;
            }
        }

        private readonly IProcess<EntityData> process = new ActiveOnlyProcess<EntityData>(new ActiveProcess());

        protected override IProcess<EntityData> Process
        {
            get
            {
                return process;
            }
        }

        private readonly IConfigurator<EntityData> configurator = new EntityConfigurator();

        protected override IConfigurator<EntityData> Configurator
        {
            get
            {
                return configurator;
            }
        }
    }
}
