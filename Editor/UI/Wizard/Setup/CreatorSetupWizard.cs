using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using VPG.Editor.PackageManager;
using VPG.Editor.XRUtils;

namespace VPG.Editor.UI.Wizard
{
    /// <summary>
    /// Wizard which guides the user through setting up a new training project,
    /// including a training course, scene and XR hardware.
    /// </summary>
    ///
#if UNITY_2019_4_OR_NEWER && !UNITY_EDITOR_OSX
    [InitializeOnLoad]
#endif
    public static class CreatorSetupWizard
    {
        /// <summary>
        /// Will be called when the VR Process Gizmo Setup wizard is closed.
        /// </summary>
        public static event EventHandler<EventArgs> SetupFinished;

        private const string XRDefaultAssemblyName = "VPG.Creator.XRInteraction";
        private const string XRAssemblyName = "Unity.XR.Management";
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

#if UNITY_2019_4_OR_NEWER && !UNITY_EDITOR_OSX
        [MenuItem("VR Process Gizmo/Create New Course...", false, 0)]
#endif
        internal static void Show()
        {
            WizardWindow wizard = EditorWindow.CreateInstance<WizardWindow>();
            List<WizardPage> pages = new List<WizardPage>()
            {
                new WelcomePage(),
                new TrainingSceneSetupPage(),
                //new AnalyticsPage(),
                new AllAboutPage()
            };

            int xrSetupIndex = 2;
#if CREATOR_PRO
            if (CreatorPro.Account.UserAccount.IsAllowedToUsePro() == false)
            {
                pages.Insert(1, new CreatorPro.Core.CreatorLoginPage());
                xrSetupIndex++;
            }
#endif
            bool isShowingXRSetupPage = EditorReflectionUtils.AssemblyExists(XRDefaultAssemblyName);
            isShowingXRSetupPage &= EditorReflectionUtils.AssemblyExists(XRAssemblyName) == false;
            isShowingXRSetupPage &= XRLoaderHelper.GetCurrentXRConfiguration()
                .Contains(XRLoaderHelper.XRConfiguration.XRLegacy) == false;

            if (isShowingXRSetupPage)
            {
                pages.Insert(xrSetupIndex, new XRSDKSetupPage());
            }

            wizard.WizardClosing += OnWizardClosing;

            wizard.Setup("Creator - VR Training Setup Wizard", pages);
            wizard.ShowModalUtility();
        }

        private static void OnWizardClosing(object sender, EventArgs args)
        {
            ((WizardWindow)sender).WizardClosing -= OnWizardClosing;
            SetupFinished?.Invoke(sender, args);
        }
    }
}
