using System;
using UnityEditor;

namespace VPG.CreatorEditor
{
    /// <summary>
    /// Tracks when the project is going to be unloaded (when assemblies are unloaded, when user starts or stop runtime, when scripts were modified).
    /// </summary>
    [InitializeOnLoad]
    internal static class AssemblyUnloadDetector
    {
        static AssemblyUnloadDetector()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;
        }

        private static void OnBeforeAssemblyReload()
        {
            GlobalEditorHandler.ProjectIsGoingToUnload();
        }

        private static void OnExitingMode()
        {
            GlobalEditorHandler.ProjectIsGoingToUnload();
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.EnteredEditMode:
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    GlobalEditorHandler.EnterPlayMode();
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    OnExitingMode();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    GlobalEditorHandler.ExitPlayMode();
                    OnExitingMode();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("state", state, null);
            }
        }
    }
}
