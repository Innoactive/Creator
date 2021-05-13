using System;
using System.Reflection;
using VPG.Creator.Core;
using UnityEngine;

namespace VPG.CreatorEditor.UI.Drawers
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
