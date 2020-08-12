using UnityEditor;
using UnityEngine;

namespace Innoactive.Creator.Core.Editor.UI
{
    public static class CreatorEditorStyles
    {

        public const int BasePadding = 2;

        public const int Ident = 12;
        public const int IdentLarge = Ident * 3;

        public static Color HighlightTextColor = EditorGUIUtility.isProSkin ? Color.white : Color.black;

        private static GUIStyle title;
        public static GUIStyle Title
        {
            get
            {
                if (title == null)
                {
                    title = new GUIStyle(EditorStyles.largeLabel);
                    title.fontSize = 24;
                    title.fontStyle = FontStyle.Bold;
                    title.normal.textColor = HighlightTextColor;
                    title.padding = new RectOffset(Ident, BasePadding, Ident, Ident);
                }

                return title;
            }
        }

        private static GUIStyle header;
        public static GUIStyle Header
        {
            get
            {
                if (header == null)
                {
                    header = new GUIStyle(EditorStyles.largeLabel);
                    header.fontSize = 16;
                    header.fontStyle = FontStyle.Bold;
                    header.normal.textColor = HighlightTextColor;
                    header.padding = new RectOffset(Ident, BasePadding, Ident, BasePadding);
                }

                return header;
            }
        }

        private static GUIStyle paragraph;
        public static GUIStyle Paragraph
        {
            get
            {
                if (paragraph == null)
                {
                    paragraph = new GUIStyle(GUI.skin.label);
                    paragraph.alignment = TextAnchor.UpperLeft;
                    paragraph.fontSize = 13;
                    paragraph.richText = true;
                    paragraph.clipping = TextClipping.Clip;
                    paragraph.wordWrap = true;
                    paragraph.padding = new RectOffset(IdentLarge, BasePadding, BasePadding, BasePadding);
                }

                return paragraph;
            }
        }

        public static GUIStyle ApplyIdent(GUIStyle style, int ident = Ident)
        {
            return new GUIStyle(style) { padding = new RectOffset(ident, style.padding.right, style.padding.top, style.padding.bottom) };
        }

        public static GUIStyle ApplyPadding(GUIStyle style, RectOffset padding)
        {
            return new GUIStyle(style) { padding = padding };
        }
    }
}
