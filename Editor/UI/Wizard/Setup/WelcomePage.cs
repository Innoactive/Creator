using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Wizard
{
    internal class WelcomePage : WizardPage
    {
        public WelcomePage() : base("Welcome")
        {

        }

        public override void Draw(Rect window)
        {
            GUILayout.BeginArea(window);
                GUILayout.Label("Welcome to Innoactive Creator", CreatorEditorStyles.Title);
                GUILayout.Label("We want to get you started with Innoactive Creator as fast as possible.\nThis Wizard guides you through the process.", CreatorEditorStyles.Paragraph);
            GUILayout.EndArea();
        }
    }
}
