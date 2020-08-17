using System;
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
