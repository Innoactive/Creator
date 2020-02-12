using UnityEngine;

namespace Innoactive.Hub
{
    [RequireComponent(typeof(TextMesh))]
    public class LocalizedTextMesh : MonoBehaviour
    {
        private static readonly Common.Logging.ILog logger = Logging.LogManager.GetLogger<LocalizedTextMesh>();

        [SerializeField]
        private LocalizedString localizedString = null;

        private void OnEnable()
        {
            UpateText();
            Localization.OnLocaleChanged += HandleLocalizationChange;
        }

        private void OnDisable()
        {
            Localization.OnLocaleChanged -= HandleLocalizationChange;
        }

        private void HandleLocalizationChange(string newLocale, string oldLocale)
        {
            UpateText();
        }

        private void UpateText()
        {
            if (localizedString == null)
            {
                logger.Error("LocalizedUIText failed - no localized string assigned.");
                return;
            }

            TextMesh text = GetComponent<TextMesh>();
            if (text == null)
            {
                logger.Error("LocalizedUIText failed - no TextMesh component found.");
                return;
            }

            text.text = localizedString.Value.Replace("\\n", "\n");
        }
    }
}
