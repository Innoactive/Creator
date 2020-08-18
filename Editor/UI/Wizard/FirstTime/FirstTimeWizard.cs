using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Wizard
{
    internal static class FirstTimeWizard
    {
        [MenuItem("Innoactive/Wizard")]
        public static void Show()
        {
            WizardWindow wizard = ScriptableObject.CreateInstance<WizardWindow>();
            List<WizardPage> pages = new List<WizardPage>()
            {
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
