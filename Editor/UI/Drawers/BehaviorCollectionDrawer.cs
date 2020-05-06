using System;
using System.Reflection;
using Innoactive.Creator.Core;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Drawers
{
    [DefaultTrainingDrawer(typeof(BehaviorCollection))]
    internal class BehaviorCollectionDrawer : DataOwnerDrawer
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
