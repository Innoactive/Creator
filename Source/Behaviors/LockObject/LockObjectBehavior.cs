﻿using System.Runtime.Serialization;
using Innoactive.Hub.Training.Attributes;
using Innoactive.Hub.Training.SceneObjects;
using Innoactive.Hub.Training.Utils;
using Newtonsoft.Json;

namespace Innoactive.Hub.Training.Behaviors
{
    /// <summary>
    /// Behavior that locks the target SceneObject while active, and unlocks it again on deactivation (unless it was locked initially)
    /// </summary>
    [DataContract(IsReference = true)]
    public class LockObjectBehavior : Behavior<LockObjectBehavior.EntityData>
    {
        [DisplayName("Lock Object")]
        [DataContract(IsReference = true)]
        public class EntityData : IBehaviorData
        {
            [DataMember]
            [DisplayName("Object to lock")]
            public SceneObjectReference Target { get; set; }

            [DataMember]
            [DisplayName("Lock only during this step")]
            public bool IsOnlyLockedInStep { get; set; }

            public bool WasLockedOnActivate { get; set; }

            public Metadata Metadata { get; set; }
            public string Name { get; set; }
        }

        private class ActivatingProcess : InstantStageProcess<EntityData>
        {
            public override void Start(EntityData data)
            {
                data.WasLockedOnActivate = data.Target.Value.IsLocked;
                if (data.WasLockedOnActivate == false)
                {
                    data.Target.Value.SetLocked(true);
                }
            }
        }

        private class DeactivatingProcess : InstantStageProcess<EntityData>
        {
            public override void Start(EntityData data)
            {
                if (data.WasLockedOnActivate == false && data.IsOnlyLockedInStep)
                {
                    data.Target.Value.SetLocked(false);
                }
            }
        }


        [JsonConstructor]
        public LockObjectBehavior() : this("") { }

        public LockObjectBehavior(ISceneObject target) : this(TrainingReferenceUtils.GetNameFrom(target)) { }

        public LockObjectBehavior(ISceneObject target, bool isOnlyLockedInStep) : this(TrainingReferenceUtils.GetNameFrom(target), isOnlyLockedInStep: isOnlyLockedInStep) { }

        public LockObjectBehavior(string targetName, string name = "Lock Object", bool isOnlyLockedInStep = true)
        {
            Data = new EntityData();
            Data.Target = new SceneObjectReference(targetName);
            Data.Name = name;
            Data.IsOnlyLockedInStep = isOnlyLockedInStep;
        }

        private readonly IProcess<EntityData> process = new Process<EntityData>(new ActivatingProcess(), new EmptyStageProcess<EntityData>(), new DeactivatingProcess());
        protected override IProcess<EntityData> Process
        {
            get
            {
                return process;
            }
        }
    }
}
