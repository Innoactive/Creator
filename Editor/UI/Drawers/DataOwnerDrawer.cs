using System;
using System.Linq;
using Innoactive.Creator.Core;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Drawers
{
    [DefaultTrainingDrawer(typeof(IDataOwner))]
    internal class DataOwnerDrawer : AbstractDrawer
    {
        public override Rect Draw(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label)
        {
            if (currentValue == null)
            {
                throw new NullReferenceException("Attempting to draw null object.");
            }

            IData data = ((IDataOwner)currentValue).Data;

            ITrainingDrawer dataDrawer = DrawerLocator.GetDrawerForMember(EditorReflectionUtils.GetFieldsAndPropertiesToDraw(currentValue).First(member => member.Name == "Data"), currentValue);

            return dataDrawer.Draw(rect, data, (value) => changeValueCallback(currentValue), label);
        }

        public override GUIContent GetLabel(object value, Type declaredType)
        {
            IData data = ((IDataOwner)value).Data;

            ITrainingDrawer dataDrawer = DrawerLocator.GetDrawerForMember(EditorReflectionUtils.GetFieldsAndPropertiesToDraw(value).First(member => member.Name == "Data"), value);
            return dataDrawer.GetLabel(data, declaredType);
        }
    }
}
