#if UNITY_XR_MANAGEMENT && (OCULUS_XR || WINDOWS_XR)
using System;
using Innoactive.CreatorEditor.PackageManager;

namespace Innoactive.CreatorEditor.XRUtils
{
    internal abstract class XRProvider : Dependency, IDisposable
    {
        protected virtual string XRLoaderName { get; } = "";

        public XRProvider()
        {
            OnPackageEnabled += InitializeXRLoader;
        }

        public void Dispose()
        {
            OnPackageEnabled -= InitializeXRLoader;
        }

        protected virtual void InitializeXRLoader(object sender, EventArgs e)
        {
            OnPackageEnabled -= InitializeXRLoader;
            XRLoaderHelper.EnableLoader(Package, XRLoaderName);
        }
    }
}
#endif
