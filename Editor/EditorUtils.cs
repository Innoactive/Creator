﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Innoactive.CreatorEditor
{
    /// <summary>
    /// Utility helper to ease up working with Unity Editor.
    /// </summary>
    [InitializeOnLoad]
    internal static class EditorUtils
    {
        private const string ignoreEditorImguiTestsDefineSymbol = "INNOACTIVE_IGNORE_EDITOR_IMGUI_TESTS";

        private static string coreFolder;

        private static MethodInfo repaintImmediately = typeof(EditorWindow).GetMethod("RepaintImmediately", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { }, new ParameterModifier[] { });

        static EditorUtils()
        {
            AssemblyReloadEvents.afterAssemblyReload += ResolveCoreFolder;
            EditorApplication.playModeStateChanged += ResolveCoreFolder;
        }

        [PublicAPI]
        private static void EnableEditorImguiTests()
        {
            SetImguiTestsState(true);
        }

        [PublicAPI]
        private static void DisableImguiTests()
        {
            SetImguiTestsState(false);
        }

        private static void SetImguiTestsState(bool enabled)
        {
            List<string> symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone).Split(';').ToList();

            bool wasEnabled = symbols.Contains(ignoreEditorImguiTestsDefineSymbol) == false;

            if (wasEnabled != enabled)
            {
                if (enabled)
                {
                    symbols.Remove(ignoreEditorImguiTestsDefineSymbol);
                }
                else
                {
                    symbols.Add(ignoreEditorImguiTestsDefineSymbol);
                }

                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, string.Join(";", symbols.ToArray()));
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
            }
        }

        /// <summary>
        /// Returns true if there is a window of type <typeparamref name="T"/> opened.
        /// </summary>
        internal static bool IsWindowOpened<T>() where T : EditorWindow
        {
            // https://answers.unity.com/questions/523839/find-out-if-an-editor-window-is-open.html
            T[] windows = Resources.FindObjectsOfTypeAll<T>();
            return windows != null && windows.Length > 0;
        }

        /// <summary>
        /// Causes the target <paramref name="window"/> to repaint immediately. Used for testing.
        /// </summary>
        internal static void RepaintImmediately(this EditorWindow window)
        {
            repaintImmediately.Invoke(window, new object[] { });
        }

        /// <summary>
        /// Takes the focus away the field where you was typing something into.
        /// </summary>
        internal static void ResetKeyboardElementFocus()
        {
            GUIUtility.keyboardControl = 0;
        }

        /// <summary>
        /// Gets the root folder of the training module.
        /// </summary>
        internal static string GetCoreFolder()
        {
            if (coreFolder == null)
            {
                ResolveCoreFolder();
            }

            return coreFolder;
        }

        /// <summary>
        /// Returns the Creator Core version as string.
        /// </summary>
        internal static string GetCoreVersion()
        {
            string versionFilePath = Path.Combine(GetCoreFolder(), "version.txt");
            if (File.Exists(versionFilePath))
            {
                return File.ReadAllText(versionFilePath);
            }

            return "unknown";
        }

        /// <summary>
        /// Gets .NET API compatibility level for current BuildTargetGroup.
        /// </summary>
        internal static ApiCompatibilityLevel GetCurrentCompatibilityLevel()
        {
            BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;
            BuildTargetGroup buildTargetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);
            return PlayerSettings.GetApiCompatibilityLevel(buildTargetGroup);
        }

        /// <summary>
        /// Returns a list of scriptable objects from provided type;
        /// </summary>
        internal static IEnumerable<T> GetAllScriptableObjects<T>() where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
            return guids.Select(AssetDatabase.GUIDToAssetPath).Select(AssetDatabase.LoadAssetAtPath<T>);
        }

        private static void ResolveCoreFolder(PlayModeStateChange state)
        {
            ResolveCoreFolder();
        }

        [DidReloadScripts]
        private static void ResolveCoreFolder()
        {
            string[] roots = Directory.GetFiles(Application.dataPath, $"{nameof(EditorUtils)}.cs", SearchOption.AllDirectories);

            if (roots.Length == 0)
            {
                throw new FileNotFoundException("Innoactive Creator Core folder not found!");
            }

            coreFolder = Path.GetDirectoryName(roots.First());
            coreFolder = coreFolder.Substring(Application.dataPath.Length);
            coreFolder = coreFolder.Substring(0, coreFolder.LastIndexOf(Path.DirectorySeparatorChar));
            // Assets folder was removed on previous step, put it back.
            coreFolder = "Assets" + coreFolder;
            // Replace backslashes with forward slashes.
            coreFolder = coreFolder.Replace('/', Path.AltDirectorySeparatorChar);
        }
    }
}
