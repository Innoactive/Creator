using System.Collections;
using VPG.Creator.Core;
using VPG.Creator.Tests.Utils;
using VPG.Creator.Tests.Utils.Mocks;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

namespace VPG.Creator.Tests.Courses
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

            private class ActiveProcess : Process<EntityData>
            {
                public ActiveProcess(EntityData data) : base(data)
                {
                }

                public override void Start()
                {
                    Data.IsUpdateFinished = false;
                    Data.IsFastForwarded = false;
                }

                public override IEnumerator Update()
                {
                    Data.IsUpdateFinished = true;
                    yield break;
                }

                public override void End()
                {
                    Data.IsEndCalled = true;
                }

                public override void FastForward()
                {
                    Data.IsFastForwarded = true;
                }
            }

            public override IProcess GetActiveProcess()
            {
                return new ActiveProcess(Data);
            }
        }

        [UnityTest]
        public IEnumerator FastForwardNextStage()
        {
            // Given an entity,
            IEntity entity = new EndlessBehaviorMock();

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
