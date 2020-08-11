#if CREATOR_XR_MANAGEMENT
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;
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
                    XRLoaderHelper.LoadOpenVR();
                    break;
                case XRLoaderHelper.XRSDK.Oculus:
                    AddScriptingDefineSymbol(XRLoaderHelper.OculusDefineSymbol, new [] {BuildTarget.StandaloneWindows, BuildTarget.Android});
                    break;
                case XRLoaderHelper.XRSDK.WindowsMR:
                    AddScriptingDefineSymbol(XRLoaderHelper.WindowsMRDefineSymbol, new [] {BuildTarget.StandaloneWindows});
                    break;
            }

            EditorPrefs.DeleteKey(nameof(XRLoaderHelper.XRSDK));
            OnPackageEnabled -= InitializeXRLoader;
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
    }
}
#endif
