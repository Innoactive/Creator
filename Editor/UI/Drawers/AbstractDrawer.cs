using System;
using System.Linq;
using System.Reflection;
using VPG.Core;
using VPG.Core.Attributes;
using VPG.Core.Utils;
using VPG.Editor.UndoRedo;
using UnityEngine;

namespace VPG.Editor.UI.Drawers
{
    /// <summary>
    /// Simple base drawer class.
    /// </summary>
    public abstract class AbstractDrawer : ITrainingDrawer
    {
        /// <inheritdoc />
        public Rect Draw(Rect rect, object currentValue, Action<object> changeValueCallback, string label)
        {
            return Draw(rect, currentValue, changeValueCallback, new GUIContent(label));
        }

        /// <inheritdoc />
        public abstract Rect Draw(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label);

        public virtual GUIContent GetLabel(MemberInfo memberInfo, object memberOwner)
        {
            Type memberType = ReflectionUtils.GetDeclaredTypeOfPropertyOrField(memberInfo);
            object value = ReflectionUtils.GetValueFromPropertyOrField(memberOwner, memberInfo);

            if (value != null)
            {
                memberType = value.GetType();
            }

            DisplayNameAttribute attribute = memberInfo.GetAttributes<DisplayNameAttribute>(true).FirstOrDefault();

            if (attribute != null && attribute.Name != null)
            {
                return new GUIContent(attribute.Name);
            }

            GUIContent valueLabel = GetLabel(value, memberType);

            if (string.IsNullOrEmpty(valueLabel.text) && valueLabel.image == null)
            {
                return new GUIContent(memberInfo.Name);
            }

            return valueLabel;
        }

        public virtual GUIContent GetLabel(object value, Type declaredType)
        {
            INamedData nameable = value as INamedData;

            if (nameable == null || string.IsNullOrEmpty(nameable.Name))
            {
                return new GUIContent(string.Empty);
            }

            return new GUIContent(nameable.Name);
        }

        /// <inheritdoc />
        public void ChangeValue(Func<object> getNewValueCallback, Func<object> getOldValueCallback, Action<object> assignValueCallback)
        {
            // ReSharper disable once ImplicitlyCapturedClosure
            Action doCallback = () => assignValueCallback(getNewValueCallback());
            // ReSharper disable once ImplicitlyCapturedClosure
            Action undoCallback = () => assignValueCallback(getOldValueCallback());
            RevertableChangesHandler.Do(new CourseCommand(doCallback, undoCallback));
        }
    }
}
