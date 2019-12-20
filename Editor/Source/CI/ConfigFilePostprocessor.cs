using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Callbacks;

namespace Innoactive.Hub.Config
{
    /// <summary>
    /// Copies all configs implementing IJsonConfig into a newly builded version of the app.
    /// </summary>
    public class ConfigFilePostprocessor
    {
        /// <summary>
        /// Base Class Type for the config files
        /// </summary>
        private static readonly Type BaseType = typeof(ConfigBase);

        /// <summary>
        /// Gets called after finishing a build in the editor
        /// Uses 'hacky' reflection, but doesn't matter much
        /// because it only gets called in the editor
        /// </summary>
        [PostProcessBuild]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    ProcessTypes(assembly.GetTypes(), pathToBuiltProject);
                }
                catch (ReflectionTypeLoadException exception)
                {
                    if (exception.Types != null)
                    {
                        ProcessTypes(exception.Types, pathToBuiltProject);
                    }
                }
            }
        }

        private static void ProcessTypes(IEnumerable<Type> types, string pathToBuiltProject)
        {
            foreach (Type type in types)
            {
                if (type != null && type.IsSubclassOf(BaseType))
                {
                    JsonConfigFileManager.CopyToBuildFolder((IJsonConfig) Activator.CreateInstance(type), pathToBuiltProject);
                }
            }
        }
    }
}
