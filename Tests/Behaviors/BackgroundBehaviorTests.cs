#if UNITY_EDITOR

using System.Collections;
using Innoactive.Hub.Training;
using Innoactive.Hub.Training.Configuration;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

namespace Innoactive.Hub.Unity.Tests.Training.Behaviors
{
    public class BackgroundBehaviorTests : RuntimeTests
    {
        [UnityTest]
        public IEnumerator NonBlockingBehaviorActivating()
        {
            // Given a non-blocking behavior,
            EndlessBehavior behavior = new EndlessBehavior(false);
            behavior.Configure(RuntimeConfigurator.Configuration.GetCurrentMode());

            // When we activate it,
            behavior.LifeCycle.Activate();

            // Then behavior starts its activation.
            Assert.AreEqual(Stage.Activating, behavior.LifeCycle.Stage);

            yield break;
        }

        [UnityTest]
        public IEnumerator NonBlockingBehaviorActivated()
        {
            // Given a non-blocking behavior,
            EndlessBehavior behavior = new EndlessBehavior(false);
            behavior.Configure(RuntimeConfigurator.Configuration.GetCurrentMode());

            // When we activate and finish activation,
            behavior.LifeCycle.Activate();

            behavior.LifeCycle.MarkToFastForward();

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
            // Given a blocking behavior,
            EndlessBehavior behavior = new EndlessBehavior(true);
            behavior.Configure(RuntimeConfigurator.Configuration.GetCurrentMode());

            // When we activate it,
            behavior.LifeCycle.Activate();

            // Then it is immediately activating.
            Assert.AreEqual(Stage.Activating, behavior.LifeCycle.Stage);

            yield break;
        }

        [UnityTest]
        public IEnumerator FastForwardInactiveNonBlockingBehavior()
        {
            // Given a non-blocking behavior,
            EndlessBehavior behavior = new EndlessBehavior(false);
            behavior.Configure(RuntimeConfigurator.Configuration.GetCurrentMode());

            // When we mark it to fast-forward,
            behavior.LifeCycle.MarkToFastForward();

            // Then it doesn't autocomplete because it hasn't been activated yet.
            Assert.AreEqual(Stage.Inactive, behavior.LifeCycle.Stage);

            yield break;
        }

        [UnityTest]
        public IEnumerator FastForwardInactiveNonBlockingBehaviorAndActivateIt()
        {
            // Given a non-blocking behavior,
            EndlessBehavior behavior = new EndlessBehavior(false);
            behavior.Configure(RuntimeConfigurator.Configuration.GetCurrentMode());

            // When we mark it to fast-forward and activate it,
            behavior.LifeCycle.MarkToFastForward();
            behavior.LifeCycle.Activate();

            // Then the behavior should be activated immediately.
            Assert.AreEqual(Stage.Active, behavior.LifeCycle.Stage);

            yield break;
        }

        [UnityTest]
        public IEnumerator FastForwardInactiveBlockingBehaviorAndActivateIt()
        {
            // Given a blocking behavior,
            EndlessBehavior behavior = new EndlessBehavior(true);
            behavior.Configure(RuntimeConfigurator.Configuration.GetCurrentMode());

            // When we mark it to fast-forward and activate it,
            behavior.LifeCycle.MarkToFastForward();
            behavior.LifeCycle.Activate();

            // Then the behavior should be activated immediately.
            Assert.AreEqual(Stage.Active, behavior.LifeCycle.Stage);

            yield break;
        }

        [UnityTest]
        public IEnumerator FastForwardActivatingNonBlockingBehavior()
        {
            // Given an activating non-blocking behavior,
            EndlessBehavior behavior = new EndlessBehavior(false);
            behavior.Configure(RuntimeConfigurator.Configuration.GetCurrentMode());

            behavior.LifeCycle.Activate();

            // When we mark it to fast-forward,
            behavior.LifeCycle.MarkToFastForward();

            // Then the behavior should be activated immediately.
            Assert.AreEqual(Stage.Active, behavior.LifeCycle.Stage);

            yield break;
        }

        [UnityTest]
        public IEnumerator FastForwardActivatingBlockingBehavior()
        {
            // Given an activating blocking behavior,
            EndlessBehavior behavior = new EndlessBehavior(true);
            behavior.Configure(RuntimeConfigurator.Configuration.GetCurrentMode());

            behavior.LifeCycle.Activate();

            // When we mark it to fast-forward,
            behavior.LifeCycle.MarkToFastForward();

            // Then the behavior should be activated immediately.
            Assert.AreEqual(Stage.Active, behavior.LifeCycle.Stage);

            yield break;
        }

        [UnityTest]
        public IEnumerator NonBlockingBehaviorDoesNotBlock()
        {
            // Given a chapter with a step with no conditions but a transition to the end, and a non-blocking endless behavior,
            EndlessBehavior nonBlockingBehavior = new EndlessBehavior(false);
            ITransition transition = new Transition();
            transition.Data.TargetStep = null;
            IStep step = new Step("NonBlockingStep");
            step.Data.Transitions.Data.Transitions.Add(transition);
            step.Data.Behaviors.Data.Behaviors.Add(nonBlockingBehavior);
            IChapter chapter = new Chapter("NonBlockingChapter", step);

            chapter.Configure(RuntimeConfigurator.Configuration.GetCurrentMode());

            // When we activate the chapter,
            chapter.LifeCycle.Activate();

            // Then it will finish activation immediately after a few update cycles.
            while (chapter.LifeCycle.Stage != Stage.Active)
            {
                yield return null;
                chapter.Update();
            }

            Assert.AreEqual(Stage.Active, chapter.LifeCycle.Stage);
        }

        [UnityTest]
        public IEnumerator BlockingBehaviorDoesBlock()
        {
            // Given a chapter with a step with no conditions but a transition to the end, and a blocking endless behavior,
            EndlessBehavior blockingBehavior = new EndlessBehavior(true);
            ITransition transition = new Transition();
            transition.Data.TargetStep = null;
            IStep step = new Step("BlockingStep");
            step.Data.Transitions.Data.Transitions.Add(transition);
            step.Data.Behaviors.Data.Behaviors.Add(blockingBehavior);
            IChapter chapter = new Chapter("BlockingChapter", step);

            chapter.Configure(RuntimeConfigurator.Configuration.GetCurrentMode());

            // When we activate the chapter,
            chapter.LifeCycle.Activate();

            while (blockingBehavior.LifeCycle.Stage != Stage.Activating)
            {
                yield return null;
                chapter.Update();
            }

            // When endless behavior stays activating even after a few frames,
            int waitingFrames = 10;
            while (waitingFrames > 0)
            {
                yield return null;
                chapter.Update();
                Assert.AreEqual(Stage.Activating, blockingBehavior.LifeCycle.Stage);
                Assert.AreEqual(Stage.Activating, chapter.LifeCycle.Stage);
                waitingFrames--;
            }

            // Then the chapter will not be activated until the behavior finishes.
            blockingBehavior.LifeCycle.MarkToFastForward();

            Assert.AreEqual(Stage.Activating, chapter.LifeCycle.Stage);

            while (chapter.LifeCycle.Stage != Stage.Active)
            {
                yield return null;
                chapter.Update();
            }

            Assert.AreEqual(Stage.Active, chapter.LifeCycle.Stage);
        }

        [UnityTest]
        public IEnumerator NonBlockingBehaviorLoop()
        {
            // Given a chapter with a step with a loop transition, and a non-blocking endless behavior,
            EndlessBehavior nonBlockingBehavior = new EndlessBehavior(false);
            ITransition transition = new Transition();
            IStep step = new Step("NonBlockingStep");
            transition.Data.TargetStep = step;
            step.Data.Transitions.Data.Transitions.Add(transition);
            step.Data.Behaviors.Data.Behaviors.Add(nonBlockingBehavior);
            IChapter chapter = new Chapter("NonBlockingChapter", step);

            chapter.Configure(RuntimeConfigurator.Configuration.GetCurrentMode());

            // When we activate the chapter,
            chapter.LifeCycle.Activate();

            // Then it will loop without any problems.
            int loops = 3;
            while (loops > 0)
            {
                while (step.LifeCycle.Stage != Stage.Activating)
                {
                    yield return null;
                    chapter.Update();
                }

                while (step.LifeCycle.Stage != Stage.Active)
                {
                    yield return null;
                    chapter.Update();
                }

                while (step.LifeCycle.Stage != Stage.Deactivating)
                {
                    yield return null;
                    chapter.Update();
                }

                while (step.LifeCycle.Stage != Stage.Inactive)
                {
                    yield return null;
                    chapter.Update();
                }

                Assert.AreEqual(Stage.Activating, chapter.LifeCycle.Stage);
                loops--;
            }

            Assert.AreEqual(Stage.Inactive, step.LifeCycle.Stage);
        }
    }
}

#endif
