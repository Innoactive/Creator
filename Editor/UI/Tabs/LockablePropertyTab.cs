﻿using Innoactive.Creator.Core;
using UnityEngine;

namespace Innoactive.CreatorEditor.Tabs
{
    internal class LockablePropertyTab : ITab
    {
        private readonly Step.EntityData data;

        private static LockableObjectsCollection collection;

        public GUIContent Label { get; private set; }

        public LockablePropertyTab(GUIContent label, Step.EntityData data)
        {
            Label = label;
            this.data = data;
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
