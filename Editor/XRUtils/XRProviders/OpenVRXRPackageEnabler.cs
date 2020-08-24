#if CREATOR_OPEN_VR
namespace Innoactive.CreatorEditor.XRUtils
{
    /// <summary>
    /// Enables the OpenVR XR Plugin.
    /// </summary>
    internal sealed class OpenVRXRPackageEnabler : XRProvider
    {
        /// <inheritdoc/>
        public override string Package { get; } = "com.valvesoftware.unity.openvr";

        /// <inheritdoc/>
        public override int Priority { get; } = 2;

        protected override string XRLoaderName { get; } = "OpenVRLoader";
    }
}
#endif
