using System;
using Common.Logging;
using Innoactive.Hub.Training.Editors.Utils;
using Innoactive.Hub.Training.Editors.Utils.Undo;
using Innoactive.Hub.Training.Utils.Serialization;
using Innoactive.Hub.Unity.Tests.Training.Editor.EditorImguiTester;
using UnityEditor;
using UnityEngine;
using LogManager = Innoactive.Hub.Logging.LogManager;

namespace Innoactive.Hub.Training.Editors.Windows
{
    /// <summary>
    /// Workflow Editor window.
    /// </summary>
    public class TrainingWindow : EditorWindow
    {
        private static readonly ILog logger = LogManager.GetLogger<TrainingWindow>();
        private static TrainingWindow window;
        private ICourse activeCourse;

        [SerializeField]
        private Vector2 currentScrollPosition;

        [SerializeField]
        private string temporarySerializedTraining;

        [SerializeField]
        private bool isDirty;

        private EditorIcon titleIcon;

        [SerializeField]
        private TrainingMenuView chapterMenu;

        private ChapterRepresentation chapterRepresentation;

        /// <summary>
        /// Determines if the training is in a different state than the last saved version.
        /// </summary>
        public bool IsDirty
        {
            get
            {
                return isDirty;
            }
            set
            {
                isDirty = value;
            }
        }

        public static bool IsOpen
        {
            get
            {
                return EditorUtils.IsWindowOpened<TrainingWindow>();
            }
        }

        public static TrainingWindow GetWindow()
        {
            if (IsOpen == false)
            {
                window = GetWindow<TrainingWindow>();
                window.minSize = new Vector2(400f, 100f);
            }

            return window;
        }

        /// <summary>
        /// Saves the training. Will open the save as dialog only, if no path exists.
        /// Otherwise, this will silently overwrite the existing version of the training.
        /// </summary>
        public void SaveTraining()
        {
            if (SaveManager.SaveTrainingCourseToFile(activeCourse))
            {
                IsDirty = false;
            }
        }

        public void MakeTemporarySave()
        {
            if (activeCourse == null)
            {
                return;
            }

            temporarySerializedTraining = JsonTrainingSerializer.Serialize(activeCourse);
        }

        /// <summary>
        /// Sets a new course in the training workflow editor window after asking to save the current one, if needed.
        /// </summary>
        /// <param name="course">New course to set.</param>
        /// <returns>True if the course is set, false if the user cancels the operation.</returns>
        public bool SetTrainingCourseWithUserConfirmation(ICourse course)
        {
            if (activeCourse != null && IsDirty)
            {
                if (IsDirty)
                {
                    int userConfirmation = TestableEditorElements.DisplayDialogComplex("Unsaved changes detected.", "Do you want to save the changes to a current training course?", "Save", "Cancel", "Discard");
                    if (userConfirmation == 0)
                    {
                        SaveTraining();
                        if (activeCourse.Data.Name.Equals(course.Data.Name))
                        {
                            return true;
                        }
                    }
                    else if (userConfirmation == 1)
                    {
                        return false;
                    }
                }
            }

            SetTrainingCourse(course);
            return true;
        }

        public void SetTrainingCourse(ICourse course)
        {
            RevertableChangesHandler.FlushStack();
            activeCourse = course;

            chapterMenu.Initialise(course, this);
            chapterMenu.ChapterChanged += (sender, args) =>
            {
                chapterRepresentation.SetChapter(args.CurrentChapter);
            };

            chapterRepresentation.SetChapter(course.Data.FirstChapter);

            IsDirty = true;
        }

        public ICourse GetTrainingCourse()
        {
            return activeCourse;
        }

        public void RefreshChapterRepresentation()
        {
            if (activeCourse != null)
            {
                chapterRepresentation.SetChapter(chapterMenu.CurrentChapter);
            }
        }

        public IChapter GetChapter()
        {
            if (activeCourse == null)
            {
                return null;
            }

            return chapterMenu.CurrentChapter;
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        private void OnEnable()
        {
            wantsMouseMove = true;
            if (chapterMenu == null)
            {
                chapterMenu = CreateInstance<TrainingMenuView>();
            }

            if (chapterRepresentation == null)
            {
                chapterRepresentation = new ChapterRepresentation();
                chapterRepresentation.Graphics.Canvas.PointerDrag += (o, eventArgs) => currentScrollPosition -= eventArgs.PointerDelta;
            }

            if (titleIcon == null)
            {
                titleIcon = new EditorIcon("icon_training_editor");
            }

            if (window != null && window != this)
            {
                window.Close();
            }

            window = this;

            if (string.IsNullOrEmpty(temporarySerializedTraining) == false)
            {
                try
                {
                    bool wasDirty = IsDirty;

                    chapterMenu = CreateInstance<TrainingMenuView>();
                    SetTrainingCourse(JsonTrainingSerializer.Deserialize(temporarySerializedTraining));
                    IsDirty = wasDirty;
                    temporarySerializedTraining = null;
                }
                catch (Exception e)
                {
                    logger.Error("Couldn't restore the training course state.", e);
                }
            }
        }

        private void SetTabName()
        {
            string tabName = "Training";
            if (IsDirty)
            {
                tabName += "*";
            }

            titleContent = new GUIContent(tabName, titleIcon.Texture);
        }

        private void OnGUI()
        {
            if (activeCourse == null)
            {
                return;
            }

            SetTabName();

            DrawUnsavedChangesIndicator();

            float width = chapterMenu.IsExtended ? TrainingMenuView.ExtendedMenuWidth : TrainingMenuView.MinimizedMenuWidth;
            Rect scrollRect = EditorDrawingHelper.GetNextLineRect(new Rect(width, 0f, position.size.x - width, position.size.y));

            chapterMenu.Draw();
            DrawChapterWorkflow(scrollRect);

            Repaint();
        }

        private void DrawChapterWorkflow(Rect scrollRect)
        {
            currentScrollPosition = GUI.BeginScrollView(new Rect(scrollRect.position, scrollRect.size), currentScrollPosition - chapterRepresentation.BoundingBox.min, chapterRepresentation.BoundingBox, true, true) + chapterRepresentation.BoundingBox.min;
            {
                Rect controlRect = new Rect(currentScrollPosition, scrollRect.size);
                chapterRepresentation.HandleEvent(Event.current, controlRect);
            }
            GUI.EndScrollView();
        }

        private void DrawUnsavedChangesIndicator()
        {
            if (IsDirty)
            {
                GUIContent isTrainingSaved = new GUIContent("Unsaved changes");
                GUIStyle isTrainingSavedStyle = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleRight,
                    fontStyle = FontStyle.Bold,
                    normal = { textColor = Color.red }
                };
                EditorGUI.LabelField(new Rect(position.width / 2f - 4f, 4f, position.width / 2f, EditorGUIUtility.singleLineHeight), isTrainingSaved, isTrainingSavedStyle);
            }
        }

        private void OnDestroy()
        {
            MakeTemporarySave();
            StepWindow.HideInspector();
        }

        public void LoadTrainingCourseFromFile(string path)
        {
            SetTrainingCourseWithUserConfirmation(SaveManager.LoadTrainingCourseFromFile(path));
            IsDirty = false;
        }
    }
}
