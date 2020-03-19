using System.IO;
using Innoactive.CreatorEditor.Configuration;
using UnityEngine;

namespace Innoactive.CreatorEditor.Utils
{
    public static class FileUtils
    {
        /// <summary>
        /// Returns true if the given <paramref name="filePath"/> represents a training course file.
        /// </summary>
        public static bool IsTrainingCourseFile(string filePath)
        {
            string assetPath = Path.Combine(Application.dataPath.Remove(Application.dataPath.LastIndexOf('/')), filePath).Replace('/', Path.DirectorySeparatorChar);
            string courseFolderPath = Path.Combine(Application.streamingAssetsPath, EditorConfigurator.Instance.DefaultCourseStreamingAssetsFolder).Replace('/', Path.DirectorySeparatorChar);

            FileInfo file = new FileInfo(assetPath);

            return new DirectoryInfo(courseFolderPath).FullName == file.Directory.Parent.FullName && Path.GetFileNameWithoutExtension(filePath) == file.Directory.Name;
        }
    }
}
