using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Serialization;
using Innoactive.CreatorEditor.Configuration;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor
{
    public static class CourseAssetManager
    {
        /// <summary>
        /// The course that is currently edited by the user through <seealso cref="CourseWindow"/> and <seealso cref="StepWindow"/>.
        /// </summary>
        public static ICourse TrackedCourse { get; private set; }

        /// <summary>
        /// Creates a new empty course with a given name. Check if it possible with the <see cref="CanCreate"/> method.
        /// </summary>
        public static void CreateEmpty(string courseName)
        {
            Import(new Course(courseName, new Chapter("Chapter 1", null)));
        }

        /// <summary>
        /// Saves the tracked course to the disk.
        /// </summary>
        public static void Save()
        {
            if (TrackedCourse == null)
            {
                return;
            }

            Save(TrackedCourse);
        }

        /// <summary>
        /// Start tracking the course with given <paramref name="courseName"/>. See also <seealso cref="TrackedCourse"/>.
        /// </summary>
        public static void Track(string courseName)
        {
            if (IsCourseAssetExist(courseName) == false)
            {
                Track(default(ICourse));
            }

            ICourse course = Load(courseName);
            Track(course);
        }

        /// <summary>
        /// Deletes the course with <paramref name="courseName"/>.
        /// </summary>
        public static void Delete(string courseName)
        {
            if (IsCourseAssetExist(courseName))
            {
                Directory.Delete(GetCourseAssetDirectory(courseName));
            }

            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Imports the given <paramref name="course"/> by saving it to the proper directory. If there is a name collision, this course will be renamed.
        /// </summary>
        public static void Import(ICourse course)
        {
            int counter = 0;
            while (IsCourseAssetExist(course.Data.Name))
            {
                if (counter > 0)
                {
                    course.Data.Name = course.Data.Name.Substring(0, course.Data.Name.Length - 2);
                }

                counter++;
                course.Data.Name += " " + counter;
            }

            Save(course);
        }

        /// <summary>
        /// Imports the course from file at given file <paramref name="path"/> if the file extensions matches the <paramref name="serializer"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="serializer"></param>
        public static void Import(string path, ICourseSerializer serializer)
        {
            ICourse course;

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
        public static string GetCourseAsset(string courseName)
        {
            return $"{GetCourseAssetDirectory(courseName)}/{courseName}.{EditorConfigurator.Instance.Serializer.FileFormat}";
        }

        /// <summary>
        /// Returns the relative path from the streaming assets directory to the course with the <paramref name="courseName"/>.
        /// </summary>
        public static string GetCourseStreamingAsset(string courseName)
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

            return new DirectoryInfo(courseFolderPath).FullName == file.Directory.Parent.FullName && Path.GetFileNameWithoutExtension(assetPath) == file.Directory.Name;
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

            string newAsset = GetCourseAsset(newName);
            string oldAsset = $"{GetCourseAssetDirectory(newName)}/{course.Data.Name}.{EditorConfigurator.Instance.Serializer.FileFormat}";
            File.Move(oldAsset, newAsset);
            File.Move($"{oldAsset}.meta", $"{newAsset}.meta");
            course.Data.Name = newName;

            Save(course);
        }

        /// <summary>
        /// Returns a list of names of all courses in the project.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetAllCourses()
        {
            DirectoryInfo coursesDirectory = new DirectoryInfo($"{Application.streamingAssetsPath}/{EditorConfigurator.Instance.CourseStreamingAssetsSubdirectory}");
            return coursesDirectory.GetDirectories()
                .Select(directory => directory.Name)
                .Where(courseName => File.Exists(GetCourseAsset(courseName)))
                .ToList();
        }

        /// <summary>
        /// Checks if you can create a course with the given <paramref name="courseName"/>.
        /// </summary>
        /// <param name="errorMessage">Empty if you can create the course or must fail silently. </param>
        /// <returns></returns>
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
        /// <returns></returns>
        public static bool CanRename(ICourse course, string newName, out string errorMessage)
        {
            errorMessage = string.Empty;

            return course.Data.Name != newName && CanCreate(newName, out errorMessage);
        }

        private static void Save(ICourse course)
        {
            string path = GetCourseAsset(course.Data.Name);

            Directory.CreateDirectory(GetCourseAssetDirectory(course.Data.Name));
            StreamWriter stream = File.CreateText(path);

            byte[] serialized = EditorConfigurator.Instance.Serializer.CourseToByteArray(course);

            stream.Write(new UTF8Encoding().GetString(serialized));
            stream.Close();

            AssetDatabase.Refresh();
        }

        private static void Track(ICourse course)
        {
            TrackedCourse = course;
        }

        private static ICourse Load(string courseName)
        {
            return IsCourseAssetExist(courseName) == false ? null : EditorConfigurator.Instance.Serializer.CourseFromByteArray(File.ReadAllBytes(GetCourseAsset(courseName)));
        }

        private static bool IsCourseAssetExist(string courseName)
        {
            return File.Exists(GetCourseAsset(courseName));
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
