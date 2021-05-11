# Lockable Properties

Some properties of the objects are lockable. This means that training designers can temporary disable user interactions for these properties to let trainees focus on the current task. You can read more [here](../innoactive-creator/suspending-interactions.md).

## Implementing Lockable Properties

To make your property lockable, inherit from the abstract class called `Innoactive.Creator.Core.Properties.LockableProperty`:

```csharp
public class LightSwitchProperty : LockableProperty
{
    // Implementation...
}
```

You will have to implement the abstract method called `InternalSetLocked(bool)`. If the input parameter is set to true, it should disable user interactions with this property.

```csharp
public class LightSwitchProperty : LockableProperty
{
    private void EnableSwitch()
    {
        // Implementation...
    }

    private void DisableSwitch()
    {
        // Implementation...
    }

    protected override void InternalSetLocked(bool lockedState)
    {
        // If the input is true, we should prevent user interactions.
        if (lockedState)
        {
            DisableSwitch();
        }
        else
        {
            EnableSwitch();
        }
    }
}
```

## Step Lock Handling
The logic of locking and unlocking LockableProperties is provided by the abstract class `StepLockHandlingStrategy`, which cares about locking LockableProperties per step. It also allows to be configured by the active mode. The default strategy is named `DefaultStepLockHandling`. To deactivate the locking feature, set the `StepLockHandling` property of your runtime configuration to an instance of the `NonLockingStepHandling` class.

### DefaultStepLockHandling
Locks & unlocks LockableProperties according to the usage in the step. It also looks up which of the properties are used in the following step and keeps them unlocked. Additionally it allows to be configured by the current active Mode, following parameters are allowed:

Name | type | default value | comment
|---|---|---|---|
LockOnCourseStart | `boolean` | `true` | *Decides if all LockableProperties should get locked when the course is started*
LockOnCourseFinished | `boolean` | `false` | *Decides if all LockableProperties should get locked when the course is finished*

## Extending Conditions
All Conditions based on the abstract class `Condition` have an already existing implementation of the method provided by the interface `ILockablePropertiesProvider`, which extracts all `LockableProperty` from the Condition's data. If you want to manually intervene and change the value or the specific properties used, you are able to override the method.

An example in `GrabbedCondition` which keeps the properties unlocked after finishing the step:
```csharp
public override IEnumerable<LockablePropertyData> GetLockableProperties()
{
    IEnumerable<LockablePropertyData> references = base.GetLockableProperties();
    foreach (LockablePropertyData propertyData in references)
    {
        propertyData.EndStepLocked = !Data.KeepUnlocked;
    }

    return references;
}
```

[To the next chapter!](input-system.md)