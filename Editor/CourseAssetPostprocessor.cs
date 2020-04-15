using System;
using System.Collections.Generic;
using System.Linq;
using Innoactive.CreatorEditor.Utils;
using UnityEditor;

namespace Innoactive.CreatorEditor
{
    /// <summary>
    /// Monitors training course files added or removed from the project.
    /// </summary>
    internal class CourseAssetPostprocessor : AssetPostprocessor
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

        private static IEnumerable<string> GetTrainingCourseAssets(IEnumerable<string> assets)
        {
            return assets.Where(CourseUtils.IsCourseFile).ToArray();
        }
    }

    /// <summary>
    /// Event args for <see cref="CourseAssetPostprocessor"/> events.
    /// </summary>
    internal class CourseAssetPostprocessorEventArgs : EventArgs
    {
    }
}
