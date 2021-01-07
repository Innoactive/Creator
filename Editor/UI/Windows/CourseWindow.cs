using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using Innoactive.Creator.Core;
using Innoactive.CreatorEditor.UndoRedo;
using Innoactive.Creator.Core.Configuration;
using Innoactive.CreatorEditor.Configuration;

namespace Innoactive.CreatorEditor.UI.Windows
{
    /// <summary>
    /// This class draws the Workflow window..
    /// </summary>
    public class CourseWindow : EditorWindow
    {
        private ICourse activeCourse;

        [SerializeField]
        private Vector2 currentScrollPosition;

        private EditorIcon titleIcon;

        [SerializeField]
        private TrainingMenuView chapterMenu;

        private ChapterRepresentation chapterRepresentation;

        private bool isPanning;

        private float zoomValue = 1f;
        private const float zoomMin = 0.5f;
        private const float zoomMax = 2.0f;

        /// <summary>
        /// Sets the <paramref name="course"/> to be displayed and edited in this window.
        /// </summary>
        public void SetCourse(ICourse course)
        {
            RevertableChangesHandler.FlushStack();

            activeCourse = course;

            if (course == null)
            {
                return;
            }

            chapterMenu.Initialise(course, this);
            chapterMenu.ChapterChanged += (sender, args) =>
            {
                chapterRepresentation.SetChapter(args.CurrentChapter);
            };

            chapterRepresentation.SetChapter(course.Data.FirstChapter);
        }

        /// <summary>
        /// Returns currently edited course.
        /// </summary>
        internal ICourse GetCourse()
        {
            return activeCourse;
        }

        /// <summary>
        /// Updates the chapter representation to the selected chapter.
        /// </summary>
        internal void RefreshChapterRepresentation()
        {
            if (activeCourse != null)
            {
                chapterRepresentation.SetChapter(chapterMenu.CurrentChapter);
            }
        }

        /// <summary>
        /// Returns currently selected chapter.
        /// </summary>
        internal IChapter GetChapter()
        {
            return activeCourse == null ? null : chapterMenu.CurrentChapter;
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
 #if CREATOR_PRO
                chapterRepresentation = new ProChapterRepresentation();
 #else
                chapterRepresentation = new ChapterRepresentation();
 #endif
                chapterRepresentation.Graphics.Canvas.PointerDrag += (o, eventArgs) => currentScrollPosition -= eventArgs.PointerDelta;
            }

            if (titleIcon == null)
            {
                titleIcon = new EditorIcon("icon_training_editor");
            }

            EditorSceneManager.newSceneCreated += OnNewScene;
            EditorSceneManager.sceneOpened += OnSceneOpened;
            GlobalEditorHandler.CourseWindowOpened(this);
        }

        private void OnDestroy()
        {
            EditorSceneManager.newSceneCreated -= OnNewScene;
            EditorSceneManager.sceneOpened -= OnSceneOpened;
            GlobalEditorHandler.CourseWindowClosed(this);
        }

        private void SetTabName()
        {
            titleContent = new GUIContent("Workflow", titleIcon.Texture);
        }

        private void OnGUI()
        {
            if (activeCourse == null)
            {
                return;
            }

            SetTabName();

            float width = chapterMenu.IsExtended ? TrainingMenuView.ExtendedMenuWidth : TrainingMenuView.MinimizedMenuWidth;
            Rect scrollRect = new Rect(width, 0f, position.size.x - width, position.size.y);

            Vector2 centerViewpointOnCanvas = currentScrollPosition + scrollRect.size / 2f;

            HandleEditorCommands(centerViewpointOnCanvas);
            chapterMenu.Draw();
            DrawChapterWorkflow(scrollRect);
        }

        private void OnFocus()
        {
            if (EditorConfigurator.Instance.Validation.IsAllowedToValidate() && activeCourse != null)
            {
                EditorConfigurator.Instance.Validation.Validate(activeCourse.Data, GlobalEditorHandler.GetCurrentCourse());
            }
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
            Event current = Event.current;

            if (current.type == EventType.MouseDown && current.button == 2)
            {
                isPanning = true;
            }
            else if (current.type == EventType.MouseUp && current.button == 2)
            {
                isPanning = false;
            }

            if (isPanning && current.type == EventType.MouseDrag)
            {
                currentScrollPosition -= current.delta / zoomValue;
                current.Use();
            }

            if (current.type == EventType.ScrollWheel && current.control)
            {
                Vector2 currentMousePositionZoomCoordinates = (current.mousePosition - new Vector2(scrollRect.xMin, scrollRect.yMin)) / zoomValue + currentScrollPosition;
                float oldZoom = zoomValue;
                zoomValue += -current.delta.y / 150.0f;
                zoomValue = Mathf.Clamp(zoomValue, zoomMin, zoomMax);
                currentScrollPosition += (currentMousePositionZoomCoordinates - currentScrollPosition) - (oldZoom / zoomValue) * (currentMousePositionZoomCoordinates - currentScrollPosition);

                current.Use();
            }

            Rect clippedArea = ScaleRectSize(scrollRect, 1.0f / zoomValue, new Vector2(scrollRect.xMin, scrollRect.yMin));
            clippedArea.y += 21f;

            GUI.EndGroup();
            currentScrollPosition = GUI.BeginScrollView(new Rect(clippedArea.position, clippedArea.size), currentScrollPosition - chapterRepresentation.BoundingBox.min, chapterRepresentation.BoundingBox, true, true, GUIStyle.none, GUIStyle.none) + chapterRepresentation.BoundingBox.min;
            {
                Matrix4x4 prevGuiMatrix = GUI.matrix;
                Matrix4x4 translation = Matrix4x4.TRS(new Vector2(clippedArea.xMin, clippedArea.yMin), Quaternion.identity, Vector3.one);
                Matrix4x4 scale = Matrix4x4.Scale(new Vector3(zoomValue, zoomValue, 1.0f));
                GUI.matrix = translation * scale * translation.inverse * GUI.matrix;

                Rect controlRect = new Rect(currentScrollPosition, clippedArea.size);
                chapterRepresentation.HandleEvent(current, controlRect);

                if (current.type == EventType.Used || isPanning)
                {
                    Repaint();
                }

                GUI.matrix = prevGuiMatrix;
            }
            GUI.EndScrollView();
            GUI.BeginGroup(new Rect(0.0f, 21f, Screen.width, Screen.height));
        }

        private static Rect ScaleRectSize(Rect rect, float scale, Vector2 pivotPoint)
        {
            Rect result = rect;
            result.x -= pivotPoint.x;
            result.y -= pivotPoint.y;
            result.xMin *= scale;
            result.xMax *= scale;
            result.yMin *= scale;
            result.yMax *= scale;
            result.x += pivotPoint.x;
            result.y += pivotPoint.y;
            return result;
        }

        private void OnSceneOpened(Scene scene, OpenSceneMode mode)
        {
            if (RuntimeConfigurator.Exists == false)
            {
                Close();
            }
        }

        private void OnNewScene(Scene scene, NewSceneSetup setup, NewSceneMode mode)
        {
            Close();
        }
    }
}
