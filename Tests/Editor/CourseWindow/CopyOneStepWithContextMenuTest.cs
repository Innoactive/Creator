using System.Collections.Generic;
using Innoactive.Creator.Core;
using Innoactive.CreatorEditor.UI.Windows;
using NUnit.Framework;

namespace Innoactive.CreatorEditor.Tests.CourseWindowTests
{
    internal class CopyOneStepWithContextMenuTest : BaseCourseWindowTest
    {
        public override string WhenDescription => "Add one step to the workflow. Copy it with RMB -> Copy Step, RMB -> Paste step.";

        public override string ThenDescription => "Then there are two identical steps.";

        protected override void Then(CourseWindow window)
        {
            IList<IStep> steps = window.GetCourse().Data.FirstChapter.Data.Steps;
            Assert.AreEqual(2, steps.Count);
            Assert.AreEqual("Copy of " + steps[0].Data.Name, steps[1].Data.Name);
            Assert.True(steps[0].Data.Behaviors.Data.Behaviors.Count == 0);
            Assert.True(steps[1].Data.Behaviors.Data.Behaviors.Count == 0);
            Assert.True(steps[0].Data.Transitions.Data.Transitions.Count == 1);
            Assert.True(steps[1].Data.Transitions.Data.Transitions.Count == 1);
            Assert.Null(steps[0].Data.Transitions.Data.Transitions[0].Data.TargetStep);
            Assert.Null(steps[1].Data.Transitions.Data.Transitions[0].Data.TargetStep);
        }
    }
}