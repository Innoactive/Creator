using System;
using UnityEditor;
using UnityEngine;
using Innoactive.Creator.Internationalization;
using Innoactive.Hub.Training.Editors.Utils;

namespace Innoactive.Hub.Training.Editors.Drawers
{
    /// <summary>
    /// Custom drawer for localized strings in PlayAudioBehavior's audio data to flatten visible hierarchy.
    /// </summary>
    public class AudioDataLocalizedStringDrawer : AbstractDrawer
    {
        /// <summary>
        /// Label displayed in front of the default path field.
        /// </summary>
        protected virtual string defaultValueName { get { return "Default"; } }

        /// <inheritdoc />
        public override Rect Draw(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label)
        {
            LocalizedString localizedString = currentValue as LocalizedString;

            if (localizedString == null)
            {
                changeValueCallback(new LocalizedString());
                return rect;
            }

            Rect keyRect = rect;
            keyRect.height = EditorDrawingHelper.SingleLineHeight;
            string newKey = EditorGUI.TextField(keyRect, "Localization key", localizedString.Key);

            Rect defaultRect = keyRect;
            defaultRect.y += keyRect.height + EditorDrawingHelper.VerticalSpacing;
            string newDefault = EditorGUI.TextField(defaultRect, defaultValueName, localizedString.Value);

            if (newKey != localizedString.Key || newDefault != localizedString.DefaultText)
            {
                string oldKey = localizedString.Key;
                string oldDefault = localizedString.DefaultText;
                ChangeValue(() => new LocalizedString(newKey, newDefault),
                    () => new LocalizedString(oldKey, oldDefault),
                    changeValueCallback);
            }

            rect.height = keyRect.height + defaultRect.height + EditorDrawingHelper.VerticalSpacing;
            return rect;
        }
    }
}
