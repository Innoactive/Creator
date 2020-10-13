using UnityEngine;

namespace Innoactive.Creator.Core.Validation
{
    public static class ReportEntryGenerator
    {
        public static ReportEntry VariableNotSet(string fieldName)
        {
            return new ReportEntry(4001, $"This variable {fieldName} is required", ValidationErrorLevel.ERROR);
        }

        public static ReportEntry NumericVariableNotSet(string fieldName)
        {
            return new ReportEntry(4001, $"This variable {fieldName} should not be zero.", ValidationErrorLevel.ERROR);
        }

        public static ReportEntry GrabAndSnapCollision()
        {
            return new ReportEntry(3001, $"A SnappedCondition and GrabbedCondition is used for the same object. The GrabbedCondition is not required.", ValidationErrorLevel.ERROR);
        }

        public static ReportEntry HighlightOnSnapZone()
        {
            return new ReportEntry(3002, $"A highlight is highlighting a SnapZone, which is automatically highlighted. The HighlightObjectBehavior is not required.", ValidationErrorLevel.ERROR);
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
