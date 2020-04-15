using Innoactive.Creator.Core;
using Innoactive.CreatorEditor.UI.Drawers;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Windows
{
    /// <inheritdoc />
    /// <summary>
    /// Step Inspector window of workflow editor.
    /// </summary>
    public class StepWindow : EditorWindow
    {
        private IStep step;

        [SerializeField]
        private Vector2 scrollPosition;

        [SerializeField]
        private Rect stepRect;

        /// <summary>
        /// Returns true if there is an instance of <see cref="CourseWindow"/> is opened.
        /// </summary>
        public static bool IsTrainingWindowOpen
        {
            get { return EditorUtils.IsWindowOpened<CourseWindow>(); }
        }

        /// <summary>
        /// Returns the first <see cref="StepWindow"/> which is currently opened.
        /// If there is none, creates and shows <see cref="StepWindow"/>.
        /// </summary>
        public static void ShowInspector()
        {
            if (EditorUtils.IsWindowOpened<StepWindow>())
            {
                return;
            }

            StepWindow instance = GetWindow<StepWindow>("Step", false);
            instance.Repaint();
        }

        /// <summary>
        /// Closes currently opened <see cref="StepWindow"/>.
        /// </summary>
        public static void HideInspector()
        {
            if (IsTrainingWindowOpen == false)
            {
                return;
            }

            StepWindow instance = GetWindow<StepWindow>();
            instance.step = null;
            instance.Close();
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        private void OnGUI()
        {
            if (CourseWindow.IsOpen && CourseWindow.GetWindow().GetChapter() != null)
            {
                step = CourseWindow.GetWindow().GetChapter().ChapterMetadata.LastSelectedStep;
            }

            titleContent = new GUIContent("Step");

            if (step == null)
            {
                return;
            }

            ITrainingDrawer drawer = DrawerLocator.GetDrawerForValue(step, typeof(Step));

            stepRect.width = position.width;

            if (stepRect.height > position.height - EditorGUIUtility.singleLineHeight)
            {
                stepRect.width -= GUI.skin.verticalScrollbar.fixedWidth;
            }

            scrollPosition = GUI.BeginScrollView(new Rect(0, 0, position.width, position.height), scrollPosition, stepRect, false, false);
            {
                stepRect = drawer.Draw(stepRect, step, SetStep, "Step");
            }
            GUI.EndScrollView();
        }

        private void SetStep(object step)
        {
            this.step = (Step)step;
            if (CourseWindow.IsOpen)
            {
                CourseWindow.GetWindow().GetChapter().ChapterMetadata.LastSelectedStep = this.step;
                CourseWindow.GetWindow().RefreshChapterRepresentation();
            }
        }
    }
}
