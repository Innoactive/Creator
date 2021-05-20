using UnityEditor;
using UnityEngine;

namespace VPG.Editor.UI.Wizard
{
    internal class WelcomePage : WizardPage
    {
        public WelcomePage() : base("Welcome")
        {

        }

        public override void Draw(Rect window)
        {
            GUILayout.BeginArea(window);
                GUILayout.Label("Welcome to the VR Process Gizmo", VPGEditorStyles.Title);
                GUILayout.Label("We want to get you started with the VR Process Gizmo as fast as possible.\nThis Wizard guides you through the process.", VPGEditorStyles.Paragraph);
            GUILayout.EndArea();
        }
    }
}
