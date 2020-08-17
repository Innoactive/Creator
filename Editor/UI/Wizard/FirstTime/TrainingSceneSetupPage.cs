﻿using System;
using System.IO;
using Innoactive.CreatorEditor;
using Innoactive.CreatorEditor.Setup;
using UnityEditor;
using UnityEngine;

namespace Innoactive.Creator.Core.Editor.UI.Wizard
{
    /// <summary>
    /// Wizard page which handles the training scene setup.
    /// </summary>
    internal class TrainingSceneSetupPage : WizardPage
    {
        [SerializeField]
        private bool useCurrentScene = true;
        [SerializeField]
        private bool loadSample = true;

        [SerializeField]
        private string courseName = "My first VR Training course";
        private string sceneDirectory = "Assets/Scenes";

        private const int MaxCourseNameLength = 40;
        private const int MinHeightOfInfoText = 20;

        public TrainingSceneSetupPage() : base("Step 1: Sample Training")
        {

        }

        /// <inheritdoc />
        public override void Draw(Rect window)
        {
            GUILayout.BeginArea(window);

            GUILayout.Label("Load a sample training", CreatorEditorStyles.Title);

            // Use the next two lines and remove the "loadSample = false" line as soon as loading samples is supported
            // if (GUILayout.Toggle(loadSample, "Load sample VR training (recommended)", CreatorEditorStyles.Toggle)) loadSample = true;
            // if (GUILayout.Toggle(!loadSample, "Start from scratch with an empty VR training", CreatorEditorStyles.Toggle)) loadSample = false;
            loadSample = false;

            if (loadSample == false)
            {
                RectOffset margin = CreatorEditorStyles.Paragraph.margin;
                margin.top = CreatorEditorStyles.BaseMargin + CreatorEditorStyles.Indent;
                GUILayout.Label("Name of your VR Training:", CreatorEditorStyles.ApplyMargin(CreatorEditorStyles.Paragraph, margin));

                GUILayout.BeginHorizontal();
                    courseName = GUILayout.TextField(courseName, MaxCourseNameLength, CreatorEditorStyles.ApplyIdent(EditorStyles.textField, CreatorEditorStyles.IndentLarge), GUILayout.Width(window.width * 0.7f));
                    GUILayout.Label($"{courseName.Length}/{MaxCourseNameLength}");
                GUILayout.EndHorizontal();

                string courseInfoText = "";
                if (CourseAssetUtils.DoesCourseAssetExist(courseName))
                {
                    courseInfoText = "Course already exists and will be used.";
                }

                GUILayout.Label(courseInfoText, CreatorEditorStyles.ApplyIdent(CreatorEditorStyles.SubText, CreatorEditorStyles.IndentLarge), GUILayout.MinHeight(MinHeightOfInfoText));

                if (GUILayout.Toggle(useCurrentScene, "Take my current scene", CreatorEditorStyles.ApplyIdent(CreatorEditorStyles.Toggle, CreatorEditorStyles.IndentLarge))) useCurrentScene = true;
                if (GUILayout.Toggle(!useCurrentScene, "Create a new scene", CreatorEditorStyles.ApplyIdent(CreatorEditorStyles.Toggle, CreatorEditorStyles.IndentLarge))) useCurrentScene = false;

                if (useCurrentScene == false)
                {
                    string sceneInfoText = "Scene will have the same name as the training course.";
                    if (SceneExists(courseName))
                    {
                        sceneInfoText += " Scene already exists";
                        CanProceed = false;
                    }
                    else
                    {
                        CanProceed = true;
                    }

                    GUILayout.Label(sceneInfoText, CreatorEditorStyles.ApplyIdent(CreatorEditorStyles.SubText, CreatorEditorStyles.IndentLarge), GUILayout.MinHeight(MinHeightOfInfoText));
                }
                else
                {
                    CanProceed = true;
                }
            }

            GUILayout.EndArea();
        }

        /// <inheritdoc />
        public override void Apply()
        {
            base.Apply();

            if (useCurrentScene == false)
            {
                SceneSetupUtils.CreateNewScene(courseName, sceneDirectory);
            }

            SceneSetupUtils.SetupSceneAndTraining(courseName);
            EditorWindow.FocusWindowIfItsOpen<WizardWindow>();
        }

        private bool SceneExists(string sceneName)
        {
            return File.Exists($"{sceneDirectory}/{sceneName}.unity");
        }
    }
}
