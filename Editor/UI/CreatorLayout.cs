using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Innoactive.Creator.Core.Editor.UI
{
    public static class CreatorLayout
    {
        public static void DrawLink(string text, string url, int intend = CreatorEditorStyles.Indent)
        {
            DrawLink(text, () =>
            {
                try
                {
                    Process.Start(url);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
            }, intend);
        }

        public static void DrawLink(string text, Action action, int intend = CreatorEditorStyles.Indent)
        {
            if (GUILayout.Button(text, CreatorEditorStyles.ApplyIdent(CreatorEditorStyles.Link, intend)))
            {
                action.Invoke();
            }

            Rect buttonRect = GUILayoutUtility.GetLastRect();
            GUI.Label(new Rect(buttonRect.x, buttonRect.y + 1, buttonRect.width, buttonRect.height), new String('_', 256), CreatorEditorStyles.ApplyIdent(CreatorEditorStyles.Link, intend));
            EditorGUIUtility.AddCursorRect(buttonRect, MouseCursor.Link);
        }
    }
}
