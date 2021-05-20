using System;

namespace VPG.Editor.UI.StepInspector.Menu
{
    /// <summary>
    /// This is a helper for generic typed class to be able to get the internal items type.
    /// </summary>
    internal interface IInternalTypeProvider
    {
        Type GetItemType();
    }
}
