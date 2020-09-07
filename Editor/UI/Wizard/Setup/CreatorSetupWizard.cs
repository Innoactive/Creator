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
        private const string XRInnoactiveAssemblyName = "Innoactive.Creator.XRInteraction";
        private const string XRAssemblyName = "Unity.XR.Management";
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

        [MenuItem("Innoactive/Create New Course...", false, 0)]
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

            bool isShowingXRSetupPage = EditorReflectionUtils.AssemblyExists(XRInnoactiveAssemblyName);
            isShowingXRSetupPage &= EditorReflectionUtils.AssemblyExists(XRAssemblyName) == false;
            isShowingXRSetupPage &= XRLoaderHelper.GetCurrentXRConfiguration()
                .Contains(XRLoaderHelper.XRConfiguration.XRLegacy) == false;

            if (isShowingXRSetupPage)
            {
                pages.Insert(2, new XRSDKSetupPage());
            }

            wizard.Setup("Innoactive Creator - VR Training Setup Wizard", pages);
            wizard.ShowModal();
        }
#endif
    }
}
