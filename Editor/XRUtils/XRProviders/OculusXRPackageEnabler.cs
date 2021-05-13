#if UNITY_XR_MANAGEMENT && OCULUS_XR
namespace VPG.CreatorEditor.XRUtils
{
    /// <summary>
    /// Enables the Oculus XR Plugin.
    /// </summary>
    internal sealed class OculusXRPackageEnabler : XRProvider
    {
        /// <inheritdoc/>
        public override string Package { get; } = "com.unity.xr.oculus";

        /// <inheritdoc/>
        public override int Priority { get; } = 2;

        protected override string XRLoaderName { get; } = "OculusLoader";
    }
}
#endif
