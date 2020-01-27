using System.IO;
using Innoactive.Hub.Training.Configuration;
using Innoactive.Hub.Training.Editors.Utils.Undo;
using UnityEditor;
using UnityEngine;

namespace Innoactive.Hub.Training.Editors.Windows
{
    public class RenameCoursePopup : EditorWindow
    {
        private static RenameCoursePopup instance;

        private readonly GUID textFieldIdentifier = new GUID();

        private ICourse course;
        private string newName;

        private bool isFocusSet;
        private string errorMessage;

        public bool IsClosed { get; protected set; }

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

            if (isFocusSet == false)
            {
                isFocusSet = true;
                EditorGUI.FocusTextInControl(textFieldIdentifier.ToString());
            }

            if (focusedWindow != this)
            {
                return;
            }

            if (Event.current.isKey == false)
            {
                return;
            }

            if ((Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter))
            {
                if (string.IsNullOrEmpty(newName))
                {
                    return;
                }

                string oldName = course.Data.Name;
                string oldPath = SaveManager.GetTrainingPath(oldName);
                string oldFolder = Path.GetDirectoryName(oldPath);
                string newPath = SaveManager.GetTrainingPath(newName);
                string newFolder = Path.GetDirectoryName(newPath);

                RevertableChangesHandler.Do(new TrainingCommand(
                    // ReSharper disable once ImplicitlyCapturedClosure
                    () =>
                    {
                        int invalidCharacterIndex = -1;

                        if ((invalidCharacterIndex = newName.IndexOfAny(Path.GetInvalidFileNameChars())) >= 0)
                        {
                            errorMessage = string.Format("Course name contains invalid character: {0}", newName[invalidCharacterIndex]);
                        }
                        else if (Directory.Exists(newFolder))
                        {
                            errorMessage = string.Format("Training course with name \"{0}\" already exists!", newName);
                        }
                        else
                        {
                            Directory.Move(oldFolder, newFolder);
                            File.Move(string.Format("{0}.meta", oldFolder), string.Format("{0}.meta", newFolder));
                            File.Move(string.Format("{0}/{1}.json", newFolder, oldName), newPath);
                            File.Move(string.Format("{0}/{1}.json.meta", newFolder, oldName), string.Format("{0}.meta", newPath));
                            course.Data.Name = newName;

                            SaveManager.SaveTrainingCourseToFile(course);
                            RuntimeConfigurator.SetSelectedTrainingCourse(newPath.Substring(Application.streamingAssetsPath.Length + 1));
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
                        RuntimeConfigurator.SetSelectedTrainingCourse(oldPath.Substring(Application.streamingAssetsPath.Length + 1));
                    }
                ));

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
