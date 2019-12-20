namespace Innoactive.Hub.Training.Editors.Drawers
{
    /// <summary>
    /// Custom drawer for localized strings in `Play TTS Audio` behavior's audio data to flatten visible hierarchy.
    /// </summary>
    public class TextToSpeechAudioDataLocalizedStringDrawer : AudioDataLocalizedStringDrawer
    {
        /// <inheritdoc />
        protected override string defaultValueName { get { return "Default text"; } }
    }
}
