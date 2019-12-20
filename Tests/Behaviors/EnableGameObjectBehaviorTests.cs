

using UnityEngine;
#if UNITY_EDITOR
using System.Collections;
using Innoactive.Hub.Training;
using Innoactive.Hub.Training.Behaviors;
using Innoactive.Hub.Training.Configuration;
using Innoactive.Hub.Training.SceneObjects;
using Innoactive.Hub.Training.Utils.Builders;
using Innoactive.Hub.Unity.Tests.Training.Utils;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Innoactive.Hub.Unity.Tests.Training.Behaviors
{
    public class EnableGameObjectBehaviorTests : RuntimeTests
    {
        [UnityTest]
        public IEnumerator GameObjectIsEnabledAfterActivation()
        {
            // Given an active training scene object and a training with enable game object behavior,
            TrainingSceneObject toEnable = TestingUtils.CreateSceneObject("toEnable");
            toEnable.GameObject.SetActive(false);

            EndlessCondition trigger = new EndlessCondition();

            ICourse course = new LinearTrainingBuilder("Training")
                .AddChapter(new LinearChapterBuilder("Chapter")
                    .AddStep(new BasicStepBuilder("Step")
                        .DisableAutomaticAudioHandling()
                        .Enable(toEnable)
                        .AddCondition(trigger)))
                .Build();

            course.Configure(RuntimeConfigurator.Configuration.GetCurrentMode());

            TrainingRunner.Initialize(course);
            TrainingRunner.Run();

            // When the behavior is activated
            TrainingRunner.Initialize(course);
            TrainingRunner.Run();

            yield return new WaitUntil(()=> course.Data.FirstChapter.Data.Steps[0].LifeCycle.Stage == Stage.Active);

            // Then the training scene object is enabled.
            Assert.True(toEnable.GameObject.activeSelf);

            // Cleanup
            TestingUtils.DestroySceneObject(toEnable);

            yield break;
        }

        [UnityTest]
        public IEnumerator GameObjectStaysEnabled()
        {
            // Given an active training scene object and a training with enalbe game object condition,
            TrainingSceneObject toEnable = TestingUtils.CreateSceneObject("toEnable");
            toEnable.GameObject.SetActive(false);

            EndlessCondition trigger = new EndlessCondition();

            ICourse course = new LinearTrainingBuilder("Training")
                .AddChapter(new LinearChapterBuilder("Chapter")
                    .AddStep(new BasicStepBuilder("Step")
                        .DisableAutomaticAudioHandling()
                        .Enable(toEnable))
                    .AddStep(new BasicStepBuilder("Step")
                        .DisableAutomaticAudioHandling()
                        .AddCondition(trigger)))
                .Build();

            course.Configure(RuntimeConfigurator.Configuration.GetCurrentMode());

            // When the behavior is activated and after the step is completed
            TrainingRunner.Initialize(course);
            TrainingRunner.Run();

            yield return new WaitUntil(()=> course.Data.FirstChapter.Data.Steps[0].LifeCycle.Stage == Stage.Active);

            trigger.Autocomplete();

            yield return new WaitUntil(()=> course.Data.FirstChapter.Data.Steps[0].LifeCycle.Stage == Stage.Inactive);

            // Then the training scene object stays enabled.
            Assert.True(toEnable.GameObject.activeSelf);

            // Cleanup
            TestingUtils.DestroySceneObject(toEnable);
        }

        [UnityTest]
        public IEnumerator FastForwardInactiveBehavior()
        {
            // Given an inactive training scene object and an EnableGameObjectBehavior,
            TrainingSceneObject toEnable = TestingUtils.CreateSceneObject("ToEnable");
            toEnable.GameObject.SetActive(false);

            EnableGameObjectBehavior behavior = new EnableGameObjectBehavior(toEnable);

            // When we mark it to fast-forward,
            behavior.LifeCycle.MarkToFastForward();

            // Then it doesn't autocomplete because it weren't activated yet.
            Assert.AreEqual(Stage.Inactive, behavior.LifeCycle.Stage);

            // Cleanup.
            TestingUtils.DestroySceneObject(toEnable);

            yield break;
        }

        [UnityTest]
        public IEnumerator FastForwardInactiveBehaviorAndActivateIt()
        {
            // Given an inactive training scene object and a EnableGameObjectBehavior,
            TrainingSceneObject toEnable = TestingUtils.CreateSceneObject("ToEnable");
            toEnable.GameObject.SetActive(false);

            EnableGameObjectBehavior behavior = new EnableGameObjectBehavior(toEnable);

            // When we mark it to fast-forward and activate it,
            behavior.LifeCycle.MarkToFastForward();
            behavior.LifeCycle.Activate();

            // Then it should work without any differences because the behavior is done immediately anyways.
            Assert.AreEqual(Stage.Active, behavior.LifeCycle.Stage);
            Assert.IsTrue(toEnable.GameObject.activeSelf);

            // Cleanup.
            TestingUtils.DestroySceneObject(toEnable);

            yield break;
        }

        [UnityTest]
        public IEnumerator FastForwardActivatingBehavior()
        {
            // Given an inactive training scene object and an active EnableGameObjectBehavior,
            TrainingSceneObject toEnable = TestingUtils.CreateSceneObject("ToEnable");
            toEnable.GameObject.SetActive(false);

            EnableGameObjectBehavior behavior = new EnableGameObjectBehavior(toEnable);

            behavior.LifeCycle.Activate();

            // When we mark it to fast-forward,
            behavior.LifeCycle.MarkToFastForward();

            // Then it should work without any differences because the behavior is done immediately anyways.
            Assert.AreEqual(Stage.Active, behavior.LifeCycle.Stage);
            Assert.IsTrue(toEnable.GameObject.activeSelf);

            // Cleanup.
            TestingUtils.DestroySceneObject(toEnable);

            yield break;
        }
    }
}

#endif
