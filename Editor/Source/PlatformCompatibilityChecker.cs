#if UNITY_ANDROID
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Innoactive.Creator.Editors.Utils
{
    /// <summary>
    /// Validates that full .Net dependencies are referenced by Unity.
    /// </summary>
    /// <remarks>See more: https://docs.unity3d.com/Manual/dotnetProfileAssemblies.html</remarks>
    [InitializeOnLoad]
    public static class PlatformCompatibilityChecker
    {
        private const string ReferenceFileName = "csc.rsp";
        private static readonly string[] Dependencies = { "-r:System.IO.Compression.dll", "-r:System.IO.Compression.FileSystem.dll" };

        static PlatformCompatibilityChecker()
        {
            CheckDependencies();
        }

        private static void CheckDependencies()
        {
            string filePath = Path.Combine(Application.dataPath, ReferenceFileName);

            if (File.Exists(filePath))
            {
                AreDependenciesUpdated(filePath);
            }
            else
            {
                CreateReferenceFile(filePath, Dependencies);
            }
        }

        private static void AreDependenciesUpdated(string filePath)
        {
            List<string> dependencyList = File.ReadAllLines(filePath).ToList();
            int numberOfCurrentDependencies = dependencyList.Count;

            foreach (string dependency in Dependencies)
            {
                if (dependencyList.All(d => d != dependency))
                {
                    dependencyList.Add(dependency);
                }
            }

            if (numberOfCurrentDependencies < dependencyList.Count)
            {
                CreateReferenceFile(filePath, dependencyList);
            }
        }

        private static void CreateReferenceFile(string filePath, IEnumerable<string> dependencyList)
        {

            File.WriteAllLines(filePath, dependencyList);
            Debug.LogWarningFormat("Required compatibility file was created at path {0}", filePath);
        }
    }
}
#endif
