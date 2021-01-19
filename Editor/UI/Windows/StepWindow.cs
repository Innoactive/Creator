using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Innoactive.Creator.Core;
using Innoactive.CreatorEditor.Tabs;
using Innoactive.CreatorEditor.UI.Drawers;
using Innoactive.CreatorEditor.Configuration;
using UnityEngine.UIElements;
using Innoactive.CreatorEditor.UI.UIElements;


namespace Innoactive.CreatorEditor.UI.Windows
{
    /// <summary>
    /// This class draws the Step Inspector.
    /// </summary>
    internal class StepWindow : EditorWindow
    {


        private const int border = 10;

        private IStep step;

        public static VisualElement root;

        [SerializeField]
        private Vector2 scrollPosition;

        [SerializeField]
        private Rect stepRect;

        private StyleSheet style;
        private VisualTreeAsset inpectorVisualTree;

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
            EditorSceneManager.sceneClosing += OnSceneClosed;
            GlobalEditorHandler.StepWindowOpened(this);
               
            root = rootVisualElement;

            //load the main visual tree containing all elements which do not need to be dynamically loaded, e.g. step name, descriptiong, the tabs
            inpectorVisualTree = (VisualTreeAsset)Resources.Load("UI/Inspector_Main");
            inpectorVisualTree.CloneTree(root);

            //load all style files required for this windows here.
            style = (StyleSheet)Resources.Load("UI/ICStepInspectorStyle");
            StyleSheet behaviorStyle = (StyleSheet)Resources.Load("UI/ICBehaviorStyle");

            root.styleSheets.Add(style);
            root.styleSheets.Add(behaviorStyle);

        }

      

            private void OnDestroy()
        {
            EditorSceneManager.sceneClosing -= OnSceneClosed;
            GlobalEditorHandler.StepWindowClosed(this);
            
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        private void OnFocus()
        {
            if (step?.Data == null)
            {
                return;
            }

            if (EditorConfigurator.Instance.Validation.IsAllowedToValidate())
            {
                EditorConfigurator.Instance.Validation.Validate(step.Data, GlobalEditorHandler.GetCurrentCourse());
            }

        }

        private void OnGUI()
        {
            titleContent = new GUIContent("Step Inspector");

            if (step == null)
            {
                return;
            }

            //ITrainingDrawer drawer = DrawerLocator.GetDrawerForValue(step, typeof(Step));

            stepRect.width = position.width;

            if (stepRect.height > position.height)
            {
                stepRect.width -= GUI.skin.verticalScrollbar.fixedWidth;
            }

            scrollPosition = GUI.BeginScrollView(new Rect(0, 0, position.width, position.height), scrollPosition, stepRect, false, false);
            {
                Rect stepDrawingRect = new Rect(stepRect.position + new Vector2(border, border), stepRect.size - new Vector2(border * 2f, border * 2f));
                //stepDrawingRect = drawer.Draw(stepDrawingRect, step, ModifyStep, "Step");
                //stepRect = new Rect(stepDrawingRect.position - new Vector2(border,border), stepDrawingRect.size + new Vector2(border * 2f, border * 2f));
            }
            GUI.EndScrollView();

           
        }

        private void OnSceneClosed(Scene scene, bool removingscene)
        {
            SetStep(null);
        }

        private void ModifyStep(object newStep)
        {
            step = (IStep)newStep;
            GlobalEditorHandler.CurrentStepModified(step);
        }

        public void SetStep(IStep newStep)
        {
            step = newStep;
        }

        public IStep GetStep()
        {
            return step;
        }

        internal void ResetStepView()
        {
            if (EditorUtils.IsWindowOpened<StepWindow>() == false || step == null)
            {
                return;
            }

            Dictionary<string, object> dict = step.Data.Metadata.GetMetadata(typeof(TabsGroup));
            if (dict.ContainsKey(TabsGroup.SelectedKey))
            {
                dict[TabsGroup.SelectedKey] = 0;
            }
        }
    }
}
