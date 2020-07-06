using System;
using Innoactive.Creator.Core.Behaviors;
using Innoactive.Creator.Core.SceneObjects;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Drawers
{
    [DefaultTrainingDrawer(typeof(LockablePropertyReference))]
    public class LockableObjectDrawer : AbstractDrawer
    {
        public override Rect Draw(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label)
        {

            GUI.Button(rect, "Button");

            return rect;
        }
    }
}
