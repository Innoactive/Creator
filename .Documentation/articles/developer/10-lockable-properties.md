# Lockable Properties



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

## Disabling The Locking Feature

To disable the locking feature, set the `StepLockHandling` property of your runtime configuration to an instance of the `Innoactive.Creator.Core.RestrictiveEnvironment.NonLockingStepHandling` class.

[To the next chapter!](11-text-to-speech.md)