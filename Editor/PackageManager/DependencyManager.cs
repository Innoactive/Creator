﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using VPG.Core.Utils;
using UnityEditor;
using Debug = UnityEngine.Debug;

namespace VPG.Editor.PackageManager
{
    /// <summary>
    /// Automatically retrieves all dependencies from the Unity's Package Manager at the startup.
    /// </summary>
    [InitializeOnLoad]
    public class DependencyManager
    {
        public class DependenciesEnabledEventArgs : EventArgs
        {
            public readonly List<Dependency> DependenciesList;

            public DependenciesEnabledEventArgs(List<Dependency> dependenciesList)
            {
                DependenciesList = dependenciesList;
            }
        }

        /// <summary>
        /// Emitted when all found <see cref="Dependency"/> were installed.
        /// </summary>
        public static event EventHandler<DependenciesEnabledEventArgs> OnPostProcess;

        private static List<Dependency> dependenciesList;

        static DependencyManager()
        {
            GatherDependencies();
        }

        private static void GatherDependencies()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            dependenciesList = new List<Dependency>();

            foreach (Type dependencyType in ReflectionUtils.GetConcreteImplementationsOf<Dependency>())
            {
                try
                {
                    if (ReflectionUtils.CreateInstanceOfType(dependencyType) is Dependency dependencyInstance && string.IsNullOrEmpty(dependencyInstance.Package) == false)
                    {
                        dependenciesList.Add(dependencyInstance);
                    }
                }
                catch (Exception exception)
                {
                    Debug.LogErrorFormat("{0} while retrieving Dependency object of type {1}.\n{2}", exception.GetType().Name, dependencyType.Name, exception.StackTrace);
                }
            }

            if (dependenciesList.Any())
            {
                dependenciesList = dependenciesList.OrderBy(setup => setup.Priority).ToList();
                ProcessDependencies();
            }
        }

        private static async void ProcessDependencies()
        {
            while (PackageOperationsManager.Packages == null || PackageOperationsManager.Packages.Any() == false || EditorApplication.isCompiling)
            {
                await Task.Delay(100);
            }

            float percentage = 100f / dependenciesList.Count;

            foreach (Dependency dependency in dependenciesList)
            {
                int index = dependenciesList.FindIndex(item => item == dependency);
                EditorUtility.DisplayProgressBar("Importing Creator dependencies", $"Fetching {dependency.Package}", index * percentage);

                if (PackageOperationsManager.IsPackageLoaded(dependency.Package, dependency.Version))
                {
                    if (string.IsNullOrEmpty(dependency.Version))
                    {
                        dependency.Version = PackageOperationsManager.GetInstalledPackageVersion(dependency.Package);
                    }

                    dependency.IsEnabled = true;
                }
                else
                {
                    PackageOperationsManager.LoadPackage(dependency.Package, dependency.Version);
                    return;
                }
            }

            EditorUtility.ClearProgressBar();
            OnPostProcess?.Invoke(null, new DependenciesEnabledEventArgs(dependenciesList));
        }
    }
}
