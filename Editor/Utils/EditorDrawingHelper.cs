using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Innoactive.Hub.Training.Editors.Utils
{
    /// <summary>
    /// Helper class for drawing primitives in custom editors.
    /// </summary>
    public static class EditorDrawingHelper
    {
        private static readonly Vector2 addComponentButtonSize = new Vector2(228, 22);

        /// <summary>
        /// Default spacing between Step Inspector elements.
        /// </summary>
        public const float VerticalSpacing = 6f;

        /// <summary>
        /// Width of one indentation step in the Step Inspector.
        /// </summary>
        public const float IndentationWidth = 16f;

        /// <summary>
        /// Height of one line in the Step Inspector.
        /// </summary>
        public static float SingleLineHeight
        {
            get
            {
                return EditorGUIUtility.singleLineHeight;
            }
        }

        /// <summary>
        /// Height of slightly bigger line in the Step Inspector.
        /// </summary>
        public static float HeaderLineHeight
        {
            get
            {
                return EditorGUIUtility.singleLineHeight + 2f;
            }
        }

        /// <summary>
        /// Draw button which is similar to default "Add Component" Unity button.
        /// </summary>
        public static bool DrawAddButton(ref Rect rect, string label)
        {
            rect.height = SingleLineHeight + addComponentButtonSize.y;

            Rect buttonRect = rect;
            buttonRect.size = addComponentButtonSize;
            buttonRect.x = rect.x + (rect.width - buttonRect.width) / 2f;
            buttonRect.y = rect.y + (rect.height - buttonRect.height) / 2f;

            GUIStyle style = new GUIStyle(GUI.skin.button)
            {
                fontSize = 12
            };

            rect.height -= VerticalSpacing;
            return GUI.Button(buttonRect, new GUIContent(label), style);
        }

        /// <summary>
        /// Draw a circle.
        /// </summary>
        /// <param name="position">Center of the circle.</param>
        /// <param name="radius">Radius of the circle.</param>
        /// <param name="color">Color of the circle.</param>
        public static void DrawCircle(Vector2 position, float radius, Color color)
        {
            Handles.BeginGUI();
            {
                Color handlesColor = Handles.color;
                Handles.color = GUI.color * color;
                Handles.DrawSolidDisc(position, new Vector3(0f, 0f, 1f), radius);
                Handles.color = handlesColor;
            }
            Handles.EndGUI();
        }

        /// <summary>
        /// Draw an arrow.
        /// </summary>
        /// <param name="from">Position from which arrow begins.</param>
        /// <param name="to">Position at which arrow is pointing.</param>
        /// <param name="color">Arrow color.</param>
        /// <param name="arrowheadAngleInDegrees">How wide is the arrow's end in degrees.</param>
        /// <param name="arrowheadLength">How long is the arrow head in pixels.</param>
        public static void DrawArrow(Vector2 from, Vector2 to, Color color, float arrowheadAngleInDegrees, float arrowheadLength)
        {
            Handles.BeginGUI();
            {
                Color handlesColor = Handles.color;
                Handles.color = GUI.color * color;
                Handles.DrawSolidArc(to, new Vector3(0, 0, 1f), from - to, arrowheadAngleInDegrees / 2f, arrowheadLength);
                Handles.DrawSolidArc(to, new Vector3(0, 0, 1f), from - to, -arrowheadAngleInDegrees / 2f, arrowheadLength);
                Handles.DrawLine(from, to);
                Handles.color = handlesColor;
            }
            Handles.EndGUI();
        }

        /// <summary>
        /// Draw a rect.
        /// </summary>
        /// <param name="rect">Rect size and position to draw.</param>
        /// <param name="color">Color of the rect.</param>
        public static void DrawRect(Rect rect, Color color)
        {
            EditorGUI.DrawRect(rect, color);
        }

        /// <summary>
        /// Draw a rectangle with rounded corners.
        /// </summary>
        /// <param name="rect">Rect size and position to draw.</param>
        /// <param name="color">Color of the rect.</param>
        /// <param name="cornerRadius">Corners are drawn as quarters of a circle with radius equal to cornerRadius. If the radius is too small (less than one pixel) or too big (bigger than a half of any side), simple rect is drawn.</param>
        public static void DrawRoundedRect(Rect rect, Color color, float cornerRadius)
        {
            if (cornerRadius < 1f || cornerRadius * 2f > rect.width || cornerRadius * 2f > rect.height)
            {
                // Incorrect radius, draw simple rect.
                DrawRect(rect, color);
                return;
            }

            DrawRect(new Rect(rect.position.x + cornerRadius, rect.position.y, rect.width - cornerRadius * 2f, rect.height), color);
            DrawRect(new Rect(rect.position.x, rect.position.y + cornerRadius, rect.width, rect.height - cornerRadius * 2f), color);

            Handles.BeginGUI();
            {
                Color handlesColor = Handles.color;
                Handles.color = GUI.color * color;
                Handles.DrawSolidArc(new Vector2(rect.xMin + cornerRadius, rect.yMin + cornerRadius), new Vector3(0f, 0f, 1f), Vector2.left, 90f, cornerRadius);
                Handles.DrawSolidArc(new Vector2(rect.xMax - cornerRadius, rect.yMin + cornerRadius), new Vector3(0f, 0f, 1f), Vector2.down, 90f, cornerRadius);
                Handles.DrawSolidArc(new Vector2(rect.xMin + cornerRadius, rect.yMax - cornerRadius), new Vector3(0f, 0f, 1f), Vector2.up, 90f, cornerRadius);
                Handles.DrawSolidArc(new Vector2(rect.xMax - cornerRadius, rect.yMax - cornerRadius), new Vector3(0f, 0f, 1f), Vector2.right, 90f, cornerRadius);
                Handles.color = handlesColor;
            }
            Handles.EndGUI();
        }

        /// <summary>
        /// Draws EditorGUI.Foldout with focus area located only around triangle-shaped button.
        /// </summary>
        /// <param name="rect">Rectangle in which foldout view is drawn.</param>
        /// <param name="currentState">Are contents folded out.</param>
        /// <param name="foldoutLabel">Foldout text.</param>
        /// <param name="foldoutStyle">Foldout style.</param>
        /// <returns>result of EditorGUI.Foldout (is foldout expanded or not).</returns>
        public static bool DrawFoldoutWithReducedFocusArea(Rect rect, bool currentState, GUIContent foldoutLabel, GUIStyle foldoutStyle = null, GUIStyle labelStyle = null)
        {
            Rect labelRect = new Rect(rect.position.x + IndentationWidth, rect.position.y, rect.size.x - IndentationWidth, rect.size.y);
            Rect buttonRect = new Rect(rect.position.x, rect.position.y, IndentationWidth, SingleLineHeight);

            if (foldoutStyle == null)
            {
                foldoutStyle = EditorStyles.foldout;
            }

            if (labelStyle == null)
            {
                labelStyle = EditorStyles.label;
            }
            
            EditorGUI.LabelField(labelRect, foldoutLabel, labelStyle);
            return EditorGUI.Foldout(buttonRect, currentState, GUIContent.none, foldoutStyle);
        }

        /// <summary>
        /// Returns Rect which is one EditorGUIUtility.singleLineHeight lower and one EditorGUIUtility.singleLineHeight shorter than origin Rect.
        /// </summary>
        public static Rect GetNextLineRect(Rect origin)
        {
            return new Rect(origin.position.x, origin.position.y + SingleLineHeight, origin.size.x, origin.size.y - SingleLineHeight);
        }

        /// <summary>
        /// Truncate given text. Applies `truncatingSequence` at the end of truncated text. Space characters at the end of the line are discarded.
        /// Designed for a single-line string, no clue how well would it work with multi-line.
        /// </summary>
        /// <param name="text">Initial text.</param>
        /// <param name="style">Style in which text width is calculated.</param>
        /// <param name="maxWidth">Maximal width allowed in pixels.</param>
        /// <param name="truncatingSequence">Text to be appended at the end of the truncated text.</param>
        /// <returns>Truncated text that is not wider than maximal width.</returns>
        public static string TruncateText(string text, GUIStyle style, float maxWidth, string truncatingSequence = "...")
        {
            if (style.CalcSize(new GUIContent(text)).x > maxWidth == false)
            {
                return text;
            }

            float desiredSize = maxWidth - style.CalcSize(new GUIContent(truncatingSequence)).x;

            string truncated = "";

            while (style.CalcSize(new GUIContent(truncated + text[truncated.Length])).x < desiredSize)
            {
                truncated += text[truncated.Length];
            }

            // If last symbols were spaces, remove it. It should look like "Step 1...", not "Step 1            ...".
            while (truncated.Length > 0 && truncated[truncated.Length - 1] == ' ')
            {
                truncated = truncated.Substring(0, truncated.Length - 1);
            }

            truncated += truncatingSequence;

            // If truncating sequence is wider than maxWidth alone, truncate it.
            truncated = TruncateText(truncated, style, maxWidth, "");

            return truncated;
        }

        /// <summary>
        /// Draws a rectangle which is hollow inside.
        /// </summary>
        /// <param name="rect">Rect that defines outer bounds of rectangle.</param>
        /// <param name="color">Color of rectangle borders.</param>
        /// <param name="lineWidth">Thickness of the rect border.</param>
        public static void DrawHollowRectangle(Rect rect, Color color, float lineWidth)
        {
            if (lineWidth >= rect.width || lineWidth >= rect.height)
            {
                EditorGUI.DrawRect(rect, color);
                return;
            }

            EditorGUI.DrawRect(new Rect(rect.x, rect.y, rect.width, lineWidth), color);
            EditorGUI.DrawRect(new Rect(rect.x, rect.y, lineWidth, rect.height), color);
            EditorGUI.DrawRect(new Rect(rect.x, rect.y + rect.height - lineWidth, rect.width, lineWidth), color);
            EditorGUI.DrawRect(new Rect(rect.x + rect.width - lineWidth, rect.y, lineWidth, rect.height), color);
        }

        /// <summary>
        /// Draws a curve by connecting the given <paramref name="points"/> with lines.
        /// </summary>
        /// <param name="color">Color of the curve.</param>
        public static void DrawPolyline(IList<Vector2> points, Color color)
        {
            Vector2 lastPosition = points[0];

            color = GUI.color * color;
            Color handlesColor = Handles.color;

            for (int i = 1; i < points.Count; i++)
            {
                Handles.BeginGUI();
                {
                    Handles.color = color;
                    Handles.DrawLine(lastPosition, points[i]);
                    Handles.color = handlesColor;
                }
                Handles.EndGUI();

                lastPosition = points[i];
            }
        }
    }
}
