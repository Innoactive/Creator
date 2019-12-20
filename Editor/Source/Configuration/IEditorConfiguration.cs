using System.Collections.ObjectModel;
using Innoactive.Hub.Training.Behaviors;
using Innoactive.Hub.Training.Conditions;

namespace Innoactive.Hub.Training.Editors.Configuration
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
        ReadOnlyCollection<Menu.Option<IBehavior>> BehaviorsMenuContent { get; }

        /// <summary>
        /// List of available options in "Add new condition" dropdown.
        /// </summary>
        ReadOnlyCollection<Menu.Option<ICondition>> ConditionsMenuContent { get; }

        /// <summary>
        /// Setup the current unity scene to be a functioning training scene.
        /// </summary>
        void SetupTrainingScene();
    }
}
