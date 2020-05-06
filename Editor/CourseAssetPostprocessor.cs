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
            if (CourseFileStructureChanged != null &&
                importedAssets.Concat(deletedAssets)
                    .Concat(movedAssets)
                    .Concat(movedFromAssetPaths)
                    .Any(CourseAssetManager.IsValidCourseAssetPath))
            {
                CourseFileStructureChanged.Invoke(null, new CourseAssetPostprocessorEventArgs());
            }
        }
    }

    /// <summary>
    /// Event args for <see cref="CourseAssetPostprocessor"/> events.
    /// </summary>
    internal class CourseAssetPostprocessorEventArgs : EventArgs
    {
    }
}
