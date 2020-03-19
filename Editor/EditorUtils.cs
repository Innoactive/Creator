using System.Collections.Generic;
using System.IO;
using System.Linq;
using Innoactive.Creator.Core.Exceptions;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Innoactive.CreatorEditor
{
    /// <summary>
    /// Utility helper to ease up working with Unity Editor.
    /// </summary>
    [InitializeOnLoad]
    public static class EditorUtils
    {
        private const string ignoreEditorImguiTestsDefineSymbol = "INNOACTIVE_IGNORE_EDITOR_IMGUI_TESTS";
        private const string rootFileName = "training-module-root.txt";
        private const string openVrPackageName = "com.unity.xr.openvr.standalone@1.0.5";

        private static string cachedRootFolder;
        private static ListRequest listRequest = null;

        static EditorUtils()
        {
            AddOpenVrPackage();
            AssemblyReloadEvents.afterAssemblyReload += ResolveModuleFolder;
            EditorApplication.playModeStateChanged += ResolveModuleFolder;
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

        [DidReloadScripts]
        private static void ResolveModuleFolder()
        {
            string[] roots = Directory.GetFiles(Application.dataPath, rootFileName, SearchOption.AllDirectories);

            if (roots.Length == 0)
            {
                throw new FileNotFoundException("Training module root folder is not found!");
            }

            if (roots.Length > 1)
            {
                throw new InvalidStateException(string.Format("Can't determine the root folder of the training module: make sure you have only one '{0}' file in your project.", rootFileName));
            }

            // Remove full path to assets folder and the file's name.
            cachedRootFolder = roots[0].Substring(Application.dataPath.Length, roots[0].Length - Application.dataPath.Length - rootFileName.Length - 1);
            // Assets folder was removed on previous step, put it back.
            cachedRootFolder = "Assets" + cachedRootFolder;
            // Replace backslashes with forward slashes.
            cachedRootFolder = cachedRootFolder.Replace('/', Path.PathSeparator);
        }

        private static void ResolveModuleFolder(PlayModeStateChange state)
        {
            ResolveModuleFolder();
        }

        /// <summary>
        /// Adds the OpenVR package dependency to the project.
        /// If it is already added, it does nothing.
        /// </summary>
        private static void AddOpenVrPackage()
        {
            EditorApplication.update += WaitForEditorApplicationUpdate;
        }

        private static void WaitForEditorApplicationUpdate()
        {
            if (listRequest == null)
            {
                listRequest = UnityEditor.PackageManager.Client.List();
            }

            if (listRequest.IsCompleted == false)
            {
                return;
            }
            else if (listRequest.Status == StatusCode.Failure)
            {
                // UB can't occur in this case.
                // ReSharper disable once DelegateSubtraction
                EditorApplication.update -= WaitForEditorApplicationUpdate;
                return;
            }

            PackageCollection listRequestResult = listRequest.Result;
            string[] packageData = openVrPackageName.Split('@');

            if (listRequestResult.Any(packageInfo => packageInfo.name == packageData[0] && packageInfo.version == packageData[1]) == false)
            {
                UnityEditor.PackageManager.Client.Add(openVrPackageName);
                Debug.LogFormat("The 'OpenVR' package was not a dependency of this Unity project. The package 'OpenVR ({0})' has been automatically added.", openVrPackageName);
            }

            // UB can't occur in this case.
            // ReSharper disable once DelegateSubtraction
            EditorApplication.update -= WaitForEditorApplicationUpdate;
        }

        /// <summary>
        /// Returns true if there is a window of type <typeparamref name="T"/> opened.
        /// </summary>
        public static bool IsWindowOpened<T>() where T : EditorWindow
        {
            // https://answers.unity.com/questions/523839/find-out-if-an-editor-window-is-open.html
            T[] windows = Resources.FindObjectsOfTypeAll<T>();
            return windows != null && windows.Length > 0;
        }

        /// <summary>
        /// Gets the root folder of the training module.
        /// </summary>
        public static string GetModuleFolder()
        {
            if (cachedRootFolder == null)
            {
                ResolveModuleFolder();
            }

            return cachedRootFolder;
        }
    }
}
