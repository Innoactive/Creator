using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Innoactive.Creator.Core.Behaviors;
using Innoactive.Creator.Core.Conditions;
using Innoactive.Creator.Core.Utils;
using Innoactive.CreatorEditor.UI;
using Innoactive.Creator.Core.Serialization;
using Innoactive.Creator.Core.Serialization.NewtonsoftJson;
using UnityEngine;

namespace Innoactive.CreatorEditor.Configuration
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
        public virtual ICourseSerializer Serializer
        {
            get { return new NewtonsoftJsonCourseSerializer(); }
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
        public virtual ReadOnlyCollection<StepInspectorMenu.Option<IBehavior>> BehaviorsMenuContent
        {
            get { return AllowedMenuItemsSettings.GetBehaviorMenuOptions().Cast<StepInspectorMenu.Option<IBehavior>>().ToList().AsReadOnly(); }
        }

        /// <inheritdoc />
        public virtual ReadOnlyCollection<StepInspectorMenu.Option<ICondition>> ConditionsMenuContent
        {
            get { return AllowedMenuItemsSettings.GetConditionMenuOptions().Cast<StepInspectorMenu.Option<ICondition>>().ToList().AsReadOnly(); }
        }

        /// <inheritdoc />
        public virtual void SetupTrainingScene()
        {
            // Create default save folder.
            Directory.CreateDirectory(DefaultCourseStreamingAssetsFolder);

            // Find and setup all OnSceneSetup classes in the project.
            IEnumerable<Type> types = ReflectionUtils.GetConcreteImplementationsOf<OnSceneSetup>();
            List<OnSceneSetup> onSceneSetups = new List<OnSceneSetup>();
            HashSet<string> initializedKeys = new HashSet<string>();

            foreach (Type onSceneSetupType in types)
            {
                try
                {
                    OnSceneSetup onSceneSetup = ReflectionUtils.CreateInstanceOfType(onSceneSetupType) as OnSceneSetup;

                    if (onSceneSetup != null)
                    {
                        onSceneSetups.Add(onSceneSetup);

                        if (onSceneSetup.Key != null && initializedKeys.Add(onSceneSetup.Key) == false)
                        {
                            Debug.LogWarningFormat("Multiple scene setups with key {0} found during Scene setup. This might cause problems and you might consider using only one.", onSceneSetup.Key);
                        }
                    }
                }
                catch (Exception exception)
                {
                    Debug.LogErrorFormat("{0} while initializing OnSceneSetup object of type {1}.\n{2}", exception.GetType().Name, onSceneSetupType.Name, exception.StackTrace);
                }
            }

            onSceneSetups = onSceneSetups.OrderBy(setup => setup.Priority).ToList();

            foreach (OnSceneSetup onSceneSetup in onSceneSetups)
            {
                try
                {
                    onSceneSetup.Setup();
                    Debug.LogFormat("Scene Setup done for {0}", onSceneSetup);
                }
                catch (Exception exception)
                {
                    Debug.LogErrorFormat("{0} while initializing OnSceneSetup object of type {1}.\n{2}", exception.GetType().Name, onSceneSetup.GetType().Name, exception.StackTrace);
                }
            }

            Debug.Log("Scene setup is complete.");
        }
    }
}
