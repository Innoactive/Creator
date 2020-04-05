using System;
using System.Collections.Generic;
using System.Linq;
using Innoactive.Creator.Core.Utils;
using UnityEditor;
using Debug = UnityEngine.Debug;

namespace Innoactive.CreatorEditor.PackageManager
{
    /// <summary>
    ///
    /// </summary>
    [InitializeOnLoad]
    public static class DependencyManager
    {
        /// <summary>
        ///
        /// </summary>
        public static event EventHandler<DependenciesEnabledEventArgs> OnPostProcess;

        private static Queue<Dependency> dependencies;
        private static List<Dependency> dependenciesList;

        static DependencyManager()
        {
            GatherDependencyList();
            if (dependencies != null && dependencies.Any())
            {
                EditorApplication.update += EditorUpdate;
            }
        }

        private static void EditorUpdate()
        {
            if (EditorApplication.isCompiling || EditorApplication.isPlaying || PackageOperationsManager.Packages == null || PackageOperationsManager.Packages.Any() == false)
            {
                return;
            }

            Dependency dependency = dependencies.Peek();

            if (PackageOperationsManager.IsPackageLoaded(dependency.Package))
            {
                dependency.IsEnabled = true;
                dependencies.Dequeue();
            }
            else
            {
                EditorPrefs.SetBool(dependency.GetType().Name, true);
                PackageOperationsManager.LoadPackage(dependency.Package);
                EditorApplication.update -= EditorUpdate;
            }

            if (dependencies.Any() == false)
            {
                EditorApplication.update -= EditorUpdate;

                if (dependenciesList.Any(loadedDependency => EditorPrefs.GetBool(loadedDependency.GetType().Name)))
                {
                    foreach (Dependency loadedDependency in dependenciesList)
                    {
                        EditorPrefs.DeleteKey(loadedDependency.GetType().Name);
                    }

                    OnPostProcess?.Invoke(null, new DependenciesEnabledEventArgs(dependenciesList));
                }
            }
        }

        private static void GatherDependencyList()
        {
            IEnumerable<Type> dependenciesTypes = ReflectionUtils.GetConcreteImplementationsOf<Dependency>();
            dependenciesList = new List<Dependency>();

            foreach (Type dependencyType in dependenciesTypes)
            {
                try
                {
                    Dependency dependencyInstance = ReflectionUtils.CreateInstanceOfType(dependencyType) as Dependency;

                    if (dependencyInstance != null)
                    {
                        dependenciesList.Add(dependencyInstance);
                    }
                }
                catch (Exception exception)
                {
                    Debug.LogErrorFormat("{0} while retrieving Dependency object of type {1}.\n{2}", exception.GetType().Name, dependencyType.Name, exception.StackTrace);
                }
            }

            if (dependenciesList.Count > 0)
            {
                dependenciesList = dependenciesList.OrderBy(setup => setup.Priority).ToList();
                dependencies = new Queue<Dependency>(dependenciesList);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class DependenciesEnabledEventArgs : EventArgs
    {
        public readonly List<Dependency> DependenciesList;

        public DependenciesEnabledEventArgs(List<Dependency> dependenciesList)
        {
            DependenciesList = dependenciesList;
        }
    }
}
