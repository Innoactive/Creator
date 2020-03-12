using Innoactive.Hub.Training.Editors.Configuration;
using UnityEngine;

namespace Innoactive.Hub.Training.Behaviors.Editors
{
    public class BehaviorSequenceMenuItem : Menu.Item<IBehavior>
    {
        public override GUIContent DisplayedName
        {
            get
            {
                return new GUIContent("Behaviors Sequence");
            }
        }

        public override IBehavior GetNewItem()
        {
            return new BehaviorSequence();
        }
    }
}
