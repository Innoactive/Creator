using System;
using System.IO;
using System.Linq;
using Innoactive.Creator.Core.IO;
using Innoactive.CreatorEditor.Configuration;
using Innoactive.CreatorEditor.Utils;
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
            return assets.Where(File.Exists).Where(FileUtils.IsTrainingCourseFile).ToArray();
        }
    }

    /// <summary>
    /// Event args for <see cref="CourseAssetPostprocessor"/> events.
    /// </summary>
    public class CourseAssetPostprocessorEventArgs : EventArgs
    {
    }
}
