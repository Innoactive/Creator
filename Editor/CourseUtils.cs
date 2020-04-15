using System;
using System.IO;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Configuration;
using Innoactive.Creator.Core.Exceptions;
using Innoactive.Creator.Core.Serialization;
using Innoactive.CreatorEditor.Configuration;
using Innoactive.CreatorEditor.UI.Windows;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Innoactive.CreatorEditor
{
    /// <summary>
    /// Course utility functions at editor runtime.
    /// </summary>
    public static class CourseUtils
    {
        private static string coreFolder;

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
                RuntimeConfigurator.Instance.SetSelectedTrainingCourse(GetCoursePath(course));
                return true;
            }
            Debug.LogError("Could not save the training course");
            return false;
        }

        /// <summary>
        /// Imports a training course by file from given path using the given serializer.
        /// </summary>
        /// <returns>Return true if the course creation was successfully finished.</returns>
        public static bool ImportTrainingCourse(string path, ICourseSerializer serializer)
        {
            try
            {
                byte[] file = File.ReadAllBytes(path);
                ICourse course = serializer.CourseFromByteArray(file);

                int counter = 0;
                while (IsCourseExisting(course.Data.Name))
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
        public static bool IsCourseExisting(string course)
        {
            return Directory.Exists(Path.GetDirectoryName(GetCoursePath(course)));
        }

        /// <summary>
        /// Get the course path by course name.
        /// </summary>
        public static string GetCoursePath(string course)
        {
            course = Path.GetFileNameWithoutExtension(course);
            string fileFormat = EditorConfigurator.Instance.Serializer.FileFormat;
            return string.Format("{0}/{1}/{1}.{2}", EditorConfigurator.Instance.CourseStreamingAssetsFolder, course, fileFormat).Replace('/', Path.DirectorySeparatorChar);
        }

        /// <summary>
        /// Get the course path by course.
        /// </summary>
        public static string GetCoursePath(ICourse course)
        {
            return GetCoursePath(course.Data.Name);
        }

        /// <summary>
        /// Get the absolute course path by course name.
        /// </summary>
        public static string GetAbsoluteCoursePath(string course)
        {
            course = Path.GetFileNameWithoutExtension(course);
            string fileFormat = EditorConfigurator.Instance.Serializer.FileFormat;
            return string.Format("{0}/{1}/{2}/{2}.{3}", Application.streamingAssetsPath, EditorConfigurator.Instance.CourseStreamingAssetsFolder, course, fileFormat).Replace('/', Path.DirectorySeparatorChar);
        }

        /// <summary>
        /// Get the absolute course path by course.
        /// </summary>
        public static string GetAbsoluteCoursePath(ICourse course)
        {
            return GetAbsoluteCoursePath(course.Data.Name);
        }

        /// <summary>
        /// Returns the currently active course's name.
        /// </summary>
        public static string GetActiveCourseName()
        {
            return Path.GetFileNameWithoutExtension(RuntimeConfigurator.Instance.GetSelectedTrainingCourse());
        }

        /// <summary>
        /// Returns true if the file at given <paramref name="filePath"/> is a course. It does not check the validity of the file's contents.
        /// </summary>
        public static bool IsCourseFile(string filePath)
        {
            if (File.Exists(filePath) == false)
            {
                return false;
            }

            string assetPath = Path.Combine(Application.dataPath.Remove(Application.dataPath.LastIndexOf('/')), filePath).Replace('/', Path.DirectorySeparatorChar);
            string courseFolderPath = Path.Combine(Application.streamingAssetsPath, EditorConfigurator.Instance.CourseStreamingAssetsFolder).Replace('/', Path.DirectorySeparatorChar);

            FileInfo file = new FileInfo(assetPath);

            return new DirectoryInfo(courseFolderPath).FullName == file.Directory.Parent.FullName && Path.GetFileNameWithoutExtension(filePath) == file.Directory.Name;
        }
    }
}
