using System;
using VPG.Core.Configuration.Modes;
using VPG.Core.IO;
using VPG.Core.RestrictiveEnvironment;
using VPG.Core.SceneObjects;
using VPG.Core.Serialization;
using UnityEngine;

namespace VPG.Core.Configuration
{
    /// <summary>
    /// Base class for your runtime course configuration. Extend it to create your own.
    /// </summary>
#pragma warning disable 0618
    public abstract class BaseRuntimeConfiguration : IRuntimeConfiguration
    {
#pragma warning restore 0618
        private ISceneObjectRegistry sceneObjectRegistry;

        /// <inheritdoc />
        public virtual ISceneObjectRegistry SceneObjectRegistry
        {
            get
            {
                if (sceneObjectRegistry == null)
                {
                    sceneObjectRegistry = new SceneObjectRegistry();
                }

                return sceneObjectRegistry;
            }
        }

        /// <inheritdoc />
        public ICourseSerializer Serializer { get; set; } = new ImprovedNewtonsoftJsonCourseSerializer();

        /// <summary>
        /// Default input action asset which is used when no customization of key bindings are done.
        /// Should be stored inside the VR Process Gizmo package.
        /// </summary>
        public virtual string DefaultInputActionAssetPath { get; } = "KeyBindings/CreatorDefaultKeyBindings";

        /// <summary>
        /// Custom InputActionAsset path which is used when key bindings are modified.
        /// Should be stored in project path.
        /// </summary>
        public virtual string CustomInputActionAssetPath { get; } = "KeyBindings/CreatorCustomKeyBindings";

#if ENABLE_INPUT_SYSTEM
        private UnityEngine.InputSystem.InputActionAsset inputActionAsset;

        /// <summary>
        /// Current active InputActionAsset.
        /// </summary>
        public virtual UnityEngine.InputSystem.InputActionAsset CurrentInputActionAsset
        {
            get
            {
                if (inputActionAsset == null)
                {
                    inputActionAsset = Resources.Load<UnityEngine.InputSystem.InputActionAsset>(CustomInputActionAssetPath);
                    if (inputActionAsset == null)
                    {
                        inputActionAsset = Resources.Load<UnityEngine.InputSystem.InputActionAsset>(DefaultInputActionAssetPath);
                    }
                }

                return inputActionAsset;
            }

            set => inputActionAsset = value;
        }
#endif

        /// <inheritdoc />
        public IModeHandler Modes { get; protected set; }

        /// <inheritdoc />
        public abstract TrainingSceneObject Trainee { get; }

        /// <inheritdoc />
        public abstract AudioSource InstructionPlayer { get; }

        /// <summary>
        /// Determines the property locking strategy used for this runtime configuration.
        /// </summary>
        public StepLockHandlingStrategy StepLockHandling { get; set; }

        protected BaseRuntimeConfiguration() : this (new DefaultStepLockHandling())
        {
        }

        protected BaseRuntimeConfiguration(StepLockHandlingStrategy lockHandling)
        {
            StepLockHandling = lockHandling;
        }

        /// <inheritdoc />
        public virtual ICourse LoadCourse(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Given path is null or empty!");
            }

            byte[] serialized = FileManager.Read(path);
            return Serializer.CourseFromByteArray(serialized);
        }
    }
}
