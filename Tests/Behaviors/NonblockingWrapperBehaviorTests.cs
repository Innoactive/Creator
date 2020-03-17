using System;
using System.Collections;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Behaviors;
using Innoactive.Creator.Core.Configuration;
using Innoactive.Creator.Core.Configuration.Modes;
using Innoactive.Creator.Tests.Utils;
using Innoactive.Creator.Tests.Utils.Mocks;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

namespace Innoactive.Creator.Tests.Behaviors
{
    [Obsolete("NonblockingWrapperBehavior is obsolete.")]
    public class NonblockingWrapperBehaviorTests : RuntimeTests
    {
        [UnityTest]
        public IEnumerator WrappedBehaviorActivating()
        {
            // Given non-blocking behavior which wraps a behavior that is not activated immediately,
            NonblockingWrapperBehavior behavior = new NonblockingWrapperBehavior(new EndlessBehaviorMock(), false);
            behavior.Configure(RuntimeConfigurator.Configuration.GetCurrentMode());

            // When we activate it,
            behavior.LifeCycle.Activate();

            // Then wrapped behavior starts its activation.
            Assert.AreEqual(Stage.Activating, behavior.Data.Behavior.LifeCycle.Stage);

            yield break;
        }

        [UnityTest]
        public IEnumerator BehaviorActivated()
        {
            // Given non-blocking behavior which wraps a behavior that is not activated immediately,
            EndlessBehaviorMock endlessBehaviorMock = new EndlessBehaviorMock();
            NonblockingWrapperBehavior behavior = new NonblockingWrapperBehavior(endlessBehaviorMock, false);
            behavior.Configure(RuntimeConfigurator.Configuration.GetCurrentMode());

            // When we activate it,
            behavior.LifeCycle.Activate();

            endlessBehaviorMock.LifeCycle.MarkToFastForward();

            yield return null;
            behavior.Update();

            yield return null;
            behavior.Update();

            // Then it is activated.
            Assert.AreEqual(Stage.Active, behavior.LifeCycle.Stage);
        }

        [UnityTest]
        public IEnumerator BlockingBehaviorActivating()
        {
            // Given blocking behavior which wraps a behavior that is not activated immediately,
            NonblockingWrapperBehavior behavior = new NonblockingWrapperBehavior(new EndlessBehaviorMock(), true);
            behavior.Configure(RuntimeConfigurator.Configuration.GetCurrentMode());

            // When we activate it,
            behavior.LifeCycle.Activate();

            // Then it is immediately activated.
            Assert.AreEqual(Stage.Activating, behavior.LifeCycle.Stage);

            yield break;
        }

        [UnityTest]
        public IEnumerator FastForwardInactiveBehavior()
        {
            // Given non-blocking behavior which wraps a behavior that is not activated immediately,
            NonblockingWrapperBehavior behavior = new NonblockingWrapperBehavior(new EndlessBehaviorMock(), false);
            behavior.Configure(RuntimeConfigurator.Configuration.GetCurrentMode());

            // When we mark it to fast-forward,
            behavior.LifeCycle.MarkToFastForward();

            // Then it doesn't autocomplete because it hasn't been activated yet.
            Assert.AreEqual(Stage.Inactive, behavior.Data.Behavior.LifeCycle.Stage);
            Assert.AreEqual(Stage.Inactive, behavior.LifeCycle.Stage);

            yield break;
        }

        [UnityTest]
        public IEnumerator FastForwardInactiveBehaviorAndActivateIt()
        {
            // Given non-blocking behavior which wraps a behavior that is not activated immediately,
            NonblockingWrapperBehavior behavior = new NonblockingWrapperBehavior(new EndlessBehaviorMock(), false);
            behavior.Configure(RuntimeConfigurator.Configuration.GetCurrentMode());

            // When we mark it to fast-forward and activate it,
            behavior.LifeCycle.MarkToFastForward();
            behavior.LifeCycle.Activate();

            // Then the behavior and its wrapped behavior should be activated immediately.
            Assert.AreEqual(Stage.Active, behavior.Data.Behavior.LifeCycle.Stage);
            Assert.AreEqual(Stage.Active, behavior.LifeCycle.Stage);

            yield break;
        }

        [UnityTest]
        public IEnumerator FastForwardInactiveBlockingBehaviorAndActivateIt()
        {
            // Given non-blocking behavior which wraps a behavior that is not activated immediately,
            NonblockingWrapperBehavior behavior = new NonblockingWrapperBehavior(new EndlessBehaviorMock(), true);
            behavior.Configure(RuntimeConfigurator.Configuration.GetCurrentMode());

            // When we mark it to fast-forward and activate it,
            behavior.LifeCycle.MarkToFastForward();
            behavior.LifeCycle.Activate();

            // Then the behavior and its wrapped behavior should be activated immediately.
            Assert.AreEqual(Stage.Active, behavior.Data.Behavior.LifeCycle.Stage);
            Assert.AreEqual(Stage.Active, behavior.LifeCycle.Stage);

            yield break;
        }

        [UnityTest]
        public IEnumerator FastForwardActivatingBehavior()
        {
            // Given an activating non-blocking behavior which wraps a behavior that is not activated immediately,
            NonblockingWrapperBehavior behavior = new NonblockingWrapperBehavior(new EndlessBehaviorMock(), false);
            behavior.Configure(RuntimeConfigurator.Configuration.GetCurrentMode());

            behavior.LifeCycle.Activate();

            // When we mark it to fast-forward,
            behavior.LifeCycle.MarkToFastForward();

            // Then the behavior and its wrapped behavior should be activated immediately.
            Assert.AreEqual(Stage.Active, behavior.Data.Behavior.LifeCycle.Stage);
            Assert.AreEqual(Stage.Active, behavior.LifeCycle.Stage);

            yield break;
        }

        [UnityTest]
        public IEnumerator FastForwardActivatingBlockingBehavior()
        {
            // Given an activating non-blocking behavior which wraps a behavior that is not activated immediately,
            NonblockingWrapperBehavior behavior = new NonblockingWrapperBehavior(new EndlessBehaviorMock(), true);
            behavior.Configure(RuntimeConfigurator.Configuration.GetCurrentMode());

            behavior.LifeCycle.Activate();

            // When we mark it to fast-forward,
            behavior.LifeCycle.MarkToFastForward();

            // Then the behavior and its wrapped behavior should be activated immediately.
            Assert.AreEqual(Stage.Active, behavior.Data.Behavior.LifeCycle.Stage);
            Assert.AreEqual(Stage.Active, behavior.LifeCycle.Stage);

            yield break;
        }

        [UnityTest]
        public IEnumerator SkipChildBlockingActivating()
        {
            // Given an activating blocking behavior with an optional child,
            OptionalEndlessBehaviorMock optional = new OptionalEndlessBehaviorMock();
            NonblockingWrapperBehavior behavior = new NonblockingWrapperBehavior(optional, true);

            // When a new mode is set and the optional child has to be skipped,
            //RuntimeConfigurator.Configuration = new DynamicRuntimeConfiguration(new Mode("Test", new WhitelistTypeRule<IOptional>().Add<OptionalEndlessBehavior>()));
            behavior.Configure(new Mode("Test", new WhitelistTypeRule<IOptional>().Add<OptionalEndlessBehaviorMock>()));

            behavior.LifeCycle.Activate();

            while (behavior.LifeCycle.Stage != Stage.Active)
            {
                behavior.Update();
                yield return null;
            }

            // The child is deactivated and behavior has finished its activation.
            Assert.AreEqual(Stage.Inactive, optional.LifeCycle.Stage);
            Assert.AreEqual(Stage.Active, behavior.LifeCycle.Stage);
            yield break;
        }

        [UnityTest]
        public IEnumerator SkipChildBlockingActive()
        {
            // Given an active blocking behavior with an optional child,
            OptionalEndlessBehaviorMock optional = new OptionalEndlessBehaviorMock();
            NonblockingWrapperBehavior behavior = new NonblockingWrapperBehavior(optional, true);
            behavior.Configure(RuntimeConfigurator.Configuration.GetCurrentMode());

            behavior.LifeCycle.Activate();
            optional.LifeCycle.MarkToFastForwardStage(Stage.Activating);

            while (optional.LifeCycle.Stage != Stage.Active)
            {
                yield return null;
                behavior.Update();
            }

            // When a new mode is set and the optional child has to be skipped,
            behavior.Configure(new Mode("Test", new WhitelistTypeRule<IOptional>().Add<OptionalEndlessBehaviorMock>()));

            while (behavior.LifeCycle.Stage != Stage.Active)
            {
                yield return null;
                behavior.Update();
                Assert.AreEqual(Stage.Inactive, optional.LifeCycle.Stage);
            }

            // The child is deactivated and behavior has finished its activation.
            Assert.AreEqual(Stage.Inactive, optional.LifeCycle.Stage);
            Assert.AreEqual(Stage.Active, behavior.LifeCycle.Stage);
            yield break;
        }

        [UnityTest]
        public IEnumerator SkipChildBlockingDeactivating()
        {
            // Given a deactivating blocking behavior with an optional child,
            OptionalEndlessBehaviorMock optional = new OptionalEndlessBehaviorMock();
            NonblockingWrapperBehavior behavior = new NonblockingWrapperBehavior(optional, true);
            behavior.Configure(RuntimeConfigurator.Configuration.GetCurrentMode());

            behavior.LifeCycle.Activate();
            optional.LifeCycle.MarkToFastForwardStage(Stage.Activating);

            while (optional.LifeCycle.Stage != Stage.Active)
            {
                yield return null;
                behavior.Update();
            }

            behavior.LifeCycle.Deactivate();
            optional.LifeCycle.MarkToFastForwardStage(Stage.Deactivating);

            while (behavior.LifeCycle.Stage != Stage.Inactive)
            {
                yield return null;
                behavior.Update();
            }

            // When a new mode is set and the optional child has to be skipped,
            behavior.Configure(new Mode("Test", new WhitelistTypeRule<IOptional>().Add<OptionalEndlessBehaviorMock>()));

            // Wait a frame because configure doesn't even change anything about the stages.
            yield return null;
            behavior.Update();

            // The child is deactivated and behavior has finished its deactivation.
            Assert.AreEqual(Stage.Inactive, optional.LifeCycle.Stage);
            Assert.AreEqual(Stage.Inactive, behavior.LifeCycle.Stage);
            yield break;
        }

        [UnityTest]
        public IEnumerator UnskipChildBlockingActive()
        {
            // Given an active blocking behavior with an optional child,
            OptionalEndlessBehaviorMock optional = new OptionalEndlessBehaviorMock();
            NonblockingWrapperBehavior behavior = new NonblockingWrapperBehavior(optional, true);
            behavior.Configure(new Mode("Test", new WhitelistTypeRule<IOptional>().Add<OptionalEndlessBehaviorMock>()));

            behavior.LifeCycle.Activate();
            while (behavior.LifeCycle.Stage != Stage.Active)
            {
                yield return null;
                behavior.Update();
            }

            // When a new mode is set and the optional child has to be reactivated,
            behavior.Configure(new Mode("Test", new WhitelistTypeRule<IOptional>()));

            yield return null;
            behavior.Update();

            // The child is activating.
            Assert.AreEqual(Stage.Activating, optional.LifeCycle.Stage);
            yield break;
        }

        [UnityTest]
        public IEnumerator SkipChildNonBlockingActive()
        {
            // Given an active non-blocking behavior with an optional child,
            OptionalEndlessBehaviorMock optional = new OptionalEndlessBehaviorMock();
            NonblockingWrapperBehavior behavior = new NonblockingWrapperBehavior(optional, false);
            behavior.Configure(RuntimeConfigurator.Configuration.GetCurrentMode());

            behavior.LifeCycle.Activate();
            optional.LifeCycle.MarkToFastForwardStage(Stage.Activating);

            while (optional.LifeCycle.Stage != Stage.Active)
            {
                yield return null;
                behavior.Update();
            }

            // When a new mode is set and the optional child has to be skipped,
            behavior.Configure(new Mode("Test", new WhitelistTypeRule<IOptional>().Add<OptionalEndlessBehaviorMock>()));

            while (behavior.LifeCycle.Stage != Stage.Active)
            {
                yield return null;
                behavior.Update();
                Assert.AreEqual(Stage.Inactive, optional.LifeCycle.Stage);
            }

            // The child is deactivated and behavior has finished it's activation.
            Assert.AreEqual(Stage.Inactive, optional.LifeCycle.Stage);
            Assert.AreEqual(Stage.Active, behavior.LifeCycle.Stage);
            yield break;
        }

        [UnityTest]
        public IEnumerator UnskipChildNotBlockingActive()
        {
            // Given an active nonblocking behavior with an optional child,
            OptionalEndlessBehaviorMock optional = new OptionalEndlessBehaviorMock();
            NonblockingWrapperBehavior behavior = new NonblockingWrapperBehavior(optional, false);
            behavior.Configure(new Mode("Test", new WhitelistTypeRule<IOptional>().Add<OptionalEndlessBehaviorMock>()));

            behavior.LifeCycle.Activate();
            while (behavior.LifeCycle.Stage != Stage.Active)
            {
                yield return null;
                behavior.Update();
            }

            // When a new mode is set and the optional child has to be reactivated,
            behavior.Configure(new Mode("Test", new WhitelistTypeRule<IOptional>()));

            yield return null;
            behavior.Update();

            // The child is activating.
            Assert.AreEqual(Stage.Activating, optional.LifeCycle.Stage);
            yield break;
        }
    }
}
