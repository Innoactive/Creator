﻿using System.Runtime.Serialization;
 using Innoactive.Creator.Core.Attributes;
 using Innoactive.Creator.Core.SceneObjects;
 using Innoactive.Creator.Core.Utils;

 namespace Innoactive.Creator.Core.Behaviors
{
    /// <summary>
    /// Behavior that unlocks the target SceneObject while active, and locks it again on deactivation (unless it was not locked initially)
    /// </summary>
    [DataContract(IsReference = true)]
    public class UnlockObjectBehavior : Behavior<UnlockObjectBehavior.EntityData>
    {
        [DisplayName("Unlock Object")]
        [DataContract(IsReference = true)]
        public class EntityData : IBehaviorData
        {
            [DataMember]
            [DisplayName("Object to unlock")]
            public SceneObjectReference Target { get; set; }

            [DataMember]
            [DisplayName("Unlock only during this step")]
            public bool IsOnlyUnlockedInStep { get; set; }

            public bool WasLockedOnActivate { get; set; }

            public Metadata Metadata { get; set; }
            public string Name { get; set; }
        }

        private class ActivatingProcess : InstantStageProcess<EntityData>
        {
            public override void Start(EntityData data)
            {
                data.WasLockedOnActivate = data.Target.Value.IsLocked;
                if (data.WasLockedOnActivate)
                {
                    data.Target.Value.SetLocked(false);
                }
            }
        }

        private class DeactivatingProcess : InstantStageProcess<EntityData>
        {
            public override void Start(EntityData data)
            {
                if (data.WasLockedOnActivate && data.IsOnlyUnlockedInStep)
                {
                    data.Target.Value.SetLocked(true);
                }
            }
        }

        public UnlockObjectBehavior() : this("") { }

        public UnlockObjectBehavior(ISceneObject target) : this(TrainingReferenceUtils.GetNameFrom(target)) { }

        public UnlockObjectBehavior(ISceneObject target, bool isOnlyUnlockedInStep) : this(TrainingReferenceUtils.GetNameFrom(target), isOnlyUnlockedInStep: isOnlyUnlockedInStep) { }

        public UnlockObjectBehavior(string targetName, string name = "Unlock Object", bool isOnlyUnlockedInStep = true)
        {
            Data = new EntityData();
            Data.Target = new SceneObjectReference(targetName);
            Data.Name = name;
            Data.IsOnlyUnlockedInStep = isOnlyUnlockedInStep;
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
