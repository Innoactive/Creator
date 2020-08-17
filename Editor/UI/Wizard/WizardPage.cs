using System;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Wizard
{
    /// <summary>
    /// Wizard pages which allows you to implement your content.
    /// Care about implementing your state serializable.
    /// </summary>
    [Serializable]
    public abstract class WizardPage
    {
        public string Name;

        public bool AllowSkip;

        public bool CanProceed = true;

        public bool Mandatory = true;

        protected int horizontalSpace = 30;
        protected int verticalSpace = 30;

        public WizardPage()
        {

        }

        public WizardPage(string name, bool allowSkip = false, bool mandatory = true)
        {
            Name = name;
            AllowSkip = allowSkip;
            Mandatory = mandatory;
        }

        public abstract void Draw(Rect window);

        public virtual void Apply()
        {

        }

        public virtual void Skip()
        {

        }

        public virtual void Back()
        {

        }

        public virtual void Closing(bool isCompleted)
        {

        }
    }
}
