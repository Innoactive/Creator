# Conditions

By adding conditions to transitions, training designers control when each step should complete. Transitions determine which steps to activate next.

Every step has one or more transitions, and every transition has a number of conditions. Once all conditions of one of the transitions complete, the step will start deactivating. The chapter will then follow the finished transition and execute the target step of it.

Before a step activates conditions and starts checking if they are complete, it will wait until all of its behaviors become active. A step will wait for behaviors to activate even if one of its transitions has no conditions, but will trigger that transition immediately afterwards.

Once a condition is complete, it will stay complete to the end of the step.

You are free to implement any kind of checks in your custom conditions. For example, a condition could check if an object is inside some particular area, or if a trainee has pressed a button on a wall, or simply for the time passed.

## Making an "Object Is Upside Down" Condition

Consider a training course about postal package handling. A trainee has to move a box marked with a "This way up!" label. A training designer would make a step with two transitions. One would trigger when the box is in the target zone, and the other one would check for the alignment of the box. If the trainee flips the box while carrying it, the latter condition will complete first and the trainee will have to try again. If the trainee handles the box carefully, he will bring it to the target zone and finish the chapter.

There is a default condition that checks if an object is inside an area. We need to create a behavior that will detect if an object is misaligned.

### Data

Just like in the chapter about [behaviors](05-behaviors.md), create a new `UpsideDownConditionData.cs` file in the `Assets` folder and fill it with the following:

```csharp
using System.Runtime.Serialization;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Attributes;
using Innoactive.Creator.Core.Conditions;
using Innoactive.Creator.Core.SceneObjects;

[DataContract(IsReference = true)]
[DisplayName("Is Object Upside Down?")]
public class UpsideDownConditionData : IConditionData
{
    // A reference to the target object that we will check.
    [DataMember]
    public SceneObjectReference Target { get; set; }

    // We will check how far the target from being upside down in degrees.
    // If the difference is lower than threshold, we must complete the condition.
    [DataMember]
    public float Threshold { get; set; }

    public Metadata Metadata { get; set; }
    public string Name { get; set; }
    public bool IsCompleted { get; set; }

    public UpsideDownConditionData()
    {
        Target = new SceneObjectReference("");
        Threshold = 135f;
    }
}
```

There are only two significant differences from the behavior's data.

We implement the `IConditionData` interface instead of `IBehaviorData`. This interface has an additional property called `IsCompleted`. The condition's process has to set this property to `true`; then, the containing step will access it.

Also, we have introduced a constructor. You can initialize some of the properties here instead of doing it in the condition itself. This change is optional; you can continue initializing data in entities, if you prefer so.

### Stage Process

Steps check for conditions in the Active stage. You need to implement only the Active stage process. It will check if the target is upside down in the `Update()` method. Depending on the result, it will either set `IsCompleted` to true and stop iterating, or wait for the next frame.

We will explain fast-forwarding of conditions in detail in the [next subsection](#fast-forwarding-and-automatic-completion).

Create a new `UpsideDownConditionActiveProcess.cs` file in the `Assets` folder and copy the following:

```csharp
using System.Collections;
using Innoactive.Creator.Core;
using UnityEngine;

public class UpsideDownConditionActiveProcess : Process<UpsideDownConditionData>
{
    public override void Start()
    {
    }

    public override IEnumerator Update()
    {
        // Get the difference between vector pointing down,
        // And the vector that comes out of the "roof" of the target.
        // Then compare it with threshold from data.
        while (Vector3.Angle(Vector3.down, Data.Target.Value.GameObject.transform.up) > Data.Threshold)
        {
            //If the angle is more than threshold, wait for the next frame.
            yield return null;
        }

        // If the angle is less or equal to threshold, mark the condition as complete.
        data.IsCompleted = true;
    }

    public override void End()
    {
    }

    // Nothing to fast-forward.
    // We will explain it soon.
    public override void FastForward()
    {
    }

    // Declare the constructor. It calls the base method to bind the data object with the process.
    public UpsideDownConditionActiveProcess(UpsideDownConditionData data) : base(data)
    {
    }
}
```

### Fast-Forwarding and Automatic Completion

As we have mentioned, the Innoactive Creator's fast-forwarding system does not skip anything. Instead, it fakes the natural execution in a single frame. It is straightforward with behaviors: they always execute in the same way. With conditions, it is more complex.

Consider a step with two transitions. During an actual training, a trainee would either complete one set of conditions, or the other one. Since all conditions were active, they all have to deactivate, but only some of them were completed. If we want to fake a natural execution, we should do the same: simulate circumstances in which certain conditions would complete, but fast-forward and deactivate all conditions.

For example, if we have a step where a trainee has to put a ball either into a left or a right basket, we would fast-forward both conditions, but our code would move the ball only into one of the baskets.

The Innoactive Creator always calls the `FastForward()` method. In majority of cases, you will leave this method empty for conditions. This is normal. To complete a condition in an artificial way, you have to create an autocompleter. It has only one method in which you have to set the `IsCompleted` flag and simulate the circumstances of the completion. Its class must inherit from `Innoactive.Hub.Training.IAutocompleter<UpsideDownConditionData>`.

Create a new `UpsideDownConditionAutocompleter.cs` file within the `Assets` folder and copy the following:

```csharp
using UnityEngine;
using Innoactive.Creator.Core;

public class UpsideDownConditionAutocompleter : Autocompleter<UpsideDownConditionData>
{
    public void Complete()
    {
        // Turn the target upside down, as it would normally happen.
        Data.Target.Value.GameObject.transform.rotation = Quaternion.Euler(0, 0, 180f);
    }
}
```

### Assemble the Condition

The code of the condition is very similar to the code of the behavior.

```csharp
using System.Runtime.Serialization;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Conditions;

[DataContract(IsReference = true)]
public class UpsideDownCondition : Condition<UpsideDownConditionData>
{
    public override IProcess GetActiveProcess()
    {
        // Always return a new instance.
        return new UpsideDownConditionActiveProcess(Data);
    }

    protected override IAutocompleter GetAutocompleter()
    {
        // Always return a new instance.
        return new UpsideDownConditionAutocompleter(Data);
    }

    public UpsideDownCondition()
    {
        Data.Name = "Upside Down";
    }
}
```

### Menu Item

See the chapter about [menu items](06-menu-items.md) to display this condition in the Step Inspector.

Test the condition in the same way as you have tested the [behavior](05-behaviors.md).

[To the next chapter!](09-properties.md)