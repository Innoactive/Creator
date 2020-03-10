using System;
using System.IO;
using Innoactive.Hub.Training;
using Innoactive.Hub.Training.Configuration;
using Innoactive.Hub.Training.Editors.Configuration;
using Innoactive.Hub.Training.Editors.Windows;
using Innoactive.Hub.Training.Utils.Serialization;
using UnityEngine;

namespace Innoactive.Creator.Core.Editor.Source.Utils
{
    /// <summary>
    /// Course utility functions at editor runtime.
    /// </summary>
    public static class EditorCourseUtils
    {
        /// <summary>
        /// Creates an empty course with given name.
        /// </summary>
        public static ICourse CreateCourse(string name)
        {
            return new Course(name, new Chapter("Chapter 1", null));
        }

        /// <summary>
        /// Sets the training course and opens the TrainingWindow.
        /// </summary>
        public static bool SetTrainingCourseActive(ICourse course)
        {
            TrainingWindow trainingWindow = TrainingWindow.GetWindow();
            trainingWindow.Focus();

            if (trainingWindow.SetTrainingCourseWithUserConfirmation(course) == false)
            {
                Debug.LogError("Could not set course.");
                return false;
            }

            if (SaveManager.SaveTrainingCourseToFile(course))
            {
                trainingWindow.IsDirty = false;
                string path = GetTrainingPath(course).Substring(Application.streamingAssetsPath.Length + 1);
                RuntimeConfigurator.SetSelectedTrainingCourse(path);
                return true;
            }
            Debug.LogError("Could not save the training course");
            return false;
        }

        /// <summary>
        /// Imports a training course by file from given path using the given serializer.
        /// </summary>
        /// <returns>Return true if the course creation was successfully finished.</returns>
        public static bool ImportTrainingCourse(string path, ITrainingSerializer serializer)
        {
            try
            {
                byte[] file = File.ReadAllBytes(path);
                ICourse course = serializer.ToCourse(file);

                int counter = 0;
                while (IsTrainingExisting(course.Data.Name))
                {
                    if (counter > 0)
                    {
                        course.Data.Name = course.Data.Name.Substring(0, course.Data.Name.Length - 2);
                    }
                    counter++;
                    course.Data.Name += "_" + counter;
                }
                return SetTrainingCourseActive(course);
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("{0} occured while trying to import file '{1}' with serializer '{2}'\n\n{3}", ex.GetType().Name, path, serializer.GetType().Name, ex.StackTrace);
            }

            return false;
        }

        /// <summary>
        /// Checks if training with given name already exists in the project.
        /// </summary>
        public static bool IsTrainingExisting(string name)
        {
            return Directory.Exists(Path.GetDirectoryName(GetTrainingPath(name)));
        }

        /// <summary>
        /// Get the course path by training name.
        /// </summary>
        public static string GetTrainingPath(string name)
        {
            name = Path.GetFileNameWithoutExtension(name);
            string fileFormat = EditorConfigurator.Instance.Serializer.FileFormat;
            return string.Format("{0}/{1}/{2}/{2}.{3}", Application.streamingAssetsPath, EditorConfigurator.Instance.DefaultCourseStreamingAssetsFolder, name, fileFormat).Replace('/', Path.DirectorySeparatorChar);
        }

        /// <summary>
        /// Get the course path by course.
        /// </summary>
        public static string GetTrainingPath(ICourse course)
        {
            return GetTrainingPath(course.Data.Name);
        }
    }
}
