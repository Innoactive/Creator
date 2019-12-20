using System;
using System.IO;
using Common.Logging;
using Innoactive.Hub.Training.Editors.Configuration;
using Innoactive.Hub.Training.Utils.Serialization;
using Innoactive.Hub.Unity.Tests.Training.Editor.EditorImguiTester;
using UnityEditor;
using UnityEngine;

namespace Innoactive.Hub.Training.Editors.Windows
{
    public static class SaveManager
    {
        private static readonly ILog logger = Logging.LogManager.GetLogger(typeof(SaveManager));

        /// <summary>
        /// Save the training to given path.
        /// </summary>
        public static bool SaveTrainingCourseToFile(ICourse course)
        {
            try
            {
                if (course == null)
                {
                    throw new NullReferenceException("The training course is not saved because it doesn't exist.");
                }

                string path = GetTrainingPath(course);

                string directory = Path.GetDirectoryName(path);
                if (string.IsNullOrEmpty(directory) == false && Directory.Exists(directory) == false)
                {
                    Directory.CreateDirectory(directory);
                }

                string serialized = JsonTrainingSerializer.Serialize(course);
                File.WriteAllText(path, serialized);
                // Check if saved as asset. If true, import it.
                TryReloadAssetByFullPath(path);
                return true;
            }
            catch (Exception e)
            {
                TestableEditorElements.DisplayDialog("Error while saving the training course!", e.ToString(), "Close");
                logger.Error(e);
                return false;
            }
        }

        [Obsolete("Path is now generated from ICourse, which removes the need of path.")]
        public static bool SaveTrainingCourseToFile(ICourse course, string path)
        {
            return SaveTrainingCourseToFile(course);
        }

        public static ICourse LoadTrainingCourseFromFile(string path)
        {
            string trainingData = File.ReadAllText(path);
            return JsonTrainingSerializer.Deserialize(trainingData);
        }

        public static string GetTrainingPath(string name)
        {
            name = Path.GetFileNameWithoutExtension(name);
            return string.Format("{0}/{1}/{2}/{2}.json", Application.streamingAssetsPath, EditorConfigurator.Instance.DefaultCourseStreamingAssetsFolder, name).Replace('/', Path.DirectorySeparatorChar);
        }

        private static string GetTrainingPath(ICourse course)
        {
            return GetTrainingPath(course.Data.Name);
        }

        private static void TryReloadAssetByFullPath(string path)
        {
            string pathUri = new Uri(path).ToString();
            string assetsUri = new Uri(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Assets").ToString();
            if (pathUri.Length > assetsUri.Length && pathUri.StartsWith(assetsUri))
            {
                // Path separator is always '/' https://tools.ietf.org/html/rfc3986
                AssetDatabase.ImportAsset("Assets" + pathUri.Substring(assetsUri.Length));
            }
        }
    }
}
