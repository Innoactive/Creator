using System;

namespace Innoactive.Hub.Training.Editors.Drawers
{
    [AttributeUsage(AttributeTargets.Class)]
    public class InstantiatorTrainingDrawerAttribute : Attribute
    {
        public Type Type { get; private set; }

        public InstantiatorTrainingDrawerAttribute(Type type)
        {
            Type = type;
        }
    }
}
