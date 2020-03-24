using System;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI
{
    public static class StepInspectorMenu
    {
        public abstract class Option<T>
        {
        }

        public sealed class Separator<T> : Option<T>
        {
            private readonly string pathToSubmenu;

            public string PathToSubmenu
            {
                get
                {
                    return pathToSubmenu;
                }
            }

            public Separator(string pathToSubmenu = "")
            {
                this.pathToSubmenu = pathToSubmenu;
            }
        }

        public class DisabledItem<T> : Option<T>
        {
            private readonly GUIContent label;

            public GUIContent Label
            {
                get
                {
                    return label;
                }
            }

            public DisabledItem(GUIContent label)
            {
                this.label = label;
            }

            public DisabledItem(string label) : this(new GUIContent(label))
            {
            }
        }

        public class DynamicItem<T> : Item<T>
        {
            private readonly GUIContent displayName;
            private readonly Func<T> getNewItem;

            public override GUIContent DisplayedName
            {
                get
                {
                    return displayName;
                }
            }

            public override T GetNewItem()
            {
                return getNewItem.Invoke();
            }

            public DynamicItem(GUIContent label, Func<T> getNewItem)
            {
                displayName = label;
                this.getNewItem = getNewItem;
            }

            public DynamicItem(string label, Func<T> getNewItem) : this(new GUIContent(label), getNewItem)
            {
            }
        }

        public abstract class Item<T> : StepInspectorMenu.Option<T>
        {
            public abstract T GetNewItem();
            public abstract GUIContent DisplayedName { get; }
        }
    }
}
