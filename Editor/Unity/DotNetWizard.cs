using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor
{
    /// <summary>
    /// ScriptableWizard that helps setting the 'API Compatibility Level' to '.Net 4.x'.
    /// If the 'API Compatibility Level' is already set to '.Net 4.x' this window is closed automatically.
    /// </summary>
    internal class DotNetWizard : ScriptableWizard
    {
        private BuildTargetGroup buildTargetGroup;
        private ApiCompatibilityLevel currentLevel;
        private readonly Vector2 fixedSize = new Vector2(320, 140);

        /// <summary>
        /// Opens an instance of <see cref="DotNetWizard"/>.
        /// </summary>
        public static void OpenWizard()
        {
            if (EditorUtils.GetCurrentCompatibilityLevel() != ApiCompatibilityLevel.NET_4_6)
            {
                DisplayWizard<DotNetWizard>(".NET 4.x Validation", "Fix it");
            }
        }

        /// <summary>
        /// This is called when the wizard is opened or whenever the user changes something in the wizard.
        /// </summary>
        private void OnWizardUpdate()
        {
            minSize = maxSize = fixedSize;
            helpString = "API Compatibility level*";

            if (currentLevel == ApiCompatibilityLevel.NET_4_6)
            {
                Close();
            }

            GatherCurrentSettings();
        }

        ///<inheritdoc />
        protected override bool DrawWizardGUI()
        {
            EditorGUILayout.HelpBox($"This Unity project is configured for {currentLevel}, but the Innoactive Creator requires .NET 4.X support.", MessageType.Error);
            return false;
        }

        /// <summary>
        /// This is called when the user clicks on the Wizard's button.
        /// </summary>
        private void OnWizardCreate()
        {
            PlayerSettings.SetApiCompatibilityLevel(buildTargetGroup, ApiCompatibilityLevel.NET_4_6);
        }

        private void GatherCurrentSettings()
        {
            BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;
            buildTargetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);
            currentLevel = PlayerSettings.GetApiCompatibilityLevel(buildTargetGroup);
        }
    }
}
