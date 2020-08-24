using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Innoactive.CreatorEditor.XRUtils
{
    /// <summary>
    /// Utility class that allows to load XR packages.
    /// </summary>
    internal static class XRLoaderHelper
    {
        private const string OpenVRXRReleaseURL = "https://api.github.com/repos/ValveSoftware/unity-xr-plugin/releases/latest";
        internal const string OculusDefineSymbol = "CREATOR_OCULUS";
        internal const string WindowsMRDefineSymbol = "CREATOR_WINDOWS_MR";
        internal const string OpenVRDefineSymbol = "CREATOR_OPEN_VR";
        internal const string XRManagementDefineSymbol = "CREATOR_XR_MANAGEMENT";

        public enum XRSDK
        {
            None,
            OpenVR,
            Oculus,
            WindowsMR
        }

        /// <summary>
        /// Retrieves and loads the OpenVR XR package.
        /// </summary>
        public static void LoadOpenVR()
        {
#if UNITY_2020_1_OR_NEWER && !CREATOR_XR_MANAGEMENT
            EditorPrefs.SetInt(nameof(XRSDK), (int)XRSDK.OpenVR);
            AddScriptingDefineSymbol(XRManagementDefineSymbol);
#elif UNITY_2020_1_OR_NEWER && CREATOR_XR_MANAGEMENT
            ListLatestOpenVRRelease();
#elif UNITY_2019_1_OR_NEWER && !UNITY_2020_1_OR_NEWER
            LoadOpenVRLegacy();
#endif
        }

        /// <summary>
        /// Retrieves and loads the Oculus XR package.
        /// </summary>
        public static void LoadOculus()
        {
#if CREATOR_XR_MANAGEMENT
            AddScriptingDefineSymbol(OculusDefineSymbol, new [] {BuildTarget.StandaloneWindows, BuildTarget.Android});
#else
            EditorPrefs.SetInt(nameof(XRSDK), (int)XRSDK.Oculus);
            AddScriptingDefineSymbol(XRManagementDefineSymbol);
#endif
        }

        /// <summary>
        /// Retrieves and loads the Windows MR package.
        /// </summary>
        public static void LoadWindowsMR()
        {
#if CREATOR_XR_MANAGEMENT
            AddScriptingDefineSymbol(WindowsMRDefineSymbol);
#else
            EditorPrefs.SetInt(nameof(XRSDK), (int)XRSDK.WindowsMR);
            AddScriptingDefineSymbol(XRManagementDefineSymbol);
#endif
        }

#if UNITY_2019_1_OR_NEWER && !UNITY_2020_1_OR_NEWER
        private async static void LoadOpenVRLegacy ()
        {
#pragma warning disable CS0618
            PlayerSettings.virtualRealitySupported = true;
            UnityEngine.XR.XRSettings.LoadDeviceByName("OpenVR");
            await System.Threading.Tasks.Task.Delay(1000);
            UnityEngine.XR.XRSettings.enabled = true;
#pragma warning restore CS0618
        }
#endif

        private static void AddScriptingDefineSymbol(string scriptingDefineSymbol)
        {
            BuildTarget buildTarget = BuildTarget.StandaloneWindows;
            BuildTargetGroup buildTargetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);
            List<string> symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup).Split(';').ToList();

            if (symbols.Contains(scriptingDefineSymbol) == false)
            {
                symbols.Add(scriptingDefineSymbol);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, string.Join(";", symbols.ToArray()));
            }
        }

        private static void AddScriptingDefineSymbol(string scriptingDefineSymbol, IEnumerable<BuildTarget> buildTargets)
        {
            foreach (BuildTarget buildTarget in buildTargets)
            {
                BuildTargetGroup buildTargetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);
                List<string> symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup).Split(';').ToList();

                if (symbols.Contains(scriptingDefineSymbol) == false)
                {
                    symbols.Add(scriptingDefineSymbol);
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, string.Join(";", symbols.ToArray()));
                }
            }
        }

        private static void ListLatestOpenVRRelease()
        {
            UnityWebRequest lastReleaseRequest = UnityWebRequest.Get(OpenVRXRReleaseURL);
            lastReleaseRequest.SendWebRequest().completed += asyncOperation =>
            {
                if (lastReleaseRequest.isNetworkError)
                {
                    Debug.LogErrorFormat("OpenVR could not be retrieved:\n{0}", lastReleaseRequest.error);
                }
                else
                {
                    string downloadHandlerText = lastReleaseRequest.downloadHandler.text;
                    dynamic data = JsonConvert.DeserializeObject<dynamic>(downloadHandlerText);
                    dynamic archive = data.assets[0];
                    string packageName = archive.name;
                    string packageURL = archive.browser_download_url;

                    RetrieveAndSaveOpenVR(packageName, packageURL);
                    lastReleaseRequest.Dispose();
                }
            };
        }

        private static void RetrieveAndSaveOpenVR(string packageName, string packageURL)
        {
            UnityWebRequest releaseDownloadRequest = UnityWebRequest.Get(packageURL);
            releaseDownloadRequest.SendWebRequest().completed += asyncOperation =>
            {
                if (releaseDownloadRequest.isNetworkError)
                {
                    Debug.LogErrorFormat("OpenVR could not be downloaded:\n{0}", releaseDownloadRequest.error);
                }
                else
                {
                    string packagePath = Path.Combine(Application.persistentDataPath, packageName);
                    byte[] downloadHandlerData = releaseDownloadRequest.downloadHandler.data;

                    File.WriteAllBytes(packagePath, downloadHandlerData);
                    ImportOpenVR(packagePath);
                    releaseDownloadRequest.Dispose();
                }
            };
        }

        private static void ImportOpenVR(string packagePath)
        {
            AssetDatabase.ImportPackage(packagePath, false);
            AddScriptingDefineSymbol(OpenVRDefineSymbol);
        }
    }
}
