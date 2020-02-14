using System;
using UnityEngine;
using Innoactive.Hub.Logging;
using Innoactive.Hub.Training.Utils;

namespace Innoactive.Hub.Training.Configuration
{
    public class RuntimeConfigurationChangedEventArgs : EventArgs
    {
    }

    /// <summary>
    /// Configurator to set the training runtime configuration which is used by a training course during its execution.
    /// There has to be one and only one training runtime configurator game object per scene.
    /// </summary>
    public sealed class RuntimeConfigurator : MonoBehaviour
    {
        [SerializeField]
        private string runtimeConfigurationName = typeof(DefaultRuntimeConfiguration).AssemblyQualifiedName;

        [SerializeField]
        private string selectedCourseStreamingAssetsPath;

        private IRuntimeConfiguration runtimeConfiguration;

        private static RuntimeConfigurator instance;

        private static RuntimeConfigurator LookUpForGameObject()
        {
            RuntimeConfigurator[] instances = FindObjectsOfType<RuntimeConfigurator>();

            if (instances.Length > 1)
            {
                Debug.LogError("More than one training runtime configurator is found in the scene. Taking the first one. This may lead to unexpected behaviour.");
            }

            if (instances.Length == 0)
            {
                return null;
            }

            return instances[0];
        }

        private static void EmitModeChanged()
        {
            if (ModeChanged != null)
            {
                ModeChanged(Instance, new ModeChangedEventArgs(Instance.runtimeConfiguration.GetCurrentMode()));
            }
        }

        private static void EmitRuntimeConfigurationChanged()
        {
            if (RuntimeConfigurationChanged != null)
            {
                RuntimeConfigurationChanged(Instance, new RuntimeConfigurationChangedEventArgs());
            }
        }

        /// <summary>
        /// The event that fires when a training mode or runtime configuration changes.
        /// </summary>
        public static event EventHandler<ModeChangedEventArgs> ModeChanged;

        /// <summary>
        /// The event that fires when a training runtime configuration changes.
        /// </summary>
        public static event EventHandler<RuntimeConfigurationChangedEventArgs> RuntimeConfigurationChanged;

        /// <summary>
        /// Checks if an training runtime configurator instance exists in scene.
        /// </summary>
        public static bool Exists
        {
            get
            {
                if (instance == null || instance.Equals(null))
                {
                    instance = LookUpForGameObject();
                }

                return (instance != null && instance.Equals(null) == false);
            }
        }

        private static RuntimeConfigurator Instance
        {
            get
            {
                if (Exists == false)
                {
                    throw new NullReferenceException("Training runtime configurator is not set in the scene. Create an empty game object with the 'RuntimeConfigurator' script attached to it.");
                }

                return instance;
            }
        }

        private void Awake()
        {
            Configuration.SceneObjectRegistry.RegisterAll();
            RuntimeConfigurationChanged += HandleRuntimeConfigurationChanged;
        }

        private void HandleRuntimeConfigurationChanged(object sender, RuntimeConfigurationChangedEventArgs e)
        {
            EmitModeChanged();
        }

        private void OnDestroy()
        {
            ModeChanged = null;
            RuntimeConfigurationChanged = null;
        }

        /// <summary>
        /// Shortcut to get the <see cref="IRuntimeConfiguration"/> of the instance.
        /// </summary>
        public static IRuntimeConfiguration Configuration
        {
            get
            {
                if (Instance.runtimeConfiguration != null)
                {
                    return Instance.runtimeConfiguration;
                }

                Type type = ReflectionUtils.GetTypeFromAssemblyQualifiedName(Instance.runtimeConfigurationName);

                if (type == null)
                {
                    Debug.LogErrorFormat("IRuntimeConfiguration type '{0}' cannot be found. Using '{1}' instead.", Instance.runtimeConfigurationName, typeof(DefaultRuntimeConfiguration).AssemblyQualifiedName);
                    type = typeof(DefaultRuntimeConfiguration);
                }

                Configuration = (IRuntimeConfiguration)ReflectionUtils.CreateInstanceOfType(type);
                return Instance.runtimeConfiguration;
            }
            set
            {
                if (value == null)
                {
                    Debug.LogError("Training runtime configuration cannot be null.");
                    return;
                }

                if (Instance.runtimeConfiguration == value)
                {
                    return;
                }

                if (Instance.runtimeConfiguration != null)
                {
                    Instance.runtimeConfiguration.ModeChanged -= RuntimeConfigurationModeChanged;
                }

                value.ModeChanged += RuntimeConfigurationModeChanged;

                Instance.runtimeConfigurationName = value.GetType().AssemblyQualifiedName;
                Instance.runtimeConfiguration = value;
                Configuration.SceneObjectRegistry.RegisterAll();

                EmitRuntimeConfigurationChanged();
            }
        }

        /// <summary>
        /// Returns the path to the selected training course.
        /// </summary>
        public static string GetSelectedTrainingCourse()
        {
            return Instance.selectedCourseStreamingAssetsPath;
        }

        /// <summary>
        /// Sets the path to the selected training course.
        /// </summary>
        public static void SetSelectedTrainingCourse(string path)
        {
            Instance.selectedCourseStreamingAssetsPath = path;
        }

        private static void RuntimeConfigurationModeChanged(object sender, ModeChangedEventArgs modeChangedEventArgs)
        {
            EmitModeChanged();
        }
    }
}
