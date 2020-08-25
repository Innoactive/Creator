#if CREATOR_OCULUS || CREATOR_OPEN_VR || CREATOR_WINDOWS_MR
using System;
using UnityEngine;
using UnityEngine.XR.Management;
using Innoactive.Creator.Core.Utils;
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

        private void InitializeXRLoader(object sender, EventArgs e)
        {
            foreach (Type loader in ReflectionUtils.GetConcreteImplementationsOf<XRLoader>())
            {
                if (loader.Name == XRLoaderName && XRGeneralSettings.Instance != null)
                {
                    XRGeneralSettings.Instance.Manager.loaders.Clear();
                    XRGeneralSettings.Instance.Manager.loaders.Add(ScriptableObject.CreateInstance(loader) as XRLoader);
                }
            }

            OnPackageEnabled -= InitializeXRLoader;
        }
    }
}
#endif
