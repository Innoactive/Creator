using Innoactive.Creator.Core.Conditions;
using Innoactive.CreatorEditor.Configuration;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Conditions
{
    public class TimeoutMenuItem : StepInspectorMenu.Item<ICondition>
    {
        public override GUIContent DisplayedName
        {
            get { return new GUIContent("Timeout"); }
        }

        public override ICondition GetNewItem()
        {
            return new TimeoutCondition();
        }
    }
}
