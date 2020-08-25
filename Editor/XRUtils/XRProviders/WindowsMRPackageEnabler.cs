#if UNITY_XR_MANAGEMENT && WINDOWS_XR
namespace Innoactive.CreatorEditor.XRUtils
{
    /// <summary>
    /// Enables the Windows MR Plugin.
    /// </summary>
    internal sealed class WindowsMRPackageEnabler : XRProvider
    {
        /// <inheritdoc/>
        public override string Package { get; } = "com.unity.xr.windowsmr";

        /// <inheritdoc/>
        public override int Priority { get; } = 2;

        protected override string XRLoaderName { get; } = "WindowsMRLoader";
    }
}
#endif
