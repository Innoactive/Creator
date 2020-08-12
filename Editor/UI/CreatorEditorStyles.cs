using System;
using UnityEditor;
using UnityEngine;

namespace Innoactive.Creator.Core.Editor.UI
{
    public static class CreatorEditorStyles
    {
        public const int BaseMargin = 2;

        public const int Indent = 12;
        public const int IndentLarge = Indent * 3;

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
                    title.margin = new RectOffset(Indent, BaseMargin, Indent, Indent);
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
                    header.margin = new RectOffset(Indent, BaseMargin, Indent, BaseMargin);
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
                    paragraph.margin = new RectOffset(IndentLarge, BaseMargin, BaseMargin, BaseMargin);
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
                    toggle.padding = new RectOffset(Indent + BaseMargin + 6, BaseMargin, 0, 2); // this only affects the text
                    toggle.margin = new RectOffset(Indent + BaseMargin * 2, BaseMargin, 0, 1); // this affects the position
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
                    subText.margin = new RectOffset(Indent, BaseMargin, 0, 0);
                }

                return subText;
            }
        }

        private static GUIStyle label;
        public static GUIStyle Label
        {
            get
            {
                if (label == null)
                {
                    label = new GUIStyle(GUI.skin.label);
                    label.alignment = TextAnchor.MiddleLeft;
                    label.fontSize = 13;
                    label.richText = true;
                    label.clipping = TextClipping.Clip;
                    label.margin = new RectOffset(Indent, BaseMargin, BaseMargin, BaseMargin);
                }

                return label;
            }
        }


        private static GUIStyle link;
        public static GUIStyle Link
        {
            get
            {
                if (link == null)
                {
                    link = new GUIStyle(EditorStyles.linkLabel);
                    link.alignment = TextAnchor.MiddleLeft;
                    link.fontSize = 13;
                    link.richText = true;
                    link.clipping = TextClipping.Clip;
                    link.margin = new RectOffset(Indent, BaseMargin, BaseMargin, BaseMargin);
                }

                return link;
            }
        }

        public static GUIStyle ApplyIdent(GUIStyle style, int ident = Indent)
        {
            return new GUIStyle(style) { margin = new RectOffset(ident, style.margin.right, style.margin.top, style.margin.bottom) };
        }

        public static GUIStyle ApplyMargin(GUIStyle style, RectOffset margin)
        {
            return new GUIStyle(style) { margin = margin };
        }
    }
}
