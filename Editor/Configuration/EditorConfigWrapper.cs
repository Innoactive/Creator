using System.Collections.ObjectModel;
using VPG.Core.Behaviors;
using VPG.Core.Conditions;
using VPG.Core.Serialization;
using VPG.Editor.UI.StepInspector.Menu;

namespace VPG.Editor.Configuration
{
    internal class EditorConfigWrapper : DefaultEditorConfiguration
    {
        private IEditorConfiguration config;

        public EditorConfigWrapper(IEditorConfiguration config)
        {
            this.config = config;
        }

        public override ICourseSerializer Serializer => config.Serializer;

        public override AllowedMenuItemsSettings AllowedMenuItemsSettings
        {
            get => config.AllowedMenuItemsSettings;
            set => config.AllowedMenuItemsSettings = value;
        }

        public override string CourseStreamingAssetsSubdirectory => config.CourseStreamingAssetsSubdirectory;

        public override string AllowedMenuItemsSettingsAssetPath => config.AllowedMenuItemsSettingsAssetPath;

        public override ReadOnlyCollection<MenuOption<IBehavior>> BehaviorsMenuContent => config.BehaviorsMenuContent;

        public override ReadOnlyCollection<MenuOption<ICondition>> ConditionsMenuContent => config.ConditionsMenuContent;

    }
}
