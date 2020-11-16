using Innoactive.CreatorEditor.UI.Windows;
using NUnit.Framework;

namespace Innoactive.Creator.Core.Tests.Editor.StepWindowTests
{
    internal class RenameTest : BaseStepWindowTest
    {
        public override string WhenDescription => "Rename step to \"New Step!\" (without quotes).";
        public override string ThenDescription => "The step's name is \"New Step!\".";
        protected override void Then(StepWindow window)
        {
            IStep step = window.GetStep();
            Assert.AreEqual("New Step!", step.Data.Name);
        }
    }
}