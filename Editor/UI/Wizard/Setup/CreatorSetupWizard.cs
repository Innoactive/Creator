using System;
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
    internal class CreatorSetupWizard : WizardWindow
    {
        private const string XRAssemblyName = "Innoactive.Creator.XRInteraction";

        protected override List<WizardPage> Pages { get; set; }

        protected override string Title => "Innoactive Creator - VR Training Setup Wizard";

        [SerializeField]
        protected WelcomePage welcomePage = new WelcomePage();

        [SerializeField]
        protected TrainingSceneSetupPage trainingSceneSetupPage = new TrainingSceneSetupPage();

        [SerializeField]
        protected AnalyticsPage analyticsPage = new AnalyticsPage();

        [SerializeField]
        protected AllAboutPage allAboutPage = new AllAboutPage();

        [SerializeField]
        protected XRSDKSetupPage xrSdkSetupPage = new XRSDKSetupPage();

        [MenuItem("Innoactive/Creator/Create New Course...")]
        public static void Show()
        {
            Pages = new List<WizardPage>()
            {
                welcomePage,
                trainingSceneSetupPage,
                analyticsPage,
                allAboutPage
            };

            /*
            if (EditorReflectionUtils.AssemblyExists(XRAssemblyName))
            {
                Pages.Insert(2, xrSdkSetupPage);
            }
            */
        }
    }
}
