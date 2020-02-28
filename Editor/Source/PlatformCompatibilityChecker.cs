#if UNITY_ANDROID
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Innoactive.Creator.Editors.Utils
{
    [InitializeOnLoad]
    public static class PlatformCompatibilityChecker
    {
        private const string ReferenceFileName = "csc.rsp";
        private static readonly string[] Dependencies = { "-r:System.IO.Compression.dll", "-r:System.IO.Compression.FileSystem.dll" };

        static PlatformCompatibilityChecker()
        {
            string filePath = Path.Combine(Application.dataPath, ReferenceFileName);

            if (File.Exists(filePath) )
            {
                CheckDependencies(filePath);
            }
            else
            {
                CreateReferenceFile(filePath);
            }
        }

        private static void CheckDependencies(string filePath)
        {
            List<string> dependencyList = File.ReadAllLines(filePath).ToList();
            int numberOfCurrentDependences = dependencyList.Count;

            foreach (string dependency in Dependencies)
            {
                if (dependencyList.All(d => d != dependency))
                {
                    dependencyList.Add(dependency);
                }
            }

            if (numberOfCurrentDependences < dependencyList.Count)
            {
                CreateReferenceFile(filePath);
            }
        }

        private static void CreateReferenceFile(string filePath)
        {
            File.WriteAllLines(filePath, Dependencies);
        }
    }
}
#endif
