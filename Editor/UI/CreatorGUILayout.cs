using System;
using System.Diagnostics;
using Innoactive.CreatorEditor.UI;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Innoactive.CreatorEditor.UI
{
    /// <summary>
    /// Layout extension for the creator. This class might
    /// </summary>
    public static class CreatorGUILayout
    {
        /// <summary>
        /// Draws a clickable link which opens a website.
        /// </summary>
        /// <param name="text">Text to be displayed</param>
        /// <param name="url">url to be opened inside the browser</param>
        /// <param name="intend">Intend on the left</param>
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

        /// <summary>
        /// Draws a clickable link which looks like a hyperlink.
        /// </summary>
        /// <param name="text">Text to be displayed</param>
        /// <param name="action">action done on click</param>
        /// <param name="intend">Intend on the left</param>
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
