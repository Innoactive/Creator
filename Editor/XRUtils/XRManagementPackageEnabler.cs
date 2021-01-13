#if UNITY_XR_MANAGEMENT
using UnityEditor;
using System;
using Innoactive.CreatorEditor.PackageManager;

namespace Innoactive.CreatorEditor.XRUtils
{
    /// <summary>
    /// Enables the XR Plug-in Management.
    /// </summary>
    internal sealed class XRManagementPackageEnabler : Dependency, IDisposable
    {
        /// <inheritdoc/>
        public override string Package { get; } = "com.unity.xr.management";

        /// <inheritdoc/>
        public override int Priority { get; } = 1;

        public XRManagementPackageEnabler()
        {
            OnPackageEnabled += InitializeXRLoader;
        }

        public void Dispose()
        {
            OnPackageEnabled -= InitializeXRLoader;
        }

        private void InitializeXRLoader(object sender, EventArgs e)
        {
            XRLoaderHelper.XRSDK sdk = (XRLoaderHelper.XRSDK) EditorPrefs.GetInt(nameof(XRLoaderHelper.XRSDK));

            switch (sdk)
            {
                case XRLoaderHelper.XRSDK.OpenVR:
                    break;
                case XRLoaderHelper.XRSDK.Oculus:
                    XRLoaderHelper.LoadOculus();
                    break;
                case XRLoaderHelper.XRSDK.WindowsMR:
                    XRLoaderHelper.LoadWindowsMR();
                    break;
            }

            EditorPrefs.DeleteKey(nameof(XRLoaderHelper.XRSDK));
            OnPackageEnabled -= InitializeXRLoader;
        }
    }
}
#endif
