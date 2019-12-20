#if UNITY_EDITOR

using System;
using System.Collections;
using Innoactive.Hub.Training;
using Innoactive.Hub.Training.Conditions;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

namespace Innoactive.Hub.Unity.Tests.Training
{
    public class LifeCycleTests : RuntimeTests
    {
        private class TestEntity : Entity<TestEntity.EntityData>
        {
            public class EntityData : IData
            {
                public Metadata Metadata { get; set; }

                public bool IsUpdateFinished { get; set; }
                public bool IsFastForwarded { get; set; }
                public bool IsEndCalled { get; set; }

            }

            private class ActiveProcess : IStageProcess<EntityData>
            {
                public void Start(EntityData data)
                {
                    data.IsUpdateFinished = false;
                    data.IsFastForwarded = false;
                }

                public IEnumerator Update(EntityData data)
                {
                    data.IsUpdateFinished = true;
                    yield break;
                }

                public void End(EntityData data)
                {
                    data.IsEndCalled = true;
                }

                public void FastForward(EntityData data)
                {
                    data.IsFastForwarded = true;
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

            public TestEntity()
            {
                Data = new EntityData();
            }
        }

        [UnityTest]
        public IEnumerator FastForwardNextStage()
        {
            // Given an entity,
            IEntity entity = new EndlessBehavior();

            entity.LifeCycle.Activate();

            Assert.AreEqual(Stage.Activating, entity.LifeCycle.Stage);

            // When you fast-forward its next stage,
            entity.LifeCycle.MarkToFastForwardStage(Stage.Active);

            // Then the current stage is not fast-forwarded.
            Assert.AreEqual(Stage.Activating, entity.LifeCycle.Stage);

            yield break;
        }

        [UnityTest]
        public IEnumerator FastForwardCompletedProcess()
        {
            // Given an entity in an active stage with exhausted process,
            TestEntity entity = new TestEntity();
            entity.LifeCycle.Activate();
            entity.Update();
            Assert.AreEqual(Stage.Active, entity.LifeCycle.Stage);

            int endlessLoopProtection = 0;

            while (entity.Data.IsUpdateFinished == false && endlessLoopProtection < 30)
            {
                entity.LifeCycle.Update();
            }

            Assert.IsTrue(endlessLoopProtection < 30);
            Assert.IsTrue(entity.Data.IsEndCalled);
            Assert.IsFalse(entity.Data.IsFastForwarded);
            Assert.AreEqual(Stage.Active, entity.LifeCycle.Stage);

            // When we fast-forward it,
            entity.LifeCycle.MarkToFastForwardStage(Stage.Active);

            // Nothing happens.
            Assert.IsFalse(entity.Data.IsFastForwarded);

            yield break;
        }
    }
}

#endif
