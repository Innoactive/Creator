using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using Innoactive.Hub.Training.Editors.Utils;
using Innoactive.Hub.Training.Utils;
using Innoactive.Hub.Unity.Tests.Training.Editor.EditorImguiTester.Windows;
using LogManager = Innoactive.Hub.Logging.LogManager;

namespace Innoactive.Hub.Unity.Tests.Training.Editor.EditorImguiTester
{
    /// <summary>
    /// Inherit from this class to implement your own editor IMGUI tests.
    /// </summary>
    public abstract class EditorImguiTest<T> : IEditorImguiTest where T : EditorWindow
    {
        private static JsonSerializerSettings JsonSerializerSettings
        {
            get
            {
                return new JsonSerializerSettings {Converters = new List<JsonConverter> {new ImguiEventConverter()}};
            }
        }

        /// <inheritdoc />
        public event EventHandler<EditorImguiTestFinishedEventArgs> Finished;

        /// <inheritdoc />
        public abstract string GivenDescription { get; }

        /// <inheritdoc />
        public abstract string WhenDescription { get; }

        /// <inheritdoc />
        public abstract string ThenDescription { get; }

        /// <summary>
        /// <inheritdoc />
        /// With default implementation, the record file is named by a full name of its type and located in <see cref="AssetFolderForRecordedActions"/>.
        /// </summary>
        public virtual string PathToRecordedActions
        {
            get
            {
                if (AssetFolderForRecordedActions.LastOrDefault() == '/')
                {
                    return AssetFolderForRecordedActions + GetType().FullName + ".json";
                }

                return AssetFolderForRecordedActions + '/' + GetType().FullName + ".json";
            }
        }

        /// <summary>
        /// Asset folder where the user actions record is saved.
        /// It is used in the default implementation of <see cref="PathToRecordedActions"/>.
        /// It should start with assets and forward slashes ("/") have to be used as path separators.
        /// </summary>
        protected abstract string AssetFolderForRecordedActions { get; }

        private EditorWindow result;

        /// Used internally by the Editor IMGUI tester tool.
        /// Override <see cref="Given"/> instead.
        public EditorWindow BaseGiven()
        {
            return Given();
        }

        /// <inheritdoc />
        /// Override <see cref="Then"/> instead.
        public void BaseThen(EditorWindow window)
        {
            Then((T)window);
        }

        /// <summary>
        /// Return editor window ready for recording/playing testing sequence.
        /// Editor window has to be shown with ShowUtility method.
        /// </summary>
        protected abstract T Given();

        /// <summary>
        /// Assert there.
        /// </summary>
        protected abstract void Then(T window);

        /// <summary>
        /// When you implement a non-abstract, non-generic class which inherits from <see cref="EditorImguiTest{T}"/>,
        /// this method will be automatically detected by the test runner. You don't need to do anything.
        /// </summary>
#if INNOACTIVE_IGNORE_EDITOR_IMGUI_TESTS
        [Ignore("Editor IMGUI tests are disabled, skipping it.")]
#endif
        [UnityTest]
        public IEnumerator Test()
        {
            TestState state = TestState.Pending;

            try
            {
                result = BaseGiven();

                BaseWhen(result, (sender, args) =>
                {
                    try
                    {
                        BaseThen(args.Result);
                        state = TestState.Passed;
                    }
                    catch (Exception e)
                    {
                        Debug.LogErrorFormat("Test {0} failed: {1}", GetType().GetNameWithNesting(), e.Message);
                        state = TestState.Failed;
                    }
                });
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("Test {0} failed: {1}", GetType().GetNameWithNesting(), e.Message);
                state = TestState.Failed;
            }

            while (state == TestState.Pending)
            {
                yield return null;
            }

            if (Finished != null)
            {
                Finished(this, new EditorImguiTestFinishedEventArgs(state));
            }
        }

        /// <summary>
        /// As <see cref="Test"/>, this method will be automatically located.
        /// If you need additional teardown logic, implement <see cref="AdditionalTeardown"/>.
        /// </summary>
        [TearDown]
        public void Teardown()
        {
            if (EditorUtils.IsWindowOpened<EditorWindowTestPlayer>())
            {
                foreach (EditorWindowTestPlayer window in Resources.FindObjectsOfTypeAll<EditorWindowTestPlayer>())
                {
                    window.Close();
                }
            }

            if (result != null)
            {
                result.Close();
            }

            if (TestableEditorElements.Mode == TestableEditorElements.DisplayMode.Playback)
            {
                TestableEditorElements.StopPlayback();
            }

            AdditionalTeardown();
        }

        /// <summary>
        /// Implement your additional teardown logic here.
        /// </summary>
        protected virtual void AdditionalTeardown()
        {
        }

        /// <summary>
        /// Apply recorded user actions to the given <paramref name="window"/>.
        /// </summary>
        private void BaseWhen(EditorWindow window, EditorWindowTestPlayer.FinishedHandler onFinishedCallback)
        {
            TextAsset recordedActionsAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(PathToRecordedActions);
            if (recordedActionsAsset == null)
            {
                throw new UserActionsRecordNotFoundException("User actions at path {0} are not found.", PathToRecordedActions);
            }

            List<UserAction> userActions = JsonConvert.DeserializeObject<List<UserAction>>(recordedActionsAsset.text, JsonSerializerSettings);
            EditorWindowTestPlayer.StartPlayback(window, userActions, onFinishedCallback);
        }
    }
}
