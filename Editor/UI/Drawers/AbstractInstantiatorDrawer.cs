using System;
using System.Collections.Generic;
using System.Linq;
using Innoactive.CreatorEditor.Configuration;
using Innoactive.CreatorEditor.ImguiTester;

namespace Innoactive.CreatorEditor.UI.Drawers
{
    public abstract class AbstractInstantiatorDrawer<T> : AbstractDrawer
    {
        protected IList<TestableEditorElements.MenuOption> ConvertFromConfigurationOptionsToGenericMenuOptions(IList<StepInspectorMenu.Option<T>> options, object currentValue, Action<object> changeValueCallback)
        {
            return options.Select<StepInspectorMenu.Option<T>, TestableEditorElements.MenuOption>(menuOption =>
            {
                StepInspectorMenu.Separator<T> separator = menuOption as StepInspectorMenu.Separator<T>;
                StepInspectorMenu.DisabledItem<T> disabled = menuOption as StepInspectorMenu.DisabledItem<T>;
                StepInspectorMenu.Item<T> item = menuOption as StepInspectorMenu.Item<T>;

                if (separator != null)
                {
                    return new TestableEditorElements.MenuSeparator(separator.PathToSubmenu);
                }

                if (disabled != null)
                {
                    return new TestableEditorElements.DisabledMenuItem(disabled.Label);
                }

                if (item != null)
                {
                    return new TestableEditorElements.MenuItem(item.DisplayedName, false, () => ChangeValue(() => item.GetNewItem(), () => currentValue, changeValueCallback));
                }

                throw new InvalidCastException("There is a closed list of implementations of AddItemMenuOption.");
            }).ToList();
        }
    }
}
