using System;
using UnityEngine;

namespace Innoactive.Creator.Core.Tabs
{
    internal class DynamicTab : ITab
    {
        public GUIContent Label { get; }

        public object GetValue()
        {
            return getter();
        }

        public void SetValue(object value)
        {
            setter(value);
        }

        private readonly Func<object> getter;
        private readonly Action<object> setter;

        public DynamicTab(GUIContent label, Func<object> getter, Action<object> setter)
        {
            Label = label;
            this.getter = getter;
            this.setter = setter;
        }
    }
}
