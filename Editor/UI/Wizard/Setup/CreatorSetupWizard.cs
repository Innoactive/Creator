using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Innoactive.CreatorEditor.UI.Wizard
{
    /// <summary>
    /// Wizard which guides the user through setting up a new training project,
    /// including a training course, scene and XR hardware.
    /// </summary>
    [InitializeOnLoad]
    internal static class CreatorSetupWizard
    {
        private const string XRAssemblyName = "Innoactive.Creator.XRInteraction";

        static CreatorSetupWizard()
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
            WizardWindow wizard = ScriptableObject.CreateInstance<WizardWindow>();
            List<WizardPage> pages = new List<WizardPage>()
            {
                new WelcomePage(),
                new TrainingSceneSetupPage(),
                new AnalyticsPage(),
                new AllAboutPage()
            };

            wizard.Setup("Innoactive Creator - VR Training Setup Wizard", pages);
            wizard.ShowModal();
        }
    }
}
