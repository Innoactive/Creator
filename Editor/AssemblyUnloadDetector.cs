using System;
using UnityEditor;

namespace Innoactive.CreatorEditor
{
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
            Editors.ProjectIsGoingToUnload();
        }

        private static void OnExitingMode()
        {
            Editors.ProjectIsGoingToUnload();
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
    }
}
