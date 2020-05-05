using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Serialization;
using Innoactive.CreatorEditor.Configuration;
using Innoactive.CreatorEditor.UI.Windows;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor
{
    [InitializeOnLoad]
    public static class Editors
    {
        public const string LastEditedCourseNameKey = "Innoactive.Creator.Editors.LastEditedCourseName";

        private static IEditingStrategy strategy;

        static Editors()
        {
            SetDefaultStrategy();

            string lastEditedCourseName = EditorPrefs.GetString(LastEditedCourseNameKey);
            SetCurrentCourse(lastEditedCourseName);
        }

        public static void SetDefaultStrategy()
        {
            SetStrategy(new DefaultEditingStrategy());
        }

        internal static void SetStrategy(IEditingStrategy newStrategy)
        {
            strategy = newStrategy;

            if (newStrategy == null)
            {
                Debug.LogError("An editing strategy cannot be null, set to default instead.");
                SetDefaultStrategy();
            }
        }

        public static void CourseWindowOpened(CourseWindow window)
        {
            strategy.HandleNewCourseWindow(window);
        }

        public static void CourseWindowClosed(CourseWindow window)
        {
            strategy.HandleCourseWindowClosed(window);
        }

        public static void StepWindowClosed(StepWindow window)
        {
            strategy.HandleStepWindowClosed(window);
        }

        public static void StepWindowOpened(StepWindow window)
        {
            strategy.HandleNewStepWindow(window);
        }

        public static void SetCurrentCourse(string courseName)
        {
            strategy.HandleCurrentCourseChanged(courseName);
        }

        public static void StartEditing()
        {
            strategy.HandleStartEditing();
        }

        public static void CurrentCourseModified()
        {
            strategy.HandleCurrentCourseModified();
        }

        public static void CurrentStepModified(IStep step)
        {
            strategy.HandleCurrentStepModified(step);
        }

        public static void StartEditingStep(IStep step)
        {
            strategy.HandleStartEditingStep(step);
        }
    }

    /// <summary>
    /// A static class that handles the course assets. It lets you to save, load, delete, and import training courses and provides multiple related utility methods.
    /// </summary>
    public static class CourseAssetManager
    {
        /// <summary>
        /// Deletes the course with <paramref name="courseName"/>.
        /// </summary>
        public static void Delete(string courseName)
        {
            if (IsCourseAssetExist(courseName))
            {
                Directory.Delete(GetCourseAssetDirectory(courseName));
                AssetDatabase.Refresh();
            }
        }

        /// <summary>
        /// Imports the given <paramref name="course"/> by saving it to the proper directory. If there is a name collision, this course will be renamed.
        /// </summary>
        public static void Import(ICourse course)
        {
            int counter = 0;
            string oldName = course.Data.Name;
            while (IsCourseAssetExist(course.Data.Name))
            {
                if (counter > 0)
                {
                    course.Data.Name = course.Data.Name.Substring(0, course.Data.Name.Length - 2);
                }

                counter++;
                course.Data.Name += " " + counter;
            }

            if (oldName != course.Data.Name)
            {
                Debug.LogWarning($"We detected a name collision while importing course \"{oldName}\". We have renamed it to \"{course.Data.Name}\" before importing.");
            }

            Save(course);
        }

        /// <summary>
        /// Imports the course from file at given file <paramref name="path"/> if the file extensions matches the <paramref name="serializer"/>.
        /// </summary>
        public static void Import(string path, ICourseSerializer serializer)
        {
            ICourse course;

            if (Path.GetExtension(path) != $".{serializer.FileFormat}")
            {
                Debug.LogError($"The file extension of {path} does not match the expected file extension of {serializer.FileFormat} of the current serializer.");
            }

            try
            {
                byte[] file = File.ReadAllBytes(path);
                course = serializer.CourseFromByteArray(file);
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.GetType().Name} occured while trying to import file '{path}' with serializer '{serializer.GetType().Name}'\n\n{e.StackTrace}");
                return;
            }

            Import(course);
        }

        /// <summary>
        /// Returns the asset path to the course with the <paramref name="courseName"/>.
        /// </summary>
        public static string GetCourseAssetPath(string courseName)
        {
            return $"{GetCourseAssetDirectory(courseName)}/{courseName}.{EditorConfigurator.Instance.Serializer.FileFormat}";
        }

        /// <summary>
        /// Returns the relative path from the streaming assets directory to the course with the <paramref name="courseName"/>.
        /// </summary>
        public static string GetCourseStreamingAssetPath(string courseName)
        {
            return $"{GetCourseStreamingAssetsSubdirectory(courseName)}/{courseName}.{EditorConfigurator.Instance.Serializer.FileFormat}";
        }

        /// <summary>
        /// Returns true if the file at given <paramref name="assetPath"/> is a course. It does not check the validity of the file's contents.
        /// </summary>
        public static bool IsCourseAsset(string assetPath)
        {
            if (File.Exists(assetPath) == false)
            {
                return false;
            }

            string filePath = Path.Combine(Application.dataPath.Remove(Application.dataPath.LastIndexOf('/')), assetPath).Replace('/', Path.DirectorySeparatorChar);
            string courseFolderPath = Path.Combine(Application.streamingAssetsPath, EditorConfigurator.Instance.CourseStreamingAssetsSubdirectory).Replace('/', Path.DirectorySeparatorChar);

            FileInfo file = new FileInfo(filePath);

            return
                file.Directory?.Parent != null
                && new DirectoryInfo(courseFolderPath).FullName == file.Directory.Parent.FullName
                && Path.GetFileNameWithoutExtension(assetPath) == file.Directory.Name;
        }

        /// <summary>
        /// Renames the <paramref name="course"/> to the <paramref name="newName"/> and moves it to the appropriate directory. Check if you can rename before with the <seealso cref="CanRename"/> method.
        /// </summary>
        public static void RenameCourse(ICourse course, string newName)
        {
            string oldDirectory = GetCourseAssetDirectory(course.Data.Name);
            string newDirectory = GetCourseAssetDirectory(newName);

            Directory.Move(oldDirectory, newDirectory);
            File.Move($"{oldDirectory}.meta", $"{newDirectory}.meta");

            string newAsset = GetCourseAssetPath(newName);
            string oldAsset = $"{GetCourseAssetDirectory(newName)}/{course.Data.Name}.{EditorConfigurator.Instance.Serializer.FileFormat}";
            File.Move(oldAsset, newAsset);
            File.Move($"{oldAsset}.meta", $"{newAsset}.meta");
            course.Data.Name = newName;

            Save(course);
        }

        /// <summary>
        /// Returns a list of names of all courses in the project.
        /// </summary>
        public static IEnumerable<string> GetAllCourses()
        {
            DirectoryInfo coursesDirectory = new DirectoryInfo($"{Application.streamingAssetsPath}/{EditorConfigurator.Instance.CourseStreamingAssetsSubdirectory}");
            return coursesDirectory.GetDirectories()
                .Select(directory => directory.Name)
                .Where(courseName => File.Exists(GetCourseAssetPath(courseName)))
                .ToList();
        }

        /// <summary>
        /// Checks if you can create a course with the given <paramref name="courseName"/>.
        /// </summary>
        /// <param name="errorMessage">Empty if you can create the course or must fail silently. </param>
        public static bool CanCreate(string courseName, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrEmpty(courseName))
            {
                errorMessage = "The course name is empty!";
                return false;
            }

            int invalidCharacterIndex;
            if ((invalidCharacterIndex = courseName.IndexOfAny(Path.GetInvalidFileNameChars())) >= 0)
            {
                errorMessage = $"The course name contains invalid character `{courseName[invalidCharacterIndex]}` at index {invalidCharacterIndex}";
                return false;
            }

            if (IsCourseAssetExist(courseName))
            {
                errorMessage = $"A course with the name \"{courseName}\" already exists!";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if you can rename the <paramref name="course"/> with to the <paramref name="newName"/>.
        /// </summary>
        /// <param name="errorMessage">Empty if you can create the course or must fail silently. </param>
        public static bool CanRename(ICourse course, string newName, out string errorMessage)
        {
            errorMessage = string.Empty;

            return course.Data.Name != newName && CanCreate(newName, out errorMessage);
        }

        public static void Save(ICourse course)
        {
            string path = GetCourseAssetPath(course.Data.Name);

            Directory.CreateDirectory(GetCourseAssetDirectory(course.Data.Name));
            StreamWriter stream = File.CreateText(path);

            byte[] serialized = EditorConfigurator.Instance.Serializer.CourseToByteArray(course);

            stream.Write(new UTF8Encoding().GetString(serialized));
            stream.Close();
        }

        public static ICourse Load(string courseName)
        {
            return IsCourseAssetExist(courseName) == false ? null : EditorConfigurator.Instance.Serializer.CourseFromByteArray(File.ReadAllBytes(GetCourseAssetPath(courseName)));
        }

        private static bool IsCourseAssetExist(string courseName)
        {
            return File.Exists(GetCourseAssetPath(courseName));
        }

        private static string GetCourseAssetDirectory(string courseName)
        {
            return $"{Application.streamingAssetsPath}/{GetCourseStreamingAssetsSubdirectory(courseName)}";
        }

        private static string GetCourseStreamingAssetsSubdirectory(string courseName)
        {
            return $"{EditorConfigurator.Instance.CourseStreamingAssetsSubdirectory}/{courseName}";
        }
    }
}
