using System;
using System.Linq;
using Innoactive.Creator.Core.Tabs;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Drawers
{
    [DefaultTrainingDrawer(typeof(ITabsGroup))]
    internal class TabsGroupDrawer : AbstractDrawer
    {
        public override Rect Draw(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label)
        {
            ITabsGroup tabsGroup = (ITabsGroup)currentValue;

            GUIContent[] labels = tabsGroup.Tabs.Select(tab => tab.Label).ToArray();

            int oldSelected = tabsGroup.Selected;

            Rect nextPosition = new Rect(rect.x, rect.y, rect.width, EditorStyles.toolbar.fixedHeight);

            int selected = GUI.Toolbar(nextPosition, oldSelected, labels);

            if (selected != oldSelected)
            {
                ChangeValue(() =>
                    {
                        tabsGroup.Selected = selected;
                        return tabsGroup;
                    },
                    () =>
                    {
                        tabsGroup.Selected = oldSelected;
                        return tabsGroup;
                    },
                    changeValueCallback);
            }

            float height = EditorStyles.toolbar.fixedHeight;
            nextPosition.position += new Vector2(0, height);
            return DrawerLocator.GetDrawerForValue(tabsGroup.Tabs[tabsGroup.Selected].GetValue(), typeof(object))
                .Draw(nextPosition,
                    tabsGroup.Tabs[tabsGroup.Selected].GetValue(),
                    (newValue) =>
                    {
                        tabsGroup.Tabs[tabsGroup.Selected].SetValue(newValue);
                        changeValueCallback(tabsGroup);
                    },
                    GUIContent.none);
        }
    }
}
