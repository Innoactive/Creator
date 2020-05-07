using System;
using System.Linq;
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
            if (CourseFileStructureChanged != null &&
                importedAssets.Concat(deletedAssets)
                    .Concat(movedAssets)
                    .Concat(movedFromAssetPaths)
                    .Any(CourseAssetUtils.IsValidCourseAssetPath))
            {
                CourseFileStructureChanged.Invoke(null, new CourseAssetPostprocessorEventArgs());
            }
        }
    }
}
