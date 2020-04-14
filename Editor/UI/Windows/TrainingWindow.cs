using Innoactive.Creator.Core;
using Innoactive.CreatorEditor.UndoRedo;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Windows
{
    /// <summary>
    /// Workflow Editor window.
    /// </summary>
    public class TrainingWindow : EditorWindow
    {
        private static TrainingWindow window;

        [SerializeField]
        private Vector2 currentScrollPosition;

        [SerializeField]
        private string lastEditedCourse;

        private EditorIcon titleIcon;

        [SerializeField]
        private TrainingMenuView chapterMenu;

        private ChapterRepresentation chapterRepresentation;

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

        private void SetTrainingCourse(ICourse course)
        {
            lastEditedCourse = course?.Data.Name;

            RevertableChangesHandler.FlushStack();
            chapterRepresentation.SetChapter(course?.Data.FirstChapter);
        }

        public void RefreshChapterRepresentation()
        {
            if (CourseAssetManager.TrackedCourse == null)
            {
                return;
            }

            if (chapterRepresentation == null)
            {
                chapterRepresentation = new ChapterRepresentation();
            }

            chapterRepresentation.SetChapter(chapterMenu.CurrentChapter);
        }


        public IChapter GetChapter()
        {
            return CourseAssetManager.TrackedCourse == null ? null : chapterMenu.CurrentChapter;
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        private void OnEnable()
        {
            if (titleIcon == null)
            {
                titleIcon = new EditorIcon("icon_training_editor");
            }

            wantsMouseMove = true;

            if (chapterMenu == null)
            {
                chapterMenu = CreateInstance<TrainingMenuView>();
                chapterMenu.Initialise(this);
            }

            chapterMenu.ChapterChanged += (sender, args) =>
            {
                chapterRepresentation.SetChapter(args.CurrentChapter);
            };

            if (lastEditedCourse != null && CourseAssetManager.TrackedCourse == null)
            {
                CourseAssetManager.Track(lastEditedCourse);
                RefreshChapterRepresentation();
            }

            if (chapterRepresentation == null)
            {
                chapterRepresentation = new ChapterRepresentation();
                chapterRepresentation.SetChapter(CourseAssetManager.TrackedCourse?.Data.FirstChapter);
                chapterRepresentation.Graphics.Canvas.PointerDrag += (o, eventArgs) => currentScrollPosition -= eventArgs.PointerDelta;
            }

            if (window != null && window != this)
            {
                window.Close();
            }

            window = this;
        }

        private void SetTabName()
        {
            const string tabName = "Training";

            titleContent = new GUIContent(tabName, titleIcon.Texture);
        }

        private void OnGUI()
        {
            if (CourseAssetManager.TrackedCourse?.Data.Name != lastEditedCourse)
            {
                SetTrainingCourse(CourseAssetManager.TrackedCourse);
            }

            if (CourseAssetManager.TrackedCourse == null)
            {
                return;
            }

            SetTabName();

            float width = chapterMenu.IsExtended ? TrainingMenuView.ExtendedMenuWidth : TrainingMenuView.MinimizedMenuWidth;
            Rect scrollRect = EditorDrawingHelper.GetNextLineRect(new Rect(width, 0f, position.size.x - width, position.size.y));

            Vector2 centerViewpointOnCanvas = currentScrollPosition + scrollRect.size / 2f;

            HandleEditorCommands(centerViewpointOnCanvas);
            chapterMenu.Draw();
            DrawChapterWorkflow(scrollRect);

            Repaint();

            lastEditedCourse = CourseAssetManager.TrackedCourse?.Data.Name;
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
    }
}
