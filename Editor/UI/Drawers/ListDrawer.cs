using System;
using System.Collections;
using Innoactive.Creator.Core.Utils;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Drawers
{
    /// <summary>
    /// A default training drawer for types implementing `IList`.
    /// </summary>
    [DefaultTrainingDrawer(typeof(IList))]
    public class ListDrawer : AbstractDrawer
    {
        /// <inheritdoc />
        public override Rect Draw(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label)
        {
            IList list = (IList)currentValue;

            Type entryDeclaredType = ReflectionUtils.GetEntryType(currentValue);

            float entryWidth = rect.width;

            float currentY = rect.y;

            GUIStyle labelStyle = new GUIStyle(EditorStyles.label)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 12
            };

            EditorGUI.LabelField(new Rect(rect.x, currentY, rect.width, EditorDrawingHelper.HeaderLineHeight), label, labelStyle);

            currentY += EditorDrawingHelper.HeaderLineHeight;

            object[] entries = new object[list.Count];
            list.CopyTo(entries, 0);

            int closuredLength = entries.Length;
            for (int index = 0; index < entries.Length; index++)
            {
                currentY += EditorDrawingHelper.VerticalSpacing;
                int closuredIndex = index;
                object entry = entries[index];

                ITrainingDrawer entryDrawer = DrawerLocator.GetDrawerForValue(entry, entryDeclaredType);

                Action<object> entryValueChangedCallback = newValue =>
                {
                    if (list.Count < closuredLength)
                    {
                        ReflectionUtils.InsertIntoList(ref list, closuredIndex, newValue);
                    }
                    else
                    {
                        list[closuredIndex] = newValue;
                    }

                    MetadataWrapper wrapper = newValue as MetadataWrapper;
                    // if new value is null, or the value is wrapper with null value, remove it from list.
                    if (newValue == null || (wrapper != null && wrapper.Value == null))
                    {
                        ReflectionUtils.RemoveFromList(ref list, closuredIndex);
                    }

                    changeValueCallback(list);
                };

                GUIContent entryLabel = entryDrawer.GetLabel(entry, entryDeclaredType);

                currentY += entryDrawer.Draw(new Rect(rect.x, currentY, entryWidth, 0), entry, entryValueChangedCallback, entryLabel).height;
            }

            return new Rect(rect.x, rect.y, rect.width, currentY - rect.y);
        }
    }
}
