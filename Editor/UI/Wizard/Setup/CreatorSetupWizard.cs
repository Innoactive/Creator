using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Innoactive.CreatorEditor.PackageManager;
using Innoactive.CreatorEditor.XRUtils;

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
#if UNITY_2019_4_OR_NEWER && !UNITY_EDITOR_OSX
        static CreatorSetupWizard()
        {
            if (Application.isBatchMode == false)
            {
                DependencyManager.OnPostProcess += OnDependenciesRetrieved;
            }
        }

        private static void OnDependenciesRetrieved(object sender, DependencyManager.DependenciesEnabledEventArgs e)
        {
            CreatorProjectSettings settings = CreatorProjectSettings.Load();

            if (settings.IsFirstTimeStarted)
            {
                settings.IsFirstTimeStarted = false;
                settings.Save();
                Show();
            }

            DependencyManager.OnPostProcess -= OnDependenciesRetrieved;
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

            if (EditorReflectionUtils.AssemblyExists(XRAssemblyName) && XRLoaderHelper.GetCurrentXRConfiguration()
                .Any(loader => loader == XRLoaderHelper.XRConfiguration.XRManagement || loader == XRLoaderHelper.XRConfiguration.None))
            {
                pages.Insert(2, new XRSDKSetupPage());
            }

            wizard.Setup("Innoactive Creator - VR Training Setup Wizard", pages);
            wizard.ShowModal();
        }
#endif
    }
}
