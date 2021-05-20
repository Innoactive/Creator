using System.Runtime.Serialization;
using VPG.Core;
using UnityEngine;

namespace VPG.Editor.Tabs
{
    internal class LockablePropertyTab : ITab
    {
        private readonly Step.EntityData data;

        private LockableObjectsCollection collection;

        public GUIContent Label { get; private set; }

        public LockablePropertyTab(GUIContent label, Step.EntityData data)
        {
            Label = label;
            this.data = data;
            collection = new LockableObjectsCollection(data);
        }

        public object GetValue()
        {
            return collection;
        }

        public void SetValue(object value)
        {
        }

        public void OnSelected()
        {
            collection = new LockableObjectsCollection(data);
        }

        public void OnUnselect()
        {

        }
    }
}
