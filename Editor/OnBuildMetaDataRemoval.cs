using System;
using System.IO;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Serialization;
using Innoactive.CreatorEditor.Configuration;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Innoactive.CreatorEditor
{
    [ExecuteInEditMode]
    internal class OnBuildMetaDataRemoval : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public int callbackOrder { get; } = Int32.MaxValue;

        public void OnPreprocessBuild(BuildReport report)
        {
            if (EditorConfigurator.Instance.Serializer is IOnBuildExportSerialization serializer)
            {
                foreach (string courseName in CourseAssetUtils.GetAllCourses())
                {
                    ICourse course = CourseAssetManager.Load(courseName);

                    string filePath = CourseAssetUtils.GetCourseAssetPath(courseName);
                    string tmpFilePath = Path.Combine(CourseAssetUtils.GetCourseAssetDirectory(courseName), $".{courseName}.tmp.meta");

                    File.Delete(tmpFilePath);
                    File.Move(filePath, tmpFilePath);

                    try
                    {
                        byte[] serialized = serializer.ConvertTrainingCourseForExport(course);

                        FileStream stream = File.Create(filePath);
                        stream.Write(serialized, 0, serialized.Length);
                        stream.Close();
                        Debug.Log($"Converted course {filePath}");
                    }
                    catch (Exception ex)
                    {

                        Debug.LogError($"Failed to export course '{courseName}'\n{ex.Message}\n{ex.StackTrace}");
                    }
                }
            }
            EditorApplication.update += Update;
        }

        private void Update()
        {
            if (BuildPipeline.isBuildingPlayer == false)
            {
                EditorApplication.update -= Update;
                CleanUpTempFiles();
            }
        }

        internal static void CleanUpTempFiles()
        {
            foreach (string courseName in CourseAssetUtils.GetAllPossibleCourseFolder())
            {
                try
                {
                    string filePath = CourseAssetUtils.GetCourseAssetPath(courseName);
                    string tmpFilePath = Path.Combine(CourseAssetUtils.GetCourseAssetDirectory(courseName), $".{courseName}.tmp.meta");

                    if (File.Exists(tmpFilePath))
                    {
                        File.Delete(filePath);
                        File.Move(tmpFilePath, filePath);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
            }
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            CleanUpTempFiles();
        }
    }
}
