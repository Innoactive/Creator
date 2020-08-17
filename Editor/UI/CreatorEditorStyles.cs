using UnityEditor;
using UnityEngine;

namespace Innoactive.Creator.Core.Editor.UI
{
    public static class CreatorEditorStyles
    {
        public const int BaseMargin = 2;

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
                    title.margin = new RectOffset(Ident, BaseMargin, Ident, Ident);
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
                    header.margin = new RectOffset(Ident, BaseMargin, Ident, BaseMargin);
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
                    paragraph.margin = new RectOffset(IdentLarge, BaseMargin, BaseMargin, BaseMargin);
                }

                return paragraph;
            }
        }

        private static GUIStyle toggle;
        public static GUIStyle Toggle
        {
            get
            {
                if (toggle == null)
                {
                    toggle = new GUIStyle(EditorStyles.toggle);
                    toggle.fontSize = Paragraph.fontSize;
                    toggle.padding = new RectOffset(Ident + BaseMargin + 6, BaseMargin, 0, 2); // this only affects the text
                    toggle.margin = new RectOffset(Ident + BaseMargin * 2, BaseMargin, 0, 1); // this affects the position
                }

                return toggle;
            }
        }

        private static GUIStyle subText;
        public static GUIStyle SubText
        {
            get
            {
                if (subText == null)
                {
                    subText = new GUIStyle(EditorStyles.miniLabel);
                    subText.padding = new RectOffset(0, 0, 0, 0);
                    subText.margin = new RectOffset(Ident, BaseMargin, 0, 0);
                }

                return subText;
            }
        }

        public static GUIStyle ApplyIdent(GUIStyle style, int ident = Ident)
        {
            return new GUIStyle(style) { margin = new RectOffset(ident, style.margin.right, style.margin.top, style.margin.bottom) };
        }

        public static GUIStyle ApplyMargin(GUIStyle style, RectOffset margin)
        {
            return new GUIStyle(style) { margin = margin };
        }
    }
}
