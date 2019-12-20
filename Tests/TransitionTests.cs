#if UNITY_EDITOR

using System.Collections;
using Innoactive.Hub.Training;
using Innoactive.Hub.Training.Configuration;
using Innoactive.Hub.Training.Configuration.Modes;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

namespace Innoactive.Hub.Unity.Tests.Training
{
    public class TransitionTests : RuntimeTests
    {
        [UnityTest]
        public IEnumerator SkipSingleConditionBeforeActivation()
        {
            // Given an inactive transition with a skipped condition,
            Transition transition = new Transition();
            transition.Data.Conditions.Add(new OptionalEndlessCondition());

            transition.Configure(new Mode("Test", new WhitelistTypeRule<IOptional>().Add<OptionalEndlessCondition>()));

            // When the transition is activated,
            transition.LifeCycle.Activate();

            while (transition.IsCompleted == false)
            {
                yield return null;
                transition.Update();
            }

            // Then it is immediately completed.
            Assert.IsTrue(transition.IsCompleted);

            yield break;
        }

        [UnityTest]
        public IEnumerator SkipSingleConditionDuringActivation()
        {
            // Given an activating transition with a condition,
            Transition transition = new Transition();
            transition.Data.Conditions.Add(new OptionalEndlessCondition());
            transition.Configure(RuntimeConfigurator.Configuration.GetCurrentMode());

            transition.LifeCycle.Activate();

            while (transition.LifeCycle.Stage != Stage.Active)
            {
                yield return null;
                transition.Update();
            }

            // When the condition is skipped,
            transition.Configure(new Mode("Test", new WhitelistTypeRule<IOptional>().Add<OptionalEndlessCondition>()));

            yield return null;
            transition.Update();

            // Then the transition immediately completes.
            Assert.IsTrue(transition.IsCompleted);

            yield break;
        }

        [UnityTest]
        public IEnumerator SkipOneOfTheConditionsDuringActivation()
        {
            // Given an activating transition with a condition,
            Transition transition = new Transition();
            transition.Data.Conditions.Add(new OptionalEndlessCondition());
            transition.Data.Conditions.Add(new EndlessCondition());
            transition.Configure(RuntimeConfigurator.Configuration.GetCurrentMode());

            transition.LifeCycle.Activate();

            while (transition.LifeCycle.Stage != Stage.Active)
            {
                yield return null;
                transition.Update();
            }

            // When the condition is skipped,
            transition.Configure(new Mode("Test", new WhitelistTypeRule<IOptional>().Add<OptionalEndlessCondition>()));

            yield return null;
            transition.Update();

            // Then the transition is not completed, as the second condition was never completed.
            Assert.IsFalse(transition.IsCompleted);

            yield break;
        }

        [UnityTest]
        public IEnumerator InactiveConditionDoesntPreventCompletion()
        {
            EndlessCondition notOptional = new EndlessCondition();

            // Given an activating transition with a condition,
            Transition transition = new Transition();
            transition.Data.Conditions.Add(new OptionalEndlessCondition());
            transition.Data.Conditions.Add(notOptional);
            transition.Configure(RuntimeConfigurator.Configuration.GetCurrentMode());

            transition.LifeCycle.Activate();

            while (transition.LifeCycle.Stage != Stage.Active)
            {
                yield return null;
                transition.Update();
            }

            // When the condition is skipped and the second condition is completed,
            transition.Configure(new Mode("Test", new WhitelistTypeRule<IOptional>().Add<OptionalEndlessCondition>()));

            notOptional.Autocomplete();

            yield return null;
            transition.Update();

            // Then the transition is completed.
            Assert.IsTrue(transition.IsCompleted);

            yield break;
        }

        [UnityTest]
        public IEnumerator MultiConditionTransitionFinishes()
        {
            // Given a transition with two conditions,
            EndlessCondition condition1 = new EndlessCondition();
            EndlessCondition condition2 = new EndlessCondition();
            Transition transition = new Transition();
            transition.Data.Conditions.Add(condition1);
            transition.Data.Conditions.Add(condition2);
            transition.Configure(RuntimeConfigurator.Configuration.GetCurrentMode());

            // After it is activated and the conditions are completed,
            transition.LifeCycle.Activate();

            yield return null;
            transition.Update();

            condition1.Autocomplete();

            Assert.IsTrue(condition1.IsCompleted);
            Assert.IsFalse(condition2.IsCompleted);
            Assert.IsFalse(transition.IsCompleted);

            condition2.Autocomplete();

            Assert.IsTrue(condition1.IsCompleted);
            Assert.IsTrue(condition2.IsCompleted);
            Assert.IsFalse(transition.IsCompleted);

            while (transition.IsCompleted == false)
            {
                yield return null;
                transition.Update();
            }

            // Then and only then the transition is completed.
            Assert.IsTrue(transition.IsCompleted);
        }
    }
}

#endif
