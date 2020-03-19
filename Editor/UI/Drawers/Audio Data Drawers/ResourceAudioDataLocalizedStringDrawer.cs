namespace Innoactive.CreatorEditor.UI.Drawers
{
    /// <summary>
    /// Custom drawer for localized strings in `Play Audio File` behavior's audio data to flatten visible hierarchy.
    /// </summary>
    public class ResourceAudioDataLocalizedStringDrawer : AudioDataLocalizedStringDrawer
    {
        /// <inheritdoc />
        protected override string defaultValueName { get { return "Default resource path"; } }
    }
}
