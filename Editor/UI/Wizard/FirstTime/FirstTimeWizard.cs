using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Innoactive.CreatorEditor.UI.Wizard
{
    internal static class FirstTimeWizard
    {
        public const string XRAssemblyName = "Innoactive.Creator.XRInteraction";

        [MenuItem("Innoactive/Wizard")]
        public static void Show()
        {
            WizardWindow wizard = ScriptableObject.CreateInstance<WizardWindow>();
            List<WizardPage> pages = new List<WizardPage>()
            {
                new TrainingSceneSetupPage(),
                new AnalyticsPage(),
                new AllAboutPage()
            };

            if (EditorReflectionUtils.AssemblyExists(XRAssemblyName))
            {
                pages.Insert(1, new XRSDKSetupPage());
            }
            wizard.Setup("Innoactive Creator - My first VR Training - Wizard", pages);
            wizard.ShowModal();
        }
    }
}
