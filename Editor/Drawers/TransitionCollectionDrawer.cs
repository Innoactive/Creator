using System;
using System.Reflection;
using Innoactive.Hub.Training;
using Innoactive.Hub.Training.Editors.Drawers;
using UnityEngine;

[DefaultTrainingDrawer(typeof(TransitionCollection))]
public class TransitionCollectionDrawer : DataOwnerDrawer
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
