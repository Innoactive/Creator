using System;
using System.Reflection;
using Innoactive.Hub.Training.Behaviors;
using Innoactive.Hub.Training.Utils;
using UnityEngine;

namespace Innoactive.Hub.Training.Editors.Drawers
{
    /// <summary>
    /// Drawer for an <see cref="NonblockingWrapperBehavior"/>. Instead of showing it as a separate behavior, display it as a toggle in a child behavior.
    /// </summary>
    [Obsolete("NonblockingWrapperBehavior is obsolete. Implement IBackgroundBehaviorData in the EntityData of your custom behavior instead.")]
    [DefaultTrainingDrawer(typeof(NonblockingWrapperBehavior.EntityData))]
    public class NonblockingWrapperBehaviorDrawer : NameableDrawer
    {
        /// <inheritdoc />
        public override Rect Draw(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label)
        {
            NonblockingWrapperBehavior.EntityData behavior = currentValue as NonblockingWrapperBehavior.EntityData;
            ITrainingDrawer drawer = DrawerLocator.GetDrawerForValue(behavior.Behavior, typeof(IBehavior));
            float height = drawer.Draw(rect, behavior.Behavior, (newWrappedBehavior) =>
            {
                behavior.Behavior = (IBehavior) newWrappedBehavior;
                changeValueCallback(behavior);
            }, label).height;

            // Don't draw checkbox anymore. Behavior is obsolete.

            rect.height = height;

            return rect;
        }

        /// <inheritdoc />
        public override GUIContent GetLabel(object value, Type declaredType)
        {
            NonblockingWrapperBehavior.EntityData behavior = value as NonblockingWrapperBehavior.EntityData;

            ITrainingDrawer drawer = DrawerLocator.GetDrawerForValue(behavior.Behavior, typeof(IBehavior));

            return drawer.GetLabel(behavior.Behavior, typeof(IBehavior));
        }

        /// <inheritdoc />
        public override GUIContent GetLabel(MemberInfo memberInfo, object memberOwner)
        {
            NonblockingWrapperBehavior.EntityData behavior = ReflectionUtils.GetValueFromPropertyOrField(memberOwner, memberInfo) as NonblockingWrapperBehavior.EntityData;

            return GetLabel(behavior.Behavior, typeof(IBehavior));
        }
    }
}
