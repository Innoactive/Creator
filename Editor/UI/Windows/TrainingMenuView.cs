using System;
using Innoactive.Creator.Core;
using Innoactive.CreatorEditor.UndoRedo;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Windows
{
    /// <summary>
    /// TrainingMenuView is shown on the left side of the <see cref="CourseWindow"/> and takes care about overall
    /// settings for the Training itself, especially chapters.
    /// </summary>
    internal class TrainingMenuView : ScriptableObject
    {
        #region Layout Constants
        public const float ExtendedMenuWidth = 330f;
        public const float MinimizedMenuWidth = ExpandButtonWidth + ChapterPaddingTop * 2f;

        public const float ExpandButtonHeight = ExpandButtonWidth;
        public const float ExpandButtonWidth = 28f;

        public const float VerticalSpace = 8f;
        public const float TabSpace = 8f;

        public const float ChapterPaddingTop = 8f;
        public const float ChapterPaddingBottom = 6f;

        public const float ButtonSize = 18f;

        private static EditorIcon deleteIcon;
        private static EditorIcon arrowUpIcon;
        private static EditorIcon arrowDownIcon;
        private static EditorIcon editIcon;
        private static EditorIcon folderIcon;
        private static EditorIcon chapterMenuExpandIcon;
        private static EditorIcon chapterMenuCollapseIcon;
        #endregion

        #region Events
        public class ChapterChangedEventArgs : EventArgs
        {
            public readonly IChapter CurrentChapter;

            public ChapterChangedEventArgs(IChapter chapter)
            {
                CurrentChapter = chapter;
            }
        }

        /// <summary>
        /// Will be called every time the selection of the chapter changes.
        /// </summary>
        [NonSerialized]
        public EventHandler<ChapterChangedEventArgs> ChapterChanged;
        #endregion

        #region Public properties
        [SerializeField]
        private bool isExtended = true;

        /// <summary>
        /// Determines if the training menu window is shown or not.
        /// </summary>
        public bool IsExtended { get; private set; }

        [SerializeField]
        private int activeChapter = 0;

        /// <summary>
        /// Returns the current active chapter.
        /// </summary>
        public IChapter CurrentChapter
        {
            get
            {
                return Course.Data.Chapters[activeChapter];
            }
        }
        #endregion

        protected ICourse Course { get; private set; }

        protected CourseWindow ParentWindow { get; private set; }

        [SerializeField]
        private Vector2 scrollPosition;

        private ChangeNamePopup changeNamePopup;
        private RenameCoursePopup renameCoursePopup;

        /// <summary>
        /// Initialises the windows with the correct training and TrainingWindow (parent).
        /// This has to be done after every time the editor reloaded the assembly (recompile).
        /// </summary>
        public void Initialise(ICourse course, CourseWindow parent)
        {
            Course = course;
            ParentWindow = parent;

            activeChapter = 0;

            if (deleteIcon == null)
            {
                LoadIcons();
            }
        }

        private void LoadIcons()
        {
            deleteIcon = new EditorIcon("icon_delete");
            arrowUpIcon = new EditorIcon("icon_arrow_up");
            arrowDownIcon = new EditorIcon("icon_arrow_down");
            editIcon = new EditorIcon("icon_edit");
            folderIcon = new EditorIcon("icon_folder");
            chapterMenuExpandIcon = new EditorIcon("icon_expand_chapter");
            chapterMenuCollapseIcon = new EditorIcon("icon_collapse_chapter");
        }

        /// <summary>
        /// Draws the training menu.
        /// </summary>
        public void Draw()
        {
            IsExtended = isExtended;
            GUILayout.BeginArea(new Rect(0f, 0f, IsExtended ? ExtendedMenuWidth : MinimizedMenuWidth, ParentWindow.position.size.y));
            {
                if (EditorGUIUtility.isProSkin)
                {
                    EditorColorUtils.SetBackgroundColor(Color.black);
                }

                GUILayout.BeginVertical("box");
                {
                    DrawExtendToggle();

                    EditorColorUtils.ResetBackgroundColor();

                    Vector2 deltaPosition = GUILayout.BeginScrollView(scrollPosition);
                    {
                        if (IsExtended)
                        {
                            DrawHeader();
                            DrawChapterList();
                            AddChapterButton();
                        }
                    }
                    GUILayout.EndScrollView();

                    if (changeNamePopup == null || changeNamePopup.IsClosed)
                    {
                        scrollPosition = deltaPosition;
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndArea();
        }

        #region Training Menu Draw
        private void DrawHeader()
        {
            GUILayout.Space(VerticalSpace);
            GUILayout.Space(VerticalSpace);

            GUILayout.BeginHorizontal();
            {
                GUIStyle labelStyle = new GUIStyle(EditorStyles.boldLabel);
                GUIContent labelContent = new GUIContent("Course Name:");
                EditorGUILayout.LabelField(labelContent, labelStyle, GUILayout.Width(labelStyle.CalcSize(labelContent).x));

                GUIStyle nameStyle = new GUIStyle(EditorStyles.label) { wordWrap = true };
                GUIContent nameContent = new GUIContent(Course.Data.Name, Course.Data.Name);

                if (renameCoursePopup == null || renameCoursePopup.IsClosed)
                {
                    EditorGUILayout.LabelField(Course.Data.Name, nameStyle, GUILayout.Width(180f), GUILayout.Height(nameStyle.CalcHeight(nameContent, 180f)));Rect labelPosition = GUILayoutUtility.GetLastRect();
                    if (FlatIconButton(editIcon.Texture))
                    {
                        labelPosition = new Rect(labelPosition.x + ParentWindow.position.x - 2, labelPosition.height + labelPosition.y + ParentWindow.position.y + 4 + ExpandButtonHeight, labelPosition.width, labelPosition.height);
                        renameCoursePopup = RenameCoursePopup.Open(Course, labelPosition, scrollPosition);
                    }
                }
            }
            GUILayout.EndHorizontal();
        }

        private void DrawChapterList()
        {
            GUILayout.Space(VerticalSpace);
            GUILayout.BeginHorizontal();
            {
                GUIContent content = new GUIContent("  Chapters", folderIcon.Texture);
                GUILayout.Label(content, EditorStyles.boldLabel, GUILayout.MaxHeight(24f));
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(VerticalSpace);

            for (int position = 0; position < Course.Data.Chapters.Count; position++)
            {
                DrawChapter(position);
            }
        }

        private void DrawChapter(int position)
        {
            EditorColorUtils.ResetBackgroundColor();

            GUIStyle chapterBoxStyle = GUI.skin.GetStyle("Label");
            if (position == activeChapter)
            {
                chapterBoxStyle = GUI.skin.GetStyle("selectionRect");
            }

            chapterBoxStyle.margin = new RectOffset(0, 0, 4, 4);
            chapterBoxStyle.padding = new RectOffset(2, 2, 2, 2);

            GUILayout.BeginHorizontal(chapterBoxStyle);
            {
                EditorColorUtils.ResetBackgroundColor();
                GUILayout.BeginVertical();
                {
                    GUILayout.Space(ChapterPaddingTop);
                    DrawChapterContent(position);
                    GUILayout.Space(ChapterPaddingBottom);
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

            Rect rect = GUILayoutUtility.GetLastRect();
            if (rect.Contains(Event.current.mousePosition))
            {
                if (Event.current.GetTypeForControl(GUIUtility.GetControlID(FocusType.Passive)) == EventType.MouseDown)
                {
                    activeChapter = position;
                    EmitChapterChanged();

                    Event.current.Use();
                }
            }
        }

        private void DrawChapterContent(int position)
        {
            bool isActiveChapter = (activeChapter == position);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(TabSpace);
                if (isActiveChapter)
                {
                    GUILayout.Space(ChapterPaddingTop);
                }

                EditorColorUtils.SetTransparency(isActiveChapter ? 0.8f : 0.25f);
                GUILayout.Label(folderIcon.Texture, GUILayout.Width(ButtonSize), GUILayout.Height(ButtonSize));
                EditorColorUtils.ResetColor();

                GUIStyle style = new GUIStyle(GUI.skin.label);
                style.alignment = TextAnchor.UpperLeft;
                GUILayout.Label(Course.Data.Chapters[position].Data.Name, style, GUILayout.Width(160f), GUILayout.Height(20f));
                Rect labelPosition = GUILayoutUtility.GetLastRect();

                GUILayout.FlexibleSpace();
                AddMoveUpButton(position);
                AddMoveDownButton(position);
                AddRemoveButton(position, Course.Data.Chapters.Count == 1);
                AddRenameButton(position, labelPosition);

                GUILayout.Space(4);
            }
            GUILayout.EndHorizontal();
        }

        private void DrawExtendToggle()
        {
            Rect buttonPosition = new Rect((IsExtended ? ExtendedMenuWidth : MinimizedMenuWidth) - ExpandButtonWidth - ChapterPaddingTop, ChapterPaddingTop, ExpandButtonWidth, ExpandButtonHeight);
            GUIStyle style = new GUIStyle();
            style.imagePosition = ImagePosition.ImageOnly;
            if (GUI.Button(buttonPosition, IsExtended ? new GUIContent(chapterMenuCollapseIcon.Texture) : new GUIContent(chapterMenuExpandIcon.Texture), style))
            {
                isExtended = !isExtended;
            }
            GUILayout.Space(ExpandButtonHeight);
        }
        #endregion

        #region Button Actions
        private void AddMoveUpButton(int position)
        {
            if (FlatIconButton(arrowUpIcon.Texture))
            {
                if (position > 0)
                {
                    RevertableChangesHandler.Do(new CourseCommand(
                        // ReSharper disable once ImplicitlyCapturedClosure
                        () =>
                        {
                            MoveChapterUp(position);
                        },
                        // ReSharper disable once ImplicitlyCapturedClosure
                        () =>
                        {
                            MoveChapterDown(position - 1);
                        }
                    ));
                }
            }
        }

        private void AddMoveDownButton(int position)
        {
            if (FlatIconButton(arrowDownIcon.Texture))
            {
                if (position + 1 < Course.Data.Chapters.Count)
                {
                    RevertableChangesHandler.Do(new CourseCommand(
                        // ReSharper disable once ImplicitlyCapturedClosure
                        () =>
                        {
                            MoveChapterDown(position);
                        },
                        // ReSharper disable once ImplicitlyCapturedClosure
                        () =>
                        {
                            MoveChapterUp(position + 1);
                        }
                    ));
                }
            }
        }

        private void AddRenameButton(int position, Rect labelPosition)
        {
            if (FlatIconButton(editIcon.Texture))
            {
                labelPosition = new Rect(labelPosition.x + ParentWindow.position.x - 2, labelPosition.height + labelPosition.y + ParentWindow.position.y + 4 + ExpandButtonHeight, labelPosition.width, labelPosition.height);
                changeNamePopup = ChangeNamePopup.Open(Course.Data.Chapters[position].Data, labelPosition, scrollPosition);
            }
        }

        private void AddRemoveButton(int position, bool isDisabled)
        {
            EditorGUI.BeginDisabledGroup(isDisabled);
            {
                if (FlatIconButton(deleteIcon.Texture))
                {
                    IChapter chapter = Course.Data.Chapters[position];
                    bool isDeleteTriggered = EditorUtility.DisplayDialog($"Delete Chapter '{chapter.Data.Name}'",
                        $"Do you really want to delete chapter '{chapter.Data.Name}'? You will lose all steps stored there.", "Delete",
                        "Cancel");

                    if (isDeleteTriggered)
                    {
                        RevertableChangesHandler.Do(new CourseCommand(
                            // ReSharper disable once ImplicitlyCapturedClosure
                            () =>
                            {
                                RemoveChapterAt(position);
                            },
                            // ReSharper disable once ImplicitlyCapturedClosure
                            () =>
                            {
                                Course.Data.Chapters.Insert(position, chapter);
                                if (position == activeChapter)
                                {
                                    EmitChapterChanged();
                                }
                            }
                        ));
                    }
                }
            }
            EditorGUI.EndDisabledGroup();
        }

        private void AddChapterButton()
        {
            GUILayout.Space(VerticalSpace);
            GUILayout.Space(VerticalSpace);

            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("+Add Chapter", GUILayout.Width(128), GUILayout.Height(32)))
                {
                    RevertableChangesHandler.Do(new CourseCommand(
                        // ReSharper disable once ImplicitlyCapturedClosure
                        () =>
                        {
                            Course.Data.Chapters.Add(EntityFactory.CreateChapter($"Chapter {(Course.Data.Chapters.Count + 1)}"));
                            activeChapter = Course.Data.Chapters.Count - 1;
                            EmitChapterChanged();
                        },
                        // ReSharper disable once ImplicitlyCapturedClosure
                        () =>
                        {
                            RemoveChapterAt(Course.Data.Chapters.Count - 1);
                        }
                    ));
                }

                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(VerticalSpace);
            GUILayout.Space(VerticalSpace);
        }
        #endregion

        #region Private helpers
        private void MoveChapterUp(int position)
        {
            IChapter chapter = Course.Data.Chapters[position];
            Course.Data.Chapters.RemoveAt(position);
            Course.Data.Chapters.Insert(position - 1, chapter);

            if (activeChapter == position)
            {
                activeChapter--;
            }
            else if (activeChapter == position - 1)
            {
                activeChapter++;
            }
        }

        private void MoveChapterDown(int position)
        {
            IChapter chapter = Course.Data.Chapters[position];
            Course.Data.Chapters.RemoveAt(position);
            Course.Data.Chapters.Insert(position + 1, chapter);

            if (activeChapter == position)
            {
                activeChapter++;
            }
            else if (activeChapter == position + 1)
            {
                activeChapter--;
            }
        }

        private static bool FlatIconButton(Texture icon)
        {
            EditorColorUtils.SetTransparency(0.25f);
            bool isTriggered = GUILayout.Button(icon, EditorStyles.label, GUILayout.Width(ButtonSize), GUILayout.Height(ButtonSize));
            // Creating a highlight effect if the mouse is currently hovering the button.
            Rect buttonRect = GUILayoutUtility.GetLastRect();
            if (buttonRect.Contains(Event.current.mousePosition))
            {
                EditorColorUtils.SetTransparency(0.5f);
                GUI.Label(buttonRect, icon);
            }

            EditorColorUtils.ResetColor();
            return isTriggered;
        }

        private void EmitChapterChanged()
        {
            if (ChapterChanged != null)
            {
                ChapterChanged.Invoke(this, new ChapterChangedEventArgs(CurrentChapter));
            }
        }

        private void RemoveChapterAt(int position)
        {
            if (position > 0)
            {
                Course.Data.Chapters.RemoveAt(position);
            }
            else if (Course.Data.Chapters.Count > 1)
            {
                Course.Data.Chapters.RemoveAt(position);
            }

            if (position < activeChapter)
            {
                activeChapter--;
            }

            if (activeChapter == position)
            {
                if (Course.Data.Chapters.Count == position)
                {
                    activeChapter--;
                }

                EmitChapterChanged();
            }
        }
        #endregion
    }
}
