// using System;
// using Innoactive.CreatorEditor.ImguiTester;
// using UnityEditor;
// using UnityEngine;
//
// namespace Innoactive.CreatorEditor.UI.Windows
// {
//     [InitializeOnLoad]
//     internal static class AssemblyUnloadDetector
//     {
//         static AssemblyUnloadDetector()
//         {
//             EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
//             AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;
//         }
//
//         private static void OnBeforeAssemblyReload()
//         {
//             PreserveTrainingState();
//         }
//
//         private static void OnExitingMode()
//         {
//             PreserveTrainingState();
//         }
//
//         private static void OnPlayModeStateChanged(PlayModeStateChange state)
//         {
//             switch (state)
//             {
//                 case PlayModeStateChange.EnteredEditMode:
//                     break;
//                 case PlayModeStateChange.EnteredPlayMode:
//                     break;
//                 case PlayModeStateChange.ExitingEditMode:
//                     OnExitingMode();
//                     break;
//                 case PlayModeStateChange.ExitingPlayMode:
//                     OnExitingMode();
//                     break;
//                 default:
//                     throw new ArgumentOutOfRangeException("state", state, null);
//             }
//         }
//
//         private static void PreserveTrainingState()
//         {
//             try
//             {
//                 if (CourseWindow.IsOpen == false)
//                 {
//                     return;
//                 }
//
//                 CourseWindow.GetWindow().MakeTemporarySave();
//             }
//             catch (Exception e)
//             {
//                 Debug.LogError(e);
//
//                 if (EditorApplication.isPlaying == false && EditorApplication.isPlayingOrWillChangePlaymode)
//                 {
//                     EditorApplication.isPlaying = false;
//
//                     TestableEditorElements.DisplayDialog("Error while serializing the training!", e.ToString(), "Close");
//                 }
//             }
//         }
//     }
// }
