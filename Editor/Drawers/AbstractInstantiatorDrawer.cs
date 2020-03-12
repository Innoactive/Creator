using System;
using System.Collections.Generic;
using System.Linq;
using Innoactive.Hub.Training.Editors.Configuration;
using Innoactive.Hub.Unity.Tests.Training.Editor.EditorImguiTester;

namespace Innoactive.Hub.Training.Editors.Drawers
{
    public abstract class AbstractInstantiatorDrawer<T> : AbstractDrawer
    {
        protected IList<TestableEditorElements.MenuOption> ConvertFromConfigurationOptionsToGenericMenuOptions(IList<Menu.Option<T>> options, object currentValue, Action<object> changeValueCallback)
        {
            return options.Select<Menu.Option<T>, TestableEditorElements.MenuOption>(menuOption =>
            {
                Menu.Separator<T> separator = menuOption as Menu.Separator<T>;
                Menu.DisabledItem<T> disabled = menuOption as Menu.DisabledItem<T>;
                Menu.Item<T> item = menuOption as Menu.Item<T>;

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
