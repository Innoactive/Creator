using UnityEngine;

namespace Innoactive.Hub
{
    [RequireComponent(typeof(UnityEngine.UI.Text))]
    public class LocalizedUIText : MonoBehaviour
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

            UnityEngine.UI.Text text = GetComponent<UnityEngine.UI.Text>();
            if (text == null)
            {
                Debug.LogError("LocalizedUIText failed - no UI.Text component found.");
                return;
            }

            text.text = localizedString.Value.Replace("\\n", "\n");
        }
    }
}
