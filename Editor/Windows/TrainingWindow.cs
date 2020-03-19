using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Innoactive.Hub.Training.Editors.Utils;
using Innoactive.Hub.Training.Editors.Utils.Undo;
using Innoactive.Hub.Training.Editors.Configuration;
using Innoactive.Hub.Unity.Tests.Training.Editor.EditorImguiTester;

namespace Innoactive.Hub.Training.Editors.Windows
{
    /// <summary>
    /// Workflow Editor window.
    /// </summary>
    public class TrainingWindow : EditorWindow
    {
        private static TrainingWindow window;
        private ICourse activeCourse;

        [SerializeField]
        private Vector2 currentScrollPosition;

        [SerializeField]
        private byte[] temporarySerializedTraining;

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
            get { return isDirty; }
            set { isDirty = value; }
        }

        public static bool IsOpen
        {
            get { return EditorUtils.IsWindowOpened<TrainingWindow>(); }
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
        public bool SaveTraining()
        {
            if (SaveManager.SaveTrainingCourseToFile(activeCourse))
            {
                IsDirty = false;
                return true;
            }

            return false;
        }

        public void MakeTemporarySave()
        {
            if (activeCourse == null)
            {
                return;
            }

            temporarySerializedTraining = EditorConfigurator.Instance.Serializer.CourseToByteArray(activeCourse);
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
                int userConfirmation = TestableEditorElements.DisplayDialogComplex("Unsaved changes detected.", "Do you want to save the changes to a current training course?", "Save", "Cancel", "Discard");
                if (userConfirmation == 0)
                {
                    if (SaveTraining() == false)
                    {
                        return false;
                    }
                }

                if (userConfirmation == 1)
                {
                    return false;
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

            if (temporarySerializedTraining != null && temporarySerializedTraining.Length > 0)
            {
                try
                {
                    bool wasDirty = IsDirty;

                    chapterMenu = CreateInstance<TrainingMenuView>();
                    SetTrainingCourse(EditorConfigurator.Instance.Serializer.CourseFromByteArray(temporarySerializedTraining));
                    IsDirty = wasDirty;
                    temporarySerializedTraining = null;
                }
                catch (Exception e)
                {
                    Debug.LogErrorFormat("Couldn't restore the training course state.\n{0}", e.Message);
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

            Vector2 centerViewpointOnCanvas = currentScrollPosition + scrollRect.size / 2f;

            HandleEditorCommands(centerViewpointOnCanvas);
            chapterMenu.Draw();
            DrawChapterWorkflow(scrollRect);

            Repaint();
        }

        private void HandleEditorCommands(Vector2 centerViewpointOnCanvas)
        {
            if (Event.current.type != EventType.ValidateCommand)
            {
                return;
            }

            bool used = false;
            switch (Event.current.commandName)
            {
                case "Copy":
                    used = chapterRepresentation.CopySelected();
                    break;
                case "Cut":
                    used = chapterRepresentation.CutSelected();
                    break;
                case "Paste":
                    used = chapterRepresentation.Paste(centerViewpointOnCanvas);
                    break;
                case "Delete":
                case "SoftDelete":
                    used = chapterRepresentation.DeleteSelected();
                    break;
                case "Duplicate":
                    break;
                case "FrameSelected":
                    break;
                case "FrameSelectedWithLock":
                    break;
                case "SelectAll":
                    break;
                case "Find":
                    break;
                case "FocusProjectWindow":
                    break;
                default:
                    break;
            }

            if (used)
            {
                Event.current.Use();
            }
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
        }

        public void LoadTrainingCourseFromFile(string path)
        {
            if (string.IsNullOrEmpty(path) || File.Exists(path) == false)
            {
                return;
            }

            ICourse course = SaveManager.LoadTrainingCourseFromFile(path);
            string filename = Path.GetFileNameWithoutExtension(path);

            if (course.Data.Name.Equals(filename) == false)
            {
                bool userConfirmation = TestableEditorElements.DisplayDialog("Course name does not match filename.",
                    string.Format("The training course name (\"{0}\") does not match the filename (\"{1}\"). To be able to load the training course, it must be renamed to \"{1}\".", course.Data.Name, filename),
                    "Rename Course",
                    "Cancel");

                if (userConfirmation == false)
                {
                    return;
                }

                course.Data.Name = filename;
                SaveManager.SaveTrainingCourseToFile(course);
            }

            SetTrainingCourseWithUserConfirmation(course);
            IsDirty = false;
        }
    }
}
