using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Wizard
{
    internal class WelcomePage : WizardPage
    {
        [SerializeField]
        private bool skipWizard;

        public WelcomePage() : base("Welcome")
        {
            skipWizard = EditorPrefs.GetBool(FirstTimeWizard.skipWizardKey, false);
        }

        public override void Draw(Rect window)
        {
            GUILayout.BeginArea(window);
                GUILayout.Label("Welcome to Innoactive Creator", CreatorEditorStyles.Title);
                GUILayout.Label("We want to get you started with Innoactive Creator as fast as possible.\nThis Wizard guides you through the process.", CreatorEditorStyles.Paragraph);
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(window.x + CreatorEditorStyles.Indent, window.y + window.height * 0.9f, window.width, window.height * 0.1f));
                skipWizard = GUILayout.Toggle(skipWizard, "Skip the Wizard in the future.", CreatorEditorStyles.Toggle);
            GUILayout.EndArea();
        }

        /// <inheritdoc />
        public override void Apply()
        {
            base.Apply();

            EditorPrefs.SetBool(FirstTimeWizard.skipWizardKey, skipWizard);
        }

        public override void Closing(bool isCompleted)
        {
            base.Closing(isCompleted);

            EditorPrefs.SetBool(FirstTimeWizard.skipWizardKey, skipWizard);
        }
    }
}
