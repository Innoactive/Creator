using VPG.Editor;
using VPG.Editor.TestTools;
using VPG.Editor.UI.Windows;
using UnityEditor;
using UnityEngine;

namespace VPG.Core.Tests.Editor.StepWindowTests
{
    internal abstract class BaseStepWindowTest : EditorImguiTest<StepWindow>
    {
        /// <inheritdoc />
        public override string GivenDescription => "A step inspector window with a new step with no behaviors and one transition to null with no conditions.";

        /// <inheritdoc />
        protected override string AssetFolderForRecordedActions => EditorUtils.GetCoreFolder() + "/Tests/Editor/StepWindow/Records";

        /// <inheritdoc />
        protected override StepWindow Given()
        {
            if (EditorUtils.IsWindowOpened<StepWindow>())
            {
                EditorWindow.GetWindow<StepWindow>().Close();
            }

            GlobalEditorHandler.SetStrategy(new EmptyTestStrategy());

            EditorUtils.ResetKeyboardElementFocus();
            StepWindow window = ScriptableObject.CreateInstance<StepWindow>();
            window.ShowUtility();
            window.position = new Rect(Vector2.zero, window.position.size);
            window.minSize = window.maxSize = new Vector2(512f, 512f);
            window.SetStep(StepFactory.Instance.Create("Test"));
            window.Focus();

            return window;
        }

        protected override void AdditionalTeardown()
        {
            if (EditorUtils.IsWindowOpened<StepWindow>())
            {
                EditorWindow.GetWindow<StepWindow>().Close();
            }

            base.AdditionalTeardown();
            GlobalEditorHandler.SetDefaultStrategy();
        }
    }
}
