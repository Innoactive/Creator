using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Innoactive.Creator.Core.Utils;
using UnityEditor;
using Debug = UnityEngine.Debug;

namespace Innoactive.CreatorEditor.PackageManager
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

        private static Queue<Dependency> dependencies;
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

            IEnumerable<Type> dependenciesTypes = ReflectionUtils.GetConcreteImplementationsOf<Dependency>();
            dependenciesList = new List<Dependency>();

            foreach (Type dependencyType in dependenciesTypes)
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
                dependencies = new Queue<Dependency>(dependenciesList);
                ProcessDependencies();
            }
        }

        private static async void ProcessDependencies()
        {
            while (PackageOperationsManager.Packages == null || PackageOperationsManager.Packages.Any() == false || EditorApplication.isCompiling)
            {
                await Task.Delay(100);
            }

            while (dependencies.Any())
            {
                Dependency dependency = dependencies.Peek();

                if (PackageOperationsManager.IsPackageLoaded(dependency.Package))
                {
                    dependency.IsEnabled = true;
                    dependencies.Dequeue();
                }
                else
                {
                    PackageOperationsManager.LoadPackage(dependency.Package);
                    return;
                }
            }

            OnPostProcess?.Invoke(null, new DependenciesEnabledEventArgs(dependenciesList));
        }
    }
}
