using System;
using UnityEditor;
using UnityEngine;
using Innoactive.Hub.Unity.Tests.Training.Editor.EditorImguiTester;

namespace Innoactive.Hub.Training.Editors.Windows
{
    [InitializeOnLoad]
    public static class AssemblyUnloadDetector
    {
        static AssemblyUnloadDetector()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;
        }

        private static void OnBeforeAssemblyReload()
        {
            PreserveTrainingState();
        }

        private static void OnExitingMode()
        {
            PreserveTrainingState();
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.EnteredEditMode:
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    OnExitingMode();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    OnExitingMode();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("state", state, null);
            }
        }

        private static void PreserveTrainingState()
        {
            try
            {
                if (TrainingWindow.IsOpen == false)
                {
                    return;
                }

                TrainingWindow.GetWindow().MakeTemporarySave();
            }
            catch (Exception e)
            {
                Debug.LogError(e);

                if (EditorApplication.isPlaying == false && EditorApplication.isPlayingOrWillChangePlaymode)
                {
                    EditorApplication.isPlaying = false;

                    TestableEditorElements.DisplayDialog("Error while serializing the training!", e.ToString(), "Close");
                }
            }
        }
    }
}
