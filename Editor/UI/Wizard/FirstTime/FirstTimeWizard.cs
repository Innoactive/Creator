using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Wizard
{
    [InitializeOnLoad]
    internal static class FirstTimeWizard
    {
        public const string shownOnCreatorImport = "ShownOnCreatorImport";

        static FirstTimeWizard()
        {
            EditorApplication.update += ShowOnLoad;
        }

        private static void ShowOnLoad()
        {
            if (EditorPrefs.GetBool(shownOnCreatorImport) == false)
            {
                Show();
            }

            EditorApplication.update -= ShowOnLoad;
        }

        [MenuItem("Innoactive/Run Training Setup Wizard...")]
        public static void Show()
        {
            WizardWindow wizard = ScriptableObject.CreateInstance<WizardWindow>();
            List<WizardPage> pages = new List<WizardPage>()
            {
                new WelcomePage(),
                new TrainingSceneSetupPage(),
                new VRHardwareSetupPage(),
                new AnalyticsPage(),
                new AllAboutPage()
            };
            wizard.Setup("Innoactive Creator - My first VR Training - Wizard", pages);
            wizard.ShowModal();
        }
    }
}
