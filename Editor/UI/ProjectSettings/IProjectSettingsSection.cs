using System;

namespace Innoactive.CreatorEditor.UI
{
    /// <summary>
    /// Allows to inject a settings section into a setting provider.
    /// </summary>
    public interface IProjectSettingsSection
    {
        /// <summary>
        /// Title of this section.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Type of the target setting provider.
        /// </summary>
        Type TargetPageProvider { get; }

        /// <summary>
        /// Determines the draw order, lower priority will be drawn first.
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Your draw call.
        /// </summary>
        void OnGUI(string searchContext);
    }
}
