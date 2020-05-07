using System.Collections.Generic;
using System.IO;
using System.Linq;
using Innoactive.Creator.Core;
using Innoactive.CreatorEditor.Configuration;
using UnityEngine;

namespace Innoactive.CreatorEditor
{
    internal static class CourseAssetUtils
    {
        /// <summary>
        /// Extracts the file name from the <paramref name="coursePath"/>. Works with both relative and full paths.
        /// </summary>
        internal static string GetCourseNameFromPath(string coursePath)
        {
            return Path.GetFileNameWithoutExtension(coursePath);
        }

        /// <summary>
        /// Returns the asset path to the course with the <paramref name="courseName"/>.
        /// </summary>
        internal static string GetCourseAssetPath(string courseName)
        {
            return $"{GetCourseAssetDirectory(courseName)}/{courseName}.{EditorConfigurator.Instance.Serializer.FileFormat}";
        }

        /// <summary>
        /// Returns the relative path from the streaming assets directory to the course with the <paramref name="courseName"/>.
        /// </summary>
        internal static string GetCourseStreamingAssetPath(string courseName)
        {
            return $"{GetCourseStreamingAssetsSubdirectory(courseName)}/{courseName}.{EditorConfigurator.Instance.Serializer.FileFormat}";
        }

        /// <summary>
        /// Returns true if the file at given <paramref name="assetPath"/> is a course. It does not check the validity of the file's contents.
        /// </summary>
        internal static bool IsValidCourseAssetPath(string assetPath)
        {
            string filePath = Path.Combine(Application.dataPath.Remove(Application.dataPath.LastIndexOf('/')), assetPath).Replace('/', Path.DirectorySeparatorChar);
            string courseFolderPath = Path.Combine(Application.streamingAssetsPath, EditorConfigurator.Instance.CourseStreamingAssetsSubdirectory).Replace('/', Path.DirectorySeparatorChar);

            FileInfo file = new FileInfo(filePath);

            return
                file.Directory?.Parent != null
                && new DirectoryInfo(courseFolderPath).FullName == file.Directory.Parent.FullName
                && Path.GetFileNameWithoutExtension(assetPath) == file.Directory.Name;
        }

        /// <summary>
        /// Returns a list of names of all courses in the project.
        /// </summary>
        internal static IEnumerable<string> GetAllCourses()
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
        internal static bool CanCreate(string courseName, out string errorMessage)
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
        /// Checks if you can rename the <paramref name="course"/> to the <paramref name="newName"/>.
        /// </summary>
        /// <param name="errorMessage">Empty if you can create the course or must fail silently. </param>
        internal static bool CanRename(ICourse course, string newName, out string errorMessage)
        {
            errorMessage = string.Empty;

            return course.Data.Name != newName && CanCreate(newName, out errorMessage);
        }

        internal static bool IsCourseAssetExist(string courseName)
        {
            return File.Exists(GetCourseAssetPath(courseName));
        }

        internal static string GetCourseAssetDirectory(string courseName)
        {
            return $"{Application.streamingAssetsPath}/{GetCourseStreamingAssetsSubdirectory(courseName)}";
        }

        private static string GetCourseStreamingAssetsSubdirectory(string courseName)
        {
            return $"{EditorConfigurator.Instance.CourseStreamingAssetsSubdirectory}/{courseName}";
        }
    }
}
