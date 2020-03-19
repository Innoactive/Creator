using System;
using System.IO;
using System.Linq;
using Innoactive.Creator.Core.IO;
using Innoactive.CreatorEditor.Configuration;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor
{
    /// <summary>
    /// Monitors training course files added or removed from the project.
    /// </summary>
    public class CourseAssetPostprocessor : AssetPostprocessor
    {
        /// <summary>
        /// Raised when a course file is added, removed or moved from the course folder.
        /// </summary>
        public static event EventHandler<CourseAssetPostprocessorEventArgs> CourseFileStructureChanged;

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (CourseFileStructureChanged != null && (
                GetTrainingCourseAssets(importedAssets).Any() ||
                GetTrainingCourseAssets(deletedAssets).Any() ||
                GetTrainingCourseAssets(movedAssets).Any() ||
                GetTrainingCourseAssets(movedFromAssetPaths).Any()))
            {
                CourseFileStructureChanged.Invoke(null, new CourseAssetPostprocessorEventArgs());
            }
        }

        private static string[] GetTrainingCourseAssets(string[] assets)
        {
            return assets.Where(File.Exists).Where(IsInCourseFolder).Where(FileUtils.IsTrainingCourseFile).ToArray();
        }

        private static bool IsInCourseFolder(string asset)
        {
            // Both Application.dataPath and asset path contain the "Assets" folder, we need to remove it from the former.
            string assetPath = Path.Combine(Application.dataPath.Remove(Application.dataPath.LastIndexOf('/')), asset).Replace('/', Path.DirectorySeparatorChar);
            string courseFolderPath = Path.Combine(Application.streamingAssetsPath, EditorConfigurator.Instance.DefaultCourseStreamingAssetsFolder).Replace('/', Path.DirectorySeparatorChar);
            return assetPath.StartsWith(courseFolderPath);
        }
    }

    /// <summary>
    /// Event args for <see cref="CourseAssetPostprocessor"/> events.
    /// </summary>
    public class CourseAssetPostprocessorEventArgs : EventArgs
    {
    }
}
