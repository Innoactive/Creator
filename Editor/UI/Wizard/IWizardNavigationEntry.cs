using UnityEngine;

namespace Innoactive.Creator.Core.Editor.UI.Wizard
{
    internal interface IWizardNavigationEntry
    {
        bool Selected { get; set; }
        void Draw(Rect window);
    }
}
