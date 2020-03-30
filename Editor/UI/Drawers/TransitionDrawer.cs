using System;
using System.Linq;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Attributes;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Drawers
{
    /// <summary>
    /// Drawer for a transition which displays name of the target step as part of its label.
    /// </summary>
    [DefaultTrainingDrawer(typeof(Transition))]
    public class TransitionDrawer : DataOwnerDrawer
    {
        public override GUIContent GetLabel(object value, Type declaredType)
        {
            return GetTypeNameLabel(value, declaredType);
        }

        protected virtual GUIContent GetTypeNameLabel(object value, Type declaredType)
        {
            Transition.EntityData transition = ((Transition)value).Data;

            Type actualType = value.GetType();

            string typeName = actualType.Name;
            DisplayNameAttribute typeNameAttribute = actualType.GetAttributes<DisplayNameAttribute>(true).FirstOrDefault();
            if (typeNameAttribute != null)
            {
                typeName = typeNameAttribute.Name;
            }

            string target;
            if (transition.TargetStep == null)
            {
                target = "the End of the Chapter";
            }
            else
            {
                target = string.Format("\"{0}\"", transition.TargetStep.Data.Name);
            }

            return new GUIContent(string.Format("{0} to {1}", typeName, target));
        }
    }
}
