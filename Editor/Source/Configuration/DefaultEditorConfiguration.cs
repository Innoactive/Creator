using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Innoactive.Creator.Utils;
using Innoactive.Hub.Training.Behaviors;
using Innoactive.Hub.Training.Conditions;
using Innoactive.Hub.Training.Utils;
using Innoactive.Hub.Training.Utils.Serialization;
using UnityEngine;

namespace Innoactive.Hub.Training.Editors.Configuration
{
    /// <summary>
    /// Default editor configuration definition which is used if no other was implemented.
    /// </summary>
    public class DefaultEditorConfiguration : IEditorConfiguration
    {
        private AllowedMenuItemsSettings allowedMenuItemsSettings;

        /// <inheritdoc />
        public virtual string DefaultCourseStreamingAssetsFolder
        {
            get { return "Training"; }
        }

        /// <inheritdoc />
        public virtual string AllowedMenuItemsSettingsAssetPath
        {
            get { return null; }
        }

        /// <inheritdoc />
        public virtual ITrainingSerializer Serializer
        {
            get { return new NewtonsoftJsonSerializer(); }
        }

        /// <inheritdoc />
        public virtual AllowedMenuItemsSettings AllowedMenuItemsSettings
        {
            get
            {
                if (allowedMenuItemsSettings == null)
                {
                    allowedMenuItemsSettings = AllowedMenuItemsSettings.Load();
                }

                return allowedMenuItemsSettings;
            }
            set { allowedMenuItemsSettings = value; }
        }

        protected DefaultEditorConfiguration()
        {
        }

        /// <inheritdoc />
        public virtual ReadOnlyCollection<Menu.Option<IBehavior>> BehaviorsMenuContent
        {
            get { return AllowedMenuItemsSettings.GetBehaviorMenuOptions().Cast<Menu.Option<IBehavior>>().ToList().AsReadOnly(); }
        }

        /// <inheritdoc />
        public virtual ReadOnlyCollection<Menu.Option<ICondition>> ConditionsMenuContent
        {
            get { return AllowedMenuItemsSettings.GetConditionMenuOptions().Cast<Menu.Option<ICondition>>().ToList().AsReadOnly(); }
        }

        /// <inheritdoc />
        public virtual void SetupTrainingScene()
        {
            // Create default save folder.
            Directory.CreateDirectory(DefaultCourseStreamingAssetsFolder);

            // Find and setup all OnSceneSetup classes in the project.
            IEnumerable<Type> types = ReflectionUtils.GetConcreteImplementationsOf<OnSceneSetup>();

            foreach (Type onSceneSetup in types)
            {
                try
                {
                    OnSceneSetup setup = ReflectionUtils.CreateInstanceOfType(onSceneSetup) as OnSceneSetup;

                    if (setup != null)
                    {
                        setup.Setup();
                    }
                }
                catch (Exception exception)
                {
                    Debug.LogErrorFormat("{0} while initializing OnSceneSetup object of type {1}. \n {2}", exception.GetType().Name, onSceneSetup, exception.StackTrace);
                }
            }

            Debug.Log("Scene setup is complete.");
        }
    }
}
