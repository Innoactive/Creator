#if UNITY_XR_MANAGEMENT && (OCULUS_XR || WINDOWS_XR)
using System;
using UnityEditor;
using VPG.Editor.PackageManager;
using UnityEngine;

namespace VPG.Editor.XRUtils
{
    internal abstract class XRProvider : Dependency, IDisposable
    {
        protected virtual string XRLoaderName { get; } = "";

        protected XRProvider()
        {
            if (EditorPrefs.GetBool(XRLoaderHelper.IsXRLoaderInitialized) == false)
            {
                OnPackageEnabled += InitializeXRLoader;
            }
        }

        public void Dispose()
        {
            OnPackageEnabled -= InitializeXRLoader;
        }

        protected virtual async void InitializeXRLoader(object sender, EventArgs e)
        {
            OnPackageEnabled -= InitializeXRLoader;
            bool wasLoaderEnabled = await XRLoaderHelper.TryToEnableLoader(XRLoaderName);

            if (wasLoaderEnabled == false)
            {
                Debug.LogWarning($"{XRLoaderName} could not be loaded. Enable it manually here:\nEdit > Project Settings... > XR Plug-in Management.");
            }
        }
    }
}
#endif
