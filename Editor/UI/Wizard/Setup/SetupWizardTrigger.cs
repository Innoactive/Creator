using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Wizard
{
    public class SetupWizardTrigger
    {
        static SetupWizardTrigger()
        {
            EditorApplication.update += ShowOnLoad;
        }

        private static void ShowOnLoad()
        {
            EditorApplication.update -= ShowOnLoad;
            CreatorProjectSettings settings = CreatorProjectSettings.Load();

            if (settings.IsFirstTimeStarted)
            {
                Show();
                settings.IsFirstTimeStarted = false;
                settings.Save();
            }
        }

        [MenuItem("Innoactive/Creator/Create New Course...")]
        public static void Show()
        {
            EditorWindow.GetWindow<CreatorSetupWizard>()?.Close();
            CreatorSetupWizard instance = ScriptableObject.CreateInstance<CreatorSetupWizard>();
#if UNITY_2019_4_OR_NEWER
            instance.ShowModal();
#else
            instance.ShowModalUtility();
#endif
            instance.Focus();
        }
    }
}
