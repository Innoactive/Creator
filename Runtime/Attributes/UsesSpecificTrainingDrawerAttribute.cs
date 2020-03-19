using System;

namespace Innoactive.Creator.Core.Attributes
{
    /// <summary>
    /// Use this attribute to explicitly specify an implementation of `ITrainingDrawer` that should be used.
    /// The drawer type is passed as string because you can't reference editor definitions in runtime classes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class UsesSpecificTrainingDrawerAttribute : Attribute
    {
        public string DrawerType { get; private set; }

        public UsesSpecificTrainingDrawerAttribute(string drawerType)
        {
            DrawerType = drawerType;
        }
    }
}
