using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Wizard
{
    internal interface IWizardNavigationEntry
    {
        bool Selected { get; set; }
        void Draw(Rect window);
    }
}
