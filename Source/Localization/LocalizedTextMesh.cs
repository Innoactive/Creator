using UnityEngine;

namespace Innoactive.Hub
{
    [RequireComponent(typeof(TextMesh))]
    public class LocalizedTextMesh : MonoBehaviour
    {
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
                Debug.LogError("LocalizedUIText failed - no localized string assigned.");
                return;
            }

            TextMesh text = GetComponent<TextMesh>();
            if (text == null)
            {
                Debug.LogError("LocalizedUIText failed - no TextMesh component found.");
                return;
            }

            text.text = localizedString.Value.Replace("\\n", "\n");
        }
    }
}
