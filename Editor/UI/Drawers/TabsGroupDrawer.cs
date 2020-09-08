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

            // Draw tabs selector.
            float tabsHeight = DrawToolbox(rect, tabsGroup, changeValueCallback).height;

            // Get drawer for the object under the tab.
            ITrainingDrawer tabValueDrawer = DrawerLocator.GetDrawerForValue(tabsGroup.Tabs[tabsGroup.Selected].GetValue(), typeof(object));

            void ChangeValueCallback(object newValue)
            {
                tabsGroup.Tabs[tabsGroup.Selected].SetValue(newValue);
                changeValueCallback(tabsGroup);
            }

            Rect tabValueRect = new Rect(rect.x, rect.y + tabsHeight, rect.width, 0);

            for (int i = 0; i < tabsGroup.Tabs.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    if (i == tabsGroup.Selected)
                    {
                        // Draw the object under the tab.
                        rect.height = tabsHeight + tabValueDrawer.Draw(tabValueRect, tabsGroup.Tabs[tabsGroup.Selected].GetValue(), ChangeValueCallback, GUIContent.none).height;

                    }
                }
                EditorGUILayout.EndHorizontal();
            }


            return rect;
        }

        private Rect DrawToolbox(Rect rect, ITabsGroup tabsGroup, Action<object> changeValueCallback)
        {
            rect.height = EditorStyles.toolbar.fixedHeight;

            GUIContent[] labels = tabsGroup.Tabs.Select(tab => tab.Label).ToArray();

            int oldSelected = tabsGroup.Selected;

            int selected = GUI.Toolbar(rect, oldSelected, labels);

            if (selected != oldSelected)
            {
                ChangeValue(() =>
                    {
                        tabsGroup.Tabs[oldSelected].OnUnselect();
                        tabsGroup.Tabs[selected].OnSelected();
                        tabsGroup.Selected = selected;
                        return tabsGroup;
                    },
                    () =>
                    {
                        tabsGroup.Tabs[selected].OnUnselect();
                        tabsGroup.Tabs[oldSelected].OnSelected();
                        tabsGroup.Selected = oldSelected;
                        return tabsGroup;
                    },
                    changeValueCallback);
            }

            return rect;
        }
    }
}
