using UnityEngine;

namespace Innoactive.Creator.Core.Validation
{
    internal static class ReportEntryGenerator
    {
        public static ReportEntry VariableNotSet(string fieldName)
        {
            return new ReportEntry(4001, $"This variable {fieldName} is required", ValidationErrorLevel.ERROR);
        }

        public static ReportEntry NumericVariableNotSet(string fieldName)
        {
            return new ReportEntry(4001, $"This variable {fieldName} should not be zero.", ValidationErrorLevel.ERROR);
        }

        public static ReportEntry StepNotReachable(IStepData step)
        {
            return new ReportEntry(2001, $"Step {step.Name} is not reachable!", ValidationErrorLevel.ERROR);
        }

        public static ReportEntry MissingCollider(GameObject gameObject)
        {
            return new ReportEntry(5001, $"The Object {gameObject.name} has no collider.", ValidationErrorLevel.ERROR);
        }

        public static ReportEntry MissingComponent(GameObject gameObject, string components)
        {
            return new ReportEntry(5002, $"The Object {gameObject.name} is missing one Component of {components}", ValidationErrorLevel.ERROR);
        }
    }
}
