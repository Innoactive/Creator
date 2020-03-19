using System;
using Innoactive.Creator.Core.Internationalization;
using Innoactive.Creator.Core.Utils;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Drawers
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
