using System.IO;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Configuration;
using Innoactive.CreatorEditor.UndoRedo;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Windows
{
    /// <summary>
    /// Handles changing the course name.
    /// </summary>
    public class RenameCoursePopup : EditorWindow
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
                if (string.IsNullOrEmpty(newName) == false && ValidateCourseName(newName))
                {
                    string oldName = course.Data.Name;
                    string oldPath = EditorCourseUtils.GetAbsoluteCoursePath(oldName);
                    string oldFolder = Path.GetDirectoryName(oldPath);
                    string newPath = EditorCourseUtils.GetAbsoluteCoursePath(newName);
                    string newFolder = Path.GetDirectoryName(newPath);

                    RevertableChangesHandler.Do(new TrainingCommand(
                        // ReSharper disable once ImplicitlyCapturedClosure
                        () =>
                        {
                            if (ValidateCourseName(newName))
                            {
                                Directory.Move(oldFolder, newFolder);
                                File.Move(string.Format("{0}.meta", oldFolder), string.Format("{0}.meta", newFolder));
                                File.Move(string.Format("{0}/{1}.json", newFolder, oldName), newPath);
                                File.Move(string.Format("{0}/{1}.json.meta", newFolder, oldName), string.Format("{0}.meta", newPath));
                                course.Data.Name = newName;

                                SaveManager.SaveTrainingCourseToFile(course);
                                RuntimeConfigurator.Instance.SetSelectedTrainingCourse(EditorCourseUtils.GetCoursePath(course));
                                TrainingWindow.GetWindow().IsDirty = false;
                            }
                        },
                        // ReSharper disable once ImplicitlyCapturedClosure
                        () =>
                        {
                            if (Directory.Exists(newFolder) == false)
                            {
                                return;
                            }

                            Directory.Move(newFolder, oldFolder);
                            File.Move(string.Format("{0}.meta", newFolder), string.Format("{0}.meta", oldFolder));
                            File.Move(string.Format("{0}/{1}.json", oldFolder, newName), oldPath);
                            File.Move(string.Format("{0}/{1}.json.meta", oldFolder, newName), string.Format("{0}.meta", oldPath));
                            course.Data.Name = oldName;

                            SaveManager.SaveTrainingCourseToFile(course);
                            RuntimeConfigurator.Instance.SetSelectedTrainingCourse(oldPath.Substring(Application.streamingAssetsPath.Length + 1));
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

        private bool ValidateCourseName(string courseName)
        {
            if (course.Data.Name.Equals(courseName))
            {
                return false;
            }

            int invalidCharacterIndex = -1;
            if ((invalidCharacterIndex = courseName.IndexOfAny(Path.GetInvalidFileNameChars())) >= 0)
            {
                EditorUtility.DisplayDialog("Changing the course name failed",
                    string.Format("Course name contains invalid character: {0}",
                        courseName[invalidCharacterIndex]), "ok");
                return false;
            }

            string newFolder = Path.GetDirectoryName(EditorCourseUtils.GetCoursePath(courseName));
            if (Directory.Exists(newFolder))
            {
                EditorUtility.DisplayDialog("Changing the course name failed",
                    string.Format("Training course with name \"{0}\" already exists!", courseName), "ok");
                return false;
            }

            return true;
        }
    }
}
