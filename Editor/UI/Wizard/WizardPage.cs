using System;
using UnityEditor;
using UnityEngine;

namespace Innoactive.Creator.Core.Editor.UI.Wizard
{
    /// <summary>
    /// Wizard pages which allows you to implement your content.
    /// Care about implementing your state serializable.
    /// </summary>
    [Serializable]
    public abstract class WizardPage
    {
        [SerializeField]
        protected Vector2 currentScrollPosition;

        [SerializeField]
        public string Name;

        [SerializeField]
        public bool AllowSkip;

        [SerializeField]
        public bool CanProceed = true;

        protected int horizontalSpace = 30;
        protected int verticalSpace = 30;

        public WizardPage()
        {

        }

        public WizardPage(string name, bool allowSkip = false)
        {
            Name = name;
            AllowSkip = allowSkip;
        }

        public abstract void Draw(Rect window);

        public virtual Rect DrawTitle(Rect window, string title)
        {
            GUILayout.BeginArea(window);
            GUILayout.Space(verticalSpace);
            GUILayout.BeginHorizontal();

                GUILayout.Space(horizontalSpace);
                GUILayout.Label(title, EditorStyles.largeLabel);

            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            Rect contentRect = new Rect(window.x, verticalSpace * 2, window.width, window.height - verticalSpace * 2);

            return contentRect;
        }

        public virtual void Apply()
        {

        }

        public virtual void Skip()
        {

        }

        public virtual void Cancel()
        {

        }
    }
}
