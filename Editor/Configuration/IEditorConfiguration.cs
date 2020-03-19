using System.Collections.ObjectModel;
using Innoactive.Creator.Core.Behaviors;
using Innoactive.Creator.Core.Conditions;
using Innoactive.Creator.Core.Serialization;
using Innoactive.CreatorEditor.UI;
using UnityEditor;

namespace Innoactive.CreatorEditor.Configuration
{
    /// <summary>
    /// Interface for editor configuration definition. Implement it to create your own.
    /// </summary>
    public interface IEditorConfiguration
    {
        /// <summary>
        /// Absolute path where training courses are stored.
        /// </summary>
        string DefaultCourseStreamingAssetsFolder { get; }

        /// <summary>
        /// Serializer used to serialize training courses.
        /// </summary>
        ITrainingSerializer Serializer { get; }

        /// <summary>
        /// Assets path where to save the serialized <see cref="AllowedMenuItemsSettings"/> file.
        /// It has to start with "Assets/".
        /// </summary>
        string AllowedMenuItemsSettingsAssetPath { get; }

        /// <summary>
        /// The current instance of the <see cref="AllowedMenuItemsSettings"/> object.
        /// It manages the display status of all available behaviors and conditions.
        /// </summary>
        AllowedMenuItemsSettings AllowedMenuItemsSettings { get; set; }

        /// <summary>
        /// List of available options in "Add new behavior" dropdown.
        /// </summary>
        ReadOnlyCollection<StepInspectorMenu.Option<IBehavior>> BehaviorsMenuContent { get; }

        /// <summary>
        /// List of available options in "Add new condition" dropdown.
        /// </summary>
        ReadOnlyCollection<StepInspectorMenu.Option<ICondition>> ConditionsMenuContent { get; }

        /// <summary>
        /// Setup the current unity scene to be a functioning training scene.
        /// </summary>
        void SetupTrainingScene();
    }
}
