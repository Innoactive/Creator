using Innoactive.Creator.Core;
using Innoactive.CreatorEditor.ImguiTester;
using Innoactive.CreatorEditor.UndoRedo;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Windows
{
    /// <summary>
    /// Handles changing the course name.
    /// </summary>
    internal class RenameCoursePopup : EditorWindow
    {
        private static RenameCoursePopup instance;

        private readonly GUID textFieldIdentifier = new GUID();

        private ICourse course;
        private string newName;

        private bool isFocusSet;

        public bool IsClosed { get; private set; }

        public static RenameCoursePopup Open(ICourse course, Rect labelPosition, Vector2 offset)
        {
            if (instance != null)
            {
                instance.Close();
            }

            instance = CreateInstance<RenameCoursePopup>();

            instance.course = course;
            instance.newName = instance.course.Data.Name;

            instance.position = new Rect(labelPosition.x - offset.x, labelPosition.y - offset.y, labelPosition.width, labelPosition.height);
            instance.ShowPopup();
            instance.Focus();

            AssemblyReloadEvents.beforeAssemblyReload += () =>
            {
                instance.Close();
                instance.IsClosed = true;
            };

            return instance;
        }

        private void OnGUI()
        {
            if (course == null || focusedWindow != this)
            {
                Close();
                instance.IsClosed = true;
            }

            GUI.SetNextControlName(textFieldIdentifier.ToString());
            newName = EditorGUILayout.TextField(newName);
            newName = newName.Trim();

            if (isFocusSet == false)
            {
                isFocusSet = true;
                EditorGUI.FocusTextInControl(textFieldIdentifier.ToString());
            }

            if ((Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter))
            {
                if (CourseAssetManager.CanRename(course, newName, out string error) == false)
                {
                    if (string.IsNullOrEmpty(error) == false && string.IsNullOrEmpty(error) == false)
                    {
                        TestableEditorElements.DisplayDialog("Cannot rename the course", error, "OK");
                    }
                }
                else
                {
                    string oldName = course.Data.Name;

                    RevertableChangesHandler.Do(new CourseCommand(
                        () =>
                        {
                            if (CourseAssetManager.CanRename(course, newName, out string errorMessage) == false)
                            {
                                if (string.IsNullOrEmpty(errorMessage) == false)
                                {
                                    TestableEditorElements.DisplayDialog("Cannot rename the course", errorMessage, "OK");
                                }

                                RevertableChangesHandler.FlushStack();
                            }

                            CourseAssetManager.RenameCourse(course, newName);
                        },
                        () =>
                        {
                            if (CourseAssetManager.CanRename(course, newName, out string errorMessage) == false)
                            {
                                if (string.IsNullOrEmpty(errorMessage) == false)
                                {
                                    TestableEditorElements.DisplayDialog("Cannot rename the course", errorMessage, "OK");
                                }

                                RevertableChangesHandler.FlushStack();
                            }

                            CourseAssetManager.RenameCourse(course, oldName);
                        }
                    ));
                }

                Close();
                instance.IsClosed = true;
                Event.current.Use();
            }
            else if (Event.current.keyCode == KeyCode.Escape)
            {
                Close();
                instance.IsClosed = true;
                Event.current.Use();
            }
        }
    }
}
