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

        private void OnEnable()
        {
            Editors.StepWindowOpened(this);
        }

        private void OnDestroy()
        {
            Editors.StepWindowClosed(this);
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        private void OnGUI()
        {
            titleContent = new GUIContent("Step Editor");

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
                stepRect = drawer.Draw(stepRect, step, ModifyStep, "Step");
            }
            GUI.EndScrollView();
        }

        private void ModifyStep(object newStep)
        {
            step = (IStep)newStep;
            Editors.CurrentStepModified(step);
        }

        public void SetStep(IStep newStep)
        {
            step = newStep;
        }
    }
}
