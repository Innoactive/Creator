using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Innoactive.Creator.Core.Editor.Source.Utils;
using Innoactive.Hub.Training.Editors.Configuration;
using Innoactive.Hub.Unity.Tests.Training.Editor.EditorImguiTester;

namespace Innoactive.Hub.Training.Editors.Windows
{
    public static class SaveManager
    {
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

                string path = EditorCourseUtils.GetTrainingPath(course);

                string directory = Path.GetDirectoryName(path);
                if (string.IsNullOrEmpty(directory) == false && Directory.Exists(directory) == false)
                {
                    Directory.CreateDirectory(directory);
                }

                byte[] serialized = EditorConfigurator.Instance.Serializer.ToByte(course);
                File.WriteAllBytes(path, serialized);
                // Check if saved as asset. If true, import it.
                TryReloadAssetByFullPath(path);
                return true;
            }
            catch (Exception e)
            {
                TestableEditorElements.DisplayDialog("Error while saving the training course!", e.ToString(), "Close");
                Debug.LogError(e);
                return false;
            }
        }

        public static ICourse LoadTrainingCourseFromFile(string path)
        {
            byte[] trainingData = File.ReadAllBytes(path);
            return EditorConfigurator.Instance.Serializer.ToCourse(trainingData);
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
