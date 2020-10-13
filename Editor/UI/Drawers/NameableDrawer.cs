using System;
using System.Collections.Generic;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Conditions;
using Innoactive.CreatorEditor.CourseValidation;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Drawers
{
    /// <summary>
    /// Drawer for values implementing INameable interface.
    /// Instead of drawing a plain text as a label, it draws a TextField with the name.
    /// </summary>
    [DefaultTrainingDrawer(typeof(INamedData))]
    public class NameableDrawer : ObjectDrawer
    {
        /// <inheritdoc />
        protected override float DrawLabel(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label)
        {
            INamedData nameable = currentValue as INamedData;


            List<EditorReportEntry> reports = GetValidationReports(currentValue);
            if (reports.Count > 0)
            {
                Rect warningRect = rect;
                warningRect.width = 20;
                rect.x += 20;
                GUI.Label(warningRect, AddValidationInformation(new GUIContent(), reports));
            }

            Rect nameRect = rect;
            nameRect.width = EditorGUIUtility.labelWidth;
            Rect typeRect = rect;
            typeRect.x += EditorGUIUtility.labelWidth;
            typeRect.width -= EditorGUIUtility.labelWidth;

            GUIStyle textFieldStyle = new GUIStyle(EditorStyles.textField)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 12
            };

            string newName = EditorGUI.DelayedTextField(nameRect, nameable.Name, textFieldStyle);

            GUIStyle labelStyle = new GUIStyle(EditorStyles.label)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 12,
                padding = new RectOffset(4, 0, 0, 0)
            };
            EditorGUI.LabelField(typeRect, GetTypeNameLabel(nameable, nameable.GetType()), labelStyle);

            if (newName != nameable.Name)
            {
                string oldName = nameable.Name;
                nameable.Name = newName;
                ChangeValue(() =>
                    {
                        nameable.Name = newName;
                        return nameable;
                    },
                    () =>
                    {
                        nameable.Name = oldName;
                        return nameable;
                    }, changeValueCallback);
            }

            return rect.height;
        }
    }
}
