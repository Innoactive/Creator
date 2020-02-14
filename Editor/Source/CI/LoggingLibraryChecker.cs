// using System;
// using System.IO;
// using UnityEngine;
// using UnityEditor;
// using System.Collections.Generic;
// using System.Linq;
// using System.Reflection;
//
// namespace Innoactive.Hub.Utils
// {
//     /// <summary>
//     /// This class ensures (at editor time) that a fitting Innoactive.Hub.Logging.Unity.dll is loaded.
//     /// </summary>
//     [InitializeOnLoad]
//     public static class LoggingLibraryChecker
//     {
// #if UNITY_2018_1_OR_NEWER
//         private const string Version = "2018.1";
// #elif UNITY_2017_4
//         private const string Version = "2017.4";
// #else
// #warning LoggingLibraryChecker found an unknown Unity version!
//         private const string Version = "error";
// #endif
//
//         static LoggingLibraryChecker()
//         {
//             CheckLoggingLibraries();
//         }
//
//         private static bool CheckLoggingLibraries()
//         {
//             bool hasChanged = false;
//
//             if (Version != "error")
//             {
//                 bool standaloneDLLsChanged = ConfigureDLLsForStandalone();
//
//                 bool disableAll = false;
// #if UNITY_5_6 || (!UNITY_WSA || !UNITY_ANDROID)
//                 disableAll = true;
// #endif
//                 bool portableDLLsChanged = ConfigureDLLsForPortable(disableAll);
//
//                 hasChanged = standaloneDLLsChanged || portableDLLsChanged;
//
//                 if (hasChanged)
//                 {
//                     ClearConsole();
//                 }
//
//                 if (standaloneDLLsChanged)
//                 {
//                     Debug.LogFormat("Switched to logging library v{0} [Standalone]", Version);
//                 }
//
//                 if (portableDLLsChanged)
//                 {
//                     Debug.LogFormat("Switched to logging library v{0} [Portable]", Version);
//                 }
//             }
//
//             return hasChanged;
//         }
//
//         private static bool ConfigureDLLsForStandalone(bool disableAll = false)
//         {
//             List<string> pathsToDLLs = GetLoggingDLLs();
//
//             bool hasSomethingChanged = false;
//
//             foreach (string path in pathsToDLLs)
//             {
//                 if (path.Contains(Version) && !disableAll)
//                 {
//                     if (SetCompatibilityForStandalone(path))
//                     {
//                         hasSomethingChanged = true;
//                     }
//                 }
//                 else
//                 {
//                     if (DisableDLL(path))
//                     {
//                         hasSomethingChanged = true;
//                     }
//                 }
//             }
//
//             return hasSomethingChanged;
//         }
//
//         private static bool ConfigureDLLsForPortable(bool disableAll = false)
//         {
//             List<string> pathsToDLLs = GetLoggingDLLs("Portable");
//
//             if (pathsToDLLs == null)
//             {
//                 // Just do nothing if Portable subfolder and therefore the DLLs do not exist.
//                 return false;
//             }
//
//             bool hasSomethingChanged = false;
//
//             foreach (string path in pathsToDLLs)
//             {
//                 if (path.Contains(Version) && !disableAll)
//                 {
//                     if (SetCompatibilityForPortable(path))
//                     {
//                         hasSomethingChanged = true;
//                     }
//                 }
//                 else
//                 {
//                     if (DisableDLL(path))
//                     {
//                         hasSomethingChanged = true;
//                     }
//                 }
//             }
//
//             return hasSomethingChanged;
//         }
//
//         private static bool SetCompatibilityForStandalone(string pathToDLL)
//         {
//             bool wasSet = false;
//
//             if (pathToDLL != null)
//             {
//                 // Since we are only supporting Windows right now it is enough to just add Windows support
//                 BuildTarget[] supportedBuildTargets = {
//                     BuildTarget.StandaloneWindows,
//                     BuildTarget.StandaloneWindows64
//                 };
//
//                 wasSet = SetDLLCompatibility(pathToDLL, supportedBuildTargets, true);
//             }
//
//             return wasSet;
//         }
//
//         private static bool SetCompatibilityForPortable(string pathToDLL)
//         {
//             bool wasSet = false;
//
//             if (pathToDLL != null)
//             {
//                 BuildTarget[] supportedBuildTargets =
//                 {
//                     BuildTarget.WSAPlayer,
//                     BuildTarget.Android
//                 };
//
//                 wasSet = SetDLLCompatibility(pathToDLL, supportedBuildTargets, false);
//             }
//
//             return wasSet;
//         }
//
//         private static bool DisableDLL(string pathToDLL)
//         {
//             if (HasCompatibility(pathToDLL, new BuildTarget[] {}, false))
//             {
//                 return false;
//             }
//
//             PluginImporter pluginImporter = (PluginImporter)AssetImporter.GetAtPath(pathToDLL);
//             pluginImporter.ClearSettings();
//             pluginImporter.SetCompatibleWithAnyPlatform(false);
//
//             pluginImporter.SaveAndReimport();
//
//             return true;
//         }
//
//         private static bool SetDLLCompatibility(string pathToDLL, BuildTarget[] supportedBuildTargets, bool isCompatibleWithEditor)
//         {
//             if (HasCompatibility(pathToDLL, supportedBuildTargets, isCompatibleWithEditor))
//             {
//                 return false;
//             }
//
//             PluginImporter pluginImporter = (PluginImporter)AssetImporter.GetAtPath(pathToDLL);
//
//             pluginImporter.SetCompatibleWithAnyPlatform(false);
//             pluginImporter.SetCompatibleWithEditor(isCompatibleWithEditor);
//             foreach (BuildTarget buildTarget in supportedBuildTargets)
//             {
//                 pluginImporter.SetCompatibleWithPlatform(buildTarget, true);
//             }
//
//             pluginImporter.SaveAndReimport();
//
//             return true;
//         }
//
//         private static bool HasCompatibility(string pathToDLL, BuildTarget[] supportedBuildTargets, bool isCompatibleWithEditor)
//         {
//             PluginImporter pluginImporter = (PluginImporter)AssetImporter.GetAtPath(pathToDLL);
//
//             // check if any platform is enabled
//             if (pluginImporter.GetCompatibleWithAnyPlatform())
//             {
//                 return false;
//             }
//
//             // check if compatible with editor if that is desired
//             if (pluginImporter.GetCompatibleWithEditor() != isCompatibleWithEditor)
//             {
//                 return false;
//             }
//
//             // check for each build target if compatible when desired
//             foreach (BuildTarget buildTarget in Enum.GetValues(typeof(BuildTarget)))
//             {
//
//                 if (IsBuildTargetObsolete(buildTarget) || buildTarget == BuildTarget.NoTarget)
//                 {
//                     continue;;
//                 }
//
//                 bool isCompatible = pluginImporter.GetCompatibleWithPlatform(buildTarget);
//
//                 if (supportedBuildTargets.Contains(buildTarget))
//                 {
//                     if (!isCompatible)
//                     {
//                         return false;
//                     }
//                 }
//                 else
//                 {
//                     if (isCompatible)
//                     {
//                         return false;
//                     }
//                 }
//             }
//             return true;
//         }
//
//         private static List<string> GetLoggingDLLs(string subdirectory = null)
//         {
//             string libraryDirectory = GetLibraryDirectory();
//
//             if (subdirectory != null)
//             {
//                 libraryDirectory = Path.Combine(libraryDirectory, subdirectory);
//
//                 if (!Directory.Exists(libraryDirectory))
//                 {
//                     return null;
//                 }
//             }
//
//             List<string> result = new List<string>();
//
//             foreach (string file in Directory.GetFiles(libraryDirectory, "*.dll"))
//             {
//                 if (Path.GetFileName(file).StartsWith("Innoactive.Hub.Logging.Unity"))
//                 {
//                     result.Add(file);
//                 }
//             }
//             return result;
//         }
//
//         private static string GetLibraryDirectory()
//         {
//             if (AssetDatabase.FindAssets("Innoactive.Hub.Logging.Unity_").Length > 0)
//             {
//                 return Path.GetDirectoryName(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("Innoactive.Hub.Logging.Unity_")[0]));
//             }
//             else if (AssetDatabase.FindAssets("Innoactive.Hub.Logging.dll").Length > 0)
//             {
//                 return Path.GetDirectoryName(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("Innoactive.Hub.Logging.dll")[0]));
//             }
//             else
//             {
//                 Debug.LogError("Could not aquire position of logging dlls, make sure your Innoactive.Hub.Logging.Unity dlls exist.");
//                 return null;
//             }
//         }
//
//         private static bool IsBuildTargetObsolete(BuildTarget buildTarget)
//         {
//             FieldInfo field = buildTarget.GetType().GetField(buildTarget.ToString());
//             ObsoleteAttribute[] attributes = (ObsoleteAttribute[]) field.GetCustomAttributes(typeof(ObsoleteAttribute), false);
//             return (attributes != null && attributes.Length > 0);
//         }
//
//         private static void ClearConsole()
//         {
//             Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
//             Type type = assembly.GetType("UnityEditor.LogEntries");
//             MethodInfo method = type.GetMethod("Clear");
//             method.Invoke(new object(), null);
//         }
//     }
// }
