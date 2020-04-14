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
        public static ICourse TrackedCourse { get; private set; }

        public static void CreateEmpty(string courseName)
        {
            Import(new Course(courseName, new Chapter("Chapter 1", null)));
        }

        public static void Save()
        {
            if (TrackedCourse == null)
            {
                return;
            }

            Save(TrackedCourse);
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

        public static void Track(string courseName)
        {
            if (IsCourseAssetExist(courseName) == false)
            {
                Track(default(ICourse));
            }

            ICourse course = Load(courseName);
            Track(course);
        }

        private static void Track(ICourse course)
        {
            TrackedCourse = course;
        }

        private static ICourse Load(string courseName)
        {
            if (IsCourseAssetExist(courseName) == false)
            {
                return null;
            }

            return EditorConfigurator.Instance.Serializer.CourseFromByteArray(File.ReadAllBytes(GetCourseAsset(courseName)));
        }

        public static void Delete(string courseName)
        {
            if (IsCourseAssetExist(courseName))
            {
                Directory.Delete(GetCourseAssetDirectory(courseName));
            }

            AssetDatabase.Refresh();
        }

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

        private static bool IsCourseAssetExist(string courseName)
        {
            return File.Exists(GetCourseAsset(courseName));
        }

        private static string GetCourseAssetDirectory(string courseName)
        {
            return $"{Application.streamingAssetsPath}/{GetCourseStreamingAssetsSubdirectory(courseName)}";
        }

        public static string GetCourseStreamingAssetsSubdirectory(string courseName)
        {
            return $"{EditorConfigurator.Instance.CourseStreamingAssetsSubdirectory}/{courseName}";
        }

        public static string GetCourseAsset(string courseName)
        {
            return $"{GetCourseAssetDirectory(courseName)}/{courseName}.{EditorConfigurator.Instance.Serializer.FileFormat}";
        }

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

        public static void RenameCourse(ICourse course, string newName)
        {
            string oldDirectory = GetCourseAssetDirectory(course.Data.Name);
            string newDirectory = GetCourseAssetDirectory(newName);

            Directory.Move(oldDirectory, newDirectory);
            File.Move($"{oldDirectory}.meta", $"{newDirectory}.meta");

            string newAsset = GetCourseAsset(newName);
            string oldAsset = $"{GetCourseAssetDirectory(newName)}/{course.Data.Name}.{EditorConfigurator.Instance.Serializer.FileFormat}";
            File.Move(oldAsset, newAsset);
            File.Move($"{oldAsset}.meta",$"{newAsset}.meta");
            course.Data.Name = newName;

            Save(course);
        }

        public static IEnumerable<string> GetAllCourses()
        {
            DirectoryInfo coursesDirectory = new DirectoryInfo($"{Application.streamingAssetsPath}/{EditorConfigurator.Instance.CourseStreamingAssetsSubdirectory}");
            return coursesDirectory.GetDirectories()
                .Select(directory => directory.Name)
                .Where(courseName => File.Exists(GetCourseAsset(courseName)))
                .ToList();
        }

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

        public static bool CanRename(ICourse course, string newName, out string errorMessage)
        {
            errorMessage = string.Empty;

            return course.Data.Name != newName && CanCreate(newName, out errorMessage);
        }
    }
}
