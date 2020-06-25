using UnityEngine;

namespace Innoactive.Creator.Core.Tabs
{
    internal interface ITab
    {
        GUIContent Label { get; }
        object GetValue();
        void SetValue(object value);
    }
}
