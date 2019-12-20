using System;
using System.Reflection;
using UnityEngine;

namespace Innoactive.Hub.Training.Editors.Drawers
{
    [DefaultTrainingDrawer(typeof(BehaviorCollection))]
    public class BehaviorCollectionDrawer : DataOwnerDrawer
    {
        public override GUIContent GetLabel(MemberInfo memberInfo, object memberOwner)
        {
            return null;
        }

        public override GUIContent GetLabel(object value, Type declaredType)
        {
            return null;
        }
    }
}
