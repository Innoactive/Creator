using System;
using Innoactive.Creator.Core.Configuration.Modes;
using Innoactive.Creator.Core.Utils;
using Innoactive.Creator.Unity;
using UnityEngine;

namespace Innoactive.Creator.Core.Configuration
{
    /// <summary>
    /// Configurator to set the training runtime configuration which is used by a training course during its execution.
    /// There has to be one and only one training runtime configurator game object per scene.
    /// </summary>
    public sealed class RuntimeConfigurator : UnitySceneSingleton<RuntimeConfigurator>
    {
        /// <summary>
        /// Fully qualified name of the runtime configuration used.
        /// This field is magically filled by <see cref="RuntimeConfiguratorEditor"/>
        /// </summary>
        public string RuntimeConfigurationName = typeof(DefaultRuntimeConfiguration).AssemblyQualifiedName;

        /// <summary>
        /// Course name which is selected.
        /// This field is magically filled by <see cref="RuntimeConfiguratorEditor"/>
        /// </summary>
        public string SelectedCourse;

        private IRuntimeConfiguration runtimeConfiguration;

        /// <summary>
        /// The event that fires when a training mode or runtime configuration changes.
        /// </summary>
        public static event EventHandler<ModeChangedEventArgs> ModeChanged;

        /// <summary>
        /// The event that fires when a training runtime configuration changes.
        /// </summary>
        public static event EventHandler<EventArgs> RuntimeConfigurationChanged;

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

                Type type = ReflectionUtils.GetTypeFromAssemblyQualifiedName(Instance.RuntimeConfigurationName);

                if (type == null)
                {
                    Debug.LogErrorFormat("IRuntimeConfiguration type '{0}' cannot be found. Using '{1}' instead.", Instance.RuntimeConfigurationName, typeof(DefaultRuntimeConfiguration).AssemblyQualifiedName);
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
                    Instance.runtimeConfiguration.Modes.ModeChanged -= RuntimeConfigurationModeChanged;
                }

                value.Modes.ModeChanged += RuntimeConfigurationModeChanged;

                Instance.RuntimeConfigurationName = value.GetType().AssemblyQualifiedName;
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
            return Instance.SelectedCourse;
        }

        /// <summary>
        /// Sets the path to the selected training course.
        /// </summary>
        public static void SetSelectedTrainingCourse(string path)
        {
            Instance.SelectedCourse = path;
        }

        protected override void Awake()
        {
            base.Awake();
            Configuration.SceneObjectRegistry.RegisterAll();
            RuntimeConfigurationChanged += HandleRuntimeConfigurationChanged;
        }

        protected override void OnDestroy()
        {
            ModeChanged = null;
            RuntimeConfigurationChanged = null;

            base.OnDestroy();
        }

        private static void EmitModeChanged()
        {
            ModeChanged?.Invoke(Instance, new ModeChangedEventArgs(Instance.runtimeConfiguration.Modes.CurrentMode));
        }

        private static void EmitRuntimeConfigurationChanged()
        {
            RuntimeConfigurationChanged?.Invoke(Instance, EventArgs.Empty);
        }

        private void HandleRuntimeConfigurationChanged(object sender, EventArgs args)
        {
            EmitModeChanged();
        }

        private static void RuntimeConfigurationModeChanged(object sender, ModeChangedEventArgs modeChangedEventArgs)
        {
            EmitModeChanged();
        }

        protected override string GetName()
        {
            return SceneUtils.TrainingConfigurationName;
        }
    }
}
