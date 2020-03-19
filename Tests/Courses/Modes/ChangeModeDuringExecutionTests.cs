using System.Collections;
using System.Collections.Generic;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Audio;
using Innoactive.Creator.Core.Behaviors;
using Innoactive.Creator.Core.Conditions;
using Innoactive.Creator.Core.Configuration.Modes;
using Innoactive.Creator.Core.Internationalization;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Innoactive.Creator.Tests.Utils
{
    public class ChangeModeDuringExecutionTests : RuntimeTests
    {
        private static AudioSource audioSource;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            // Setup the player and its audio source in here.
            // AudioSource.playOnAwake is by default true. Thus audioSource.isPlaying is true during the first frame.
            // The first frame is skipped after setup and audioSource.isPlaying is false as desired.
            GameObject player = new GameObject("AudioPlayer");
            audioSource = player.AddComponent<AudioSource>();
        }

        [UnityTest]
        public IEnumerator ActivationBehavior()
        {
            // Given a linear three step course where each step has a PlayAudioBehavior (ExecutionStage: Activation) and an EndlessConditionMock,
            ResourceAudio audioData = new ResourceAudio(new LocalizedString("Sounds/test-sound", "Sounds/test-sound"));
            IBehavior behavior1 = new PlayAudioBehavior(audioData, BehaviorExecutionStages.Activation, true, audioSource);
            IBehavior behavior2 = new PlayAudioBehavior(audioData, BehaviorExecutionStages.Activation, true, audioSource);
            IBehavior behavior3 = new PlayAudioBehavior(audioData, BehaviorExecutionStages.Activation, true, audioSource);

            TestLinearChapterBuilder chapterBuilder = TestLinearChapterBuilder.SetupChapterBuilder(3, true);
            chapterBuilder.Steps[0].Data.Behaviors.Data.Behaviors = new List<IBehavior> { behavior1 };
            chapterBuilder.Steps[1].Data.Behaviors.Data.Behaviors = new List<IBehavior> { behavior2 };
            chapterBuilder.Steps[2].Data.Behaviors.Data.Behaviors = new List<IBehavior> { behavior3 };
            IChapter chapter = chapterBuilder.Build();

            ICourse course = new Course("course", chapter);

            // And given a "no audio" and a "with audio" mode.
            IMode noHints = new Mode("No Audio Hints", new WhitelistTypeRule<IOptional>().Add<PlayAudioBehavior>());
            IMode withHints = new Mode("With Audio Hints", new WhitelistTypeRule<IOptional>());

            // When running it and changing the mode during execution several times,
            // Then the corresponding PlayAudioBehavior of the current step is activated and deactivated accordingly.
            // The other PlayAudioBehaviors of the other steps stay inactive.
            TrainingRunner.Initialize(course);
            TrainingRunner.Run();
            course.Configure(withHints);

            yield return new WaitUntil(() => behavior1.LifeCycle.Stage == Stage.Activating);

            Assert.AreEqual(Stage.Activating, behavior1.LifeCycle.Stage);
            Assert.AreEqual(Stage.Inactive, behavior2.LifeCycle.Stage);
            Assert.AreEqual(Stage.Inactive, behavior3.LifeCycle.Stage);

            course.Configure(noHints);

            Assert.AreEqual(Stage.Inactive, behavior1.LifeCycle.Stage);
            Assert.AreEqual(Stage.Inactive, behavior2.LifeCycle.Stage);
            Assert.AreEqual(Stage.Inactive, behavior3.LifeCycle.Stage);

            ICondition condition1 = course.Data.FirstChapter.Data.FirstStep.Data.Transitions.Data.Transitions[0].Data.Conditions[0];
            yield return new WaitUntil(() => condition1.LifeCycle.Stage == Stage.Active);

            condition1.Autocomplete();

            ICondition condition2 = course.Data.FirstChapter.Data.Steps[1].Data.Transitions.Data.Transitions[0].Data.Conditions[0];
            yield return new WaitUntil(() => condition2.LifeCycle.Stage == Stage.Active);

            Assert.AreEqual(Stage.Inactive, behavior1.LifeCycle.Stage);
            Assert.AreEqual(Stage.Inactive, behavior2.LifeCycle.Stage);
            Assert.AreEqual(Stage.Inactive, behavior3.LifeCycle.Stage);

            course.Configure(withHints);

            Assert.AreEqual(Stage.Inactive, behavior1.LifeCycle.Stage);
            Assert.AreEqual(Stage.Activating, behavior2.LifeCycle.Stage);
            Assert.AreEqual(Stage.Inactive, behavior3.LifeCycle.Stage);

            condition2.Autocomplete();

            ICondition condition3 = course.Data.FirstChapter.Data.Steps[2].Data.Transitions.Data.Transitions[0].Data.Conditions[0];
            yield return new WaitUntil(() => condition3.LifeCycle.Stage == Stage.Active);

            Assert.AreEqual(Stage.Inactive, behavior1.LifeCycle.Stage);
            Assert.AreEqual(Stage.Inactive, behavior2.LifeCycle.Stage);
            Assert.AreEqual(Stage.Active, behavior3.LifeCycle.Stage);

            course.Configure(noHints);

            Assert.AreEqual(Stage.Inactive, behavior1.LifeCycle.Stage);
            Assert.AreEqual(Stage.Inactive, behavior2.LifeCycle.Stage);
            Assert.AreEqual(Stage.Inactive, behavior3.LifeCycle.Stage);

            course.Configure(withHints);

            Assert.AreEqual(Stage.Inactive, behavior1.LifeCycle.Stage);
            Assert.AreEqual(Stage.Inactive, behavior2.LifeCycle.Stage);
            Assert.AreEqual(Stage.Activating, behavior3.LifeCycle.Stage);

            condition3.Autocomplete();

            yield return new WaitUntil(() => condition3.LifeCycle.Stage == Stage.Inactive);

            Assert.AreEqual(Stage.Inactive, behavior1.LifeCycle.Stage);
            Assert.AreEqual(Stage.Inactive, behavior2.LifeCycle.Stage);
            Assert.AreEqual(Stage.Active, behavior3.LifeCycle.Stage);
        }

        [UnityTest]
        public IEnumerator DeactivationBehavior()
        {
            // Given a linear three step course where each step has a PlayAudioBehavior (ExecutionStage: Deactivation) and an EndlessConditionMock,
            ResourceAudio audioData = new ResourceAudio(new LocalizedString("Sounds/test-sound", "Sounds/test-sound"));
            IBehavior behavior1 = new PlayAudioBehavior(audioData, BehaviorExecutionStages.Deactivation, true, audioSource);
            IBehavior behavior2 = new PlayAudioBehavior(audioData, BehaviorExecutionStages.Deactivation, true, audioSource);
            IBehavior behavior3 = new PlayAudioBehavior(audioData, BehaviorExecutionStages.Deactivation, true, audioSource);

            TestLinearChapterBuilder chapterBuilder = TestLinearChapterBuilder.SetupChapterBuilder(3, true);
            chapterBuilder.Steps[0].Data.Behaviors.Data.Behaviors = new List<IBehavior> { behavior1 };
            chapterBuilder.Steps[1].Data.Behaviors.Data.Behaviors = new List<IBehavior> { behavior2 };
            chapterBuilder.Steps[2].Data.Behaviors.Data.Behaviors = new List<IBehavior> { behavior3 };
            IChapter chapter = chapterBuilder.Build();

            ICourse course = new Course("course", chapter);

            // And given a "no audio" and a "with audio" mode.
            IMode noHints = new Mode("No Audio Hints", new WhitelistTypeRule<IOptional>().Add<PlayAudioBehavior>());
            IMode withHints = new Mode("With Audio Hints", new WhitelistTypeRule<IOptional>());

            // When running it and changing the mode during execution several times,
            // Then the corresponding PlayAudioBehavior of the current step is activated and deactivated accordingly.
            // The other PlayAudioBehaviors of the other steps stay inactive.
            TrainingRunner.Initialize(course);
            TrainingRunner.Run();
            course.Configure(withHints);

            ICondition condition1 = course.Data.FirstChapter.Data.FirstStep.Data.Transitions.Data.Transitions[0].Data.Conditions[0];
            yield return new WaitUntil(() => condition1.LifeCycle.Stage == Stage.Active);

            Assert.AreEqual(Stage.Active, behavior1.LifeCycle.Stage);
            Assert.AreEqual(Stage.Inactive, behavior2.LifeCycle.Stage);
            Assert.AreEqual(Stage.Inactive, behavior3.LifeCycle.Stage);

            course.Configure(noHints);

            Assert.AreEqual(Stage.Inactive, behavior1.LifeCycle.Stage);
            Assert.AreEqual(Stage.Inactive, behavior2.LifeCycle.Stage);
            Assert.AreEqual(Stage.Inactive, behavior3.LifeCycle.Stage);

            condition1.Autocomplete();
            yield return null;

            Assert.AreEqual(Stage.Inactive, behavior1.LifeCycle.Stage);
            Assert.AreEqual(Stage.Inactive, behavior2.LifeCycle.Stage);
            Assert.AreEqual(Stage.Inactive, behavior3.LifeCycle.Stage);

            ICondition condition2 = course.Data.FirstChapter.Data.Steps[1].Data.Transitions.Data.Transitions[0].Data.Conditions[0];
            yield return new WaitUntil(() => condition2.LifeCycle.Stage == Stage.Active);

            Assert.AreEqual(Stage.Inactive, behavior1.LifeCycle.Stage);
            Assert.AreEqual(Stage.Inactive, behavior2.LifeCycle.Stage);
            Assert.AreEqual(Stage.Inactive, behavior3.LifeCycle.Stage);

            course.Configure(withHints);

            Assert.AreEqual(Stage.Inactive, behavior1.LifeCycle.Stage);
            Assert.AreEqual(Stage.Activating, behavior2.LifeCycle.Stage);
            Assert.AreEqual(Stage.Inactive, behavior3.LifeCycle.Stage);

            condition2.Autocomplete();
            yield return null;

            Assert.AreEqual(Stage.Inactive, behavior1.LifeCycle.Stage);
            Assert.AreEqual(Stage.Active, behavior2.LifeCycle.Stage);
            Assert.AreEqual(Stage.Inactive, behavior3.LifeCycle.Stage);

            ICondition condition3 = course.Data.FirstChapter.Data.Steps[2].Data.Transitions.Data.Transitions[0].Data.Conditions[0];
            yield return new WaitUntil(() => condition3.LifeCycle.Stage == Stage.Active);

            Assert.AreEqual(Stage.Inactive, behavior1.LifeCycle.Stage);
            Assert.AreEqual(Stage.Inactive, behavior2.LifeCycle.Stage);
            Assert.AreEqual(Stage.Active, behavior3.LifeCycle.Stage);

            course.Configure(noHints);

            Assert.AreEqual(Stage.Inactive, behavior1.LifeCycle.Stage);
            Assert.AreEqual(Stage.Inactive, behavior2.LifeCycle.Stage);
            Assert.AreEqual(Stage.Inactive, behavior3.LifeCycle.Stage);

            course.Configure(withHints);

            Assert.AreEqual(Stage.Inactive, behavior1.LifeCycle.Stage);
            Assert.AreEqual(Stage.Inactive, behavior2.LifeCycle.Stage);
            Assert.AreEqual(Stage.Activating, behavior3.LifeCycle.Stage);

            condition3.Autocomplete();
            yield return null;

            Assert.AreEqual(Stage.Inactive, behavior1.LifeCycle.Stage);
            Assert.AreEqual(Stage.Inactive, behavior2.LifeCycle.Stage);
            Assert.AreEqual(Stage.Active, behavior3.LifeCycle.Stage);

            course.Configure(noHints);

            Assert.AreEqual(Stage.Inactive, behavior1.LifeCycle.Stage);
            Assert.AreEqual(Stage.Inactive, behavior2.LifeCycle.Stage);
            Assert.AreEqual(Stage.Inactive, behavior3.LifeCycle.Stage);
        }
    }
}
