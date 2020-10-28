using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Innoactive.CreatorEditor.PackageManager;

#if UNITY_XR_MANAGEMENT
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine.XR.Management;
using UnityEditor.XR.Management.Metadata;
#endif

namespace Innoactive.CreatorEditor.XRUtils
{
    /// <summary>
    /// Utility class that allows to load XR packages.
    /// </summary>
    internal class XRLoaderHelper
    {
        internal const string IsXRLoaderInitialized = "IsXRLoaderInitialized";
        private const string OculusXRPackage = "com.unity.xr.oculus";
        private const string WindowsXRPackage = "com.unity.xr.windowsmr";
        private const string XRManagementPackage = "com.unity.xr.management";

        public enum XRSDK
        {
            None,
            OpenVR,
            Oculus,
            WindowsMR
        }

        public enum XRConfiguration
        {
            None,
            XRManagement,
            OpenVRLegacy,
            OculusLegacy,
            OpenVRXR,
            OculusXR,
            WindowsMR,
            XRLegacy
        }

        /// <summary>
        /// Retrieves and loads the OpenVR XR package.
        /// </summary>
        public static void LoadOpenVR()
        {
            if (GetCurrentXRConfiguration().Any(loader => loader == XRConfiguration.OpenVRXR || loader == XRConfiguration.OpenVRLegacy))
            {
                Debug.LogWarning("OpenVR is already loaded.");
                return;
            }

            EditorPrefs.DeleteKey(IsXRLoaderInitialized);

#if UNITY_2020_1_OR_NEWER
            // This will be integrated as soon as there is an OpenVR XR SDK compatible with the XR interaction framework.
#elif UNITY_2019_1_OR_NEWER
            if (EditorReflectionUtils.AssemblyExists("Unity.XR.Management") == false)
            {
                LoadOpenVRLegacy();
            }
            else
            {
                Debug.LogWarning("OpenVR could not be loaded. XR Plug-in Management must be disabled before start using OpenVR.");
            }
#endif
        }

        /// <summary>
        /// Retrieves and loads the Oculus XR package.
        /// </summary>
        public static void LoadOculus()
        {
            if (GetCurrentXRConfiguration().Any(loader => loader == XRConfiguration.OculusXR))
            {
                Debug.LogWarning("Oculus XR is already loaded.");
                return;
            }

            EditorPrefs.DeleteKey(IsXRLoaderInitialized);

#if UNITY_XR_MANAGEMENT
            DisplayDialog("Oculus XR");
            PackageOperationsManager.LoadPackage(OculusXRPackage);
#else
            DisplayDialog("XR Plug-in Management");
            EditorPrefs.SetInt(nameof(XRSDK), (int)XRSDK.Oculus);
            PackageOperationsManager.LoadPackage(XRManagementPackage);
#endif
        }

        /// <summary>
        /// Retrieves and loads the Windows MR package.
        /// </summary>
        public static void LoadWindowsMR()
        {
            if (GetCurrentXRConfiguration().Any(loader => loader == XRConfiguration.WindowsMR))
            {
                Debug.LogWarning("Windows MR is already loaded.");
                return;
            }

            EditorPrefs.DeleteKey(IsXRLoaderInitialized);

#if UNITY_XR_MANAGEMENT
            DisplayDialog("Windows MR");
            PackageOperationsManager.LoadPackage(WindowsXRPackage);
#else
            DisplayDialog("XR Plug-in Management");
            EditorPrefs.SetInt(nameof(XRSDK), (int)XRSDK.WindowsMR);
            PackageOperationsManager.LoadPackage(XRManagementPackage);
#endif
        }

        /// <summary>
        /// Returns a list with all XR SDKs enabled in this project.
        /// </summary>
        public static IEnumerable<XRConfiguration> GetCurrentXRConfiguration()
        {
            List<XRConfiguration> enabledSDKs = new List<XRConfiguration>();

            if (EditorReflectionUtils.AssemblyExists("Unity.XR.Management"))
            {
#if UNITY_XR_MANAGEMENT
                if (XRGeneralSettings.Instance != null)
                {
                    foreach (XRLoader loader in XRGeneralSettings.Instance.Manager.loaders)
                    {
                        if (loader.name == "Oculus Loader")
                        {
                            enabledSDKs.Add(XRConfiguration.OculusXR);
                        }

                        if (loader.name == "Windows MR Loader")
                        {
                            enabledSDKs.Add(XRConfiguration.WindowsMR);
                        }
                    }

                    enabledSDKs.Add(XRConfiguration.XRManagement);
                }
#endif
            }
#if UNITY_2019_1_OR_NEWER && !UNITY_2020_1_OR_NEWER
#pragma warning disable CS0618
            else if (PlayerSettings.virtualRealitySupported)
            {
                foreach (string device in UnityEngine.XR.XRSettings.supportedDevices)
                {
                    if (device == "OpenVR")
                    {
                        enabledSDKs.Add(XRConfiguration.OpenVRLegacy);
                    }

                    if (device == "Oculus")
                    {
                        enabledSDKs.Add(XRConfiguration.OculusLegacy);
                    }
                }
                enabledSDKs.Add(XRConfiguration.XRLegacy);
            }
#pragma warning restore CS0618
#endif

            if (enabledSDKs.Any() == false)
            {
                enabledSDKs.Add(XRConfiguration.None);
            }

            return enabledSDKs;
        }

#if UNITY_XR_MANAGEMENT
        internal static async void EnableLoader(string package, string loader, BuildTargetGroup buildTargetGroup = BuildTargetGroup.Standalone)
        {
            EditorPrefs.SetBool(IsXRLoaderInitialized, true);
            SettingsService.OpenProjectSettings("Project/XR Plug-in Management");

            await Task.Delay(500);

            if (XRGeneralSettings.Instance == null)
            {
                EditorUtility.DisplayDialog($"Can't enable {loader}!", $"Can't enable {loader} because general settings for XR are missing. Enable them manually here:\nEdit > Project Settings... > XR Plug-in Management\nand then select the provider for your VR headset.", "Continue");
                return;
            }

            if (XRGeneralSettings.Instance.Manager.loaders.Any(xrLoader => xrLoader.GetType().Name == loader))
            {
                return;
            }

            typeof(XRPackageMetadataStore)
                .GetMethod("InstallPackageAndAssignLoaderForBuildTarget", BindingFlags.Static | BindingFlags.NonPublic)?
                .Invoke(null, new object[] { package, loader, buildTargetGroup });
        }
#endif

        private static void DisplayDialog(string loader)
        {
            EditorUtility.DisplayDialog($"Enabling {loader}", "Wait until the setup is done.", "Continue");
        }

#if UNITY_2019_1_OR_NEWER && !UNITY_2020_1_OR_NEWER
        private static async void LoadOpenVRLegacy()
        {
            DisplayDialog("OpenVR");
#pragma warning disable CS0618
            PlayerSettings.virtualRealitySupported = true;
            UnityEngine.XR.XRSettings.LoadDeviceByName("OpenVR");
            await System.Threading.Tasks.Task.Delay(1000);
            UnityEngine.XR.XRSettings.enabled = true;
#pragma warning restore CS0618
        }
#endif
    }
}
