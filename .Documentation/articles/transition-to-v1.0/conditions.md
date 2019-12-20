# Conditions

This page provides a high-level step-by-step guide on how to rewrite old conditions to be compatible with the Innoactive Creator v1.0. We kept this guide very practical; see the [General Concepts](general-concepts.md) page for the overview.

## Review the Old Condition

Below is the listing for a condition that uses an older version of the Tranining Module. It subscribes to its pointer property's event at activation and unsubscribes on deactivation. When the pointing property hits the target object, the condition completes and messages its transition about it (via `MarkAsCompleted()` method).

```csharp
[DataContract(IsReference = true)]
[DisplayName("Point at Object")]
// Condition which is completed when Pointer points at Target.
public class PointedCondition : Condition
{
    [DataMember]
    // Reference to a pointer property.
    public ScenePropertyReference<PointingProperty> Pointer { get; private set; }

    [DisplayName("Target with a collider")]
    [DataMember]
    // Reference to a target property.
    public ScenePropertyReference<ColliderWithTriggerProperty> Target { get; private set; }

    [JsonConstructor]
    // Make sure that references are initialized.
    public PointedCondition() : this(new ScenePropertyReference<PointingProperty>(), new ScenePropertyReference<ColliderWithTriggerProperty>())
    {
    }

    public PointedCondition(ScenePropertyReference<PointingProperty> pointer, ScenePropertyReference<ColliderWithTriggerProperty> target)
    {
        Pointer = pointer;
        Target = target;
    }

    // This method is called when the step with that condition has completed activation of its behaviors.
    protected override void PerformActivation()
    {
        Pointer.Value.PointerEnter += OnPointerEnter;
        SignalActivationFinished();
    }

    // This method is called at deactivation of the step, after every behavior has completed its deactivation.
    protected override void PerformDeactivation()
    {
        Pointer.Value.PointerEnter -= OnPointerEnter;
        SignalDeactivationFinished();
    }

    // When a condition or behavior is fast-forwarded, the activation has to complete immediately.
    // This method should handle it, but since the activation is instanteneous,
    // It doesn't require any additional actions.
    protected override void FastForwardActivating()
    {
    }

    // When fast-forwarded, a conditions should complete immediately.
    // For that, the pointer fakes that it pointed at the target.
    protected override void FastForwardActive()
    {
        Pointer.Value.FastForwardPoint(Target);
    }

    // When a condition or behavior is fast-forwarded, the deactivation has to complete immediately.
    // This method should handle it, but since the deactivation is instanteneous,
    // It doesn't require any additional actions.
    protected override void FastForwardDeactivating()
    {
    }

    // When PointerProperty points at something,
    private void OnPointerEnter(ColliderWithTriggerProperty pointed)
    {
        // Ignore it if this condition is already fulfilled.
        if (IsCompleted)
        {
            return;
        }

        // Else, if Target references the pointed object, complete the condition.
        if (Target.Value == pointed)
        {
            MarkAsCompleted();
        }
    }
}
```

## Extract the Data

As with behaviors, we need to extract the state of the condition to a separate class. It has to implement `Metadata`, `Name`, and `IsCompleted` properties of the `IConditionData` interface in addition to its specific properties.

```csharp
// Mark the class as serializeable so we can save and load it.
[DataContract(IsReference = true)]
// This attribute changes the title of the condition in the Step Inspector.
[DisplayName("Point at Object")]
public class PointedConditionData : IConditionData
{
    // The [DataMember] attribute marks a property as serializeable.
    [DataMember]
    public ScenePropertyReference<PointingProperty> Pointer { get; set; }

    [DisplayName("Target with a collider")]
    [DataMember]
    public ScenePropertyReference<ColliderWithTriggerProperty> Target { get; set; }

    // Reset it at the beginning of the Active stage.
    // Set this property to true once the condition has completed.
    public bool IsCompleted { get; set; }

    // Every condition has a name that can be set in the Step Inspector.
    public string Name { get; set; }

    // Every IData has to implement the Metadata property.
    // We use it internally in the Step Inspector.
    public Metadata Metadata { get; set; }

    // You don't need to declare any attributes on the inherited properties because we do it in the interface declaration already.
}
```

## Extract the Process

In previous versions of the training module, a condition was messaging the transition about its completion with the `MarkAsCompleted()` method. Now, a condition sets the `IsCompleted` flag, and then the transition checks for it. A transitions does that only when it is `Active`, and it can be active only when all of its conditions are `Active`. As the most of conditions had instanteneous `Activation` and `Deactivation` activation states, this logic can be moved to the `Start()` and `End()` methods of the active process.

```csharp
public class PointedConditionActiveProcess : IStageProcess<PointedConditionData>
{    
    // This method is a good place to initialize the process.
    public void Start(PointedConditionData data)
    {
        // Reset the flag so it would work properly next time you activate it.
        // The condition can be activated multiple times in training chapter workflows with loops.
        // This can also happen if the trainer switches between the training modes.
        data.IsCompleted = false;

        // Subscribe to the event as part of the initialization.
        data.Pointer.Value.PointerEnter += OnPointerEnter;
    }

    // The update has to run until the condition is completed,
    // otherwise the End() method will be called prematurely.
    public IEnumerator Update(PointedConditionData data)
    {
        // As we set this flag in the event handler, we simply check for the flag here.
        // Alternatively, we could check something every frame in this method.
        while (data.IsCompleted == false)
        {
            yield return null;
        }
    }

    // Use the `End()` method as a deinitializer.
    public void End(PointedConditionData data)
    {
        // Here, we unsubscribe from the event to which we subscribed at Start().
        data.Pointer.Value.PointerEnter -= OnPointerEnter;
    }

    // Fast-forwarded conditions should not complete themselves anymore.
    // See next chapter for details.
    public void FastForward(PointedConditionData data)
    {
    }

    // The pointing event handler method.
    private void OnPointerEnter(ColliderWithTriggerProperty pointed)
    {
        // If the pointer points at the target, complete the condition.
        if (data.Target.Value == pointed)
        {
            data.IsCompleted = true;
        }
    }
}
```

## Extract the Autocompleter

A transition is complete only when all its conditions are complete. Training designers were able to create only linear workflows: every step
 had only one transition. It means that all activated conditions had to be completed before you could proceed to the next step: completion of
 a condition was an integral part of its execution. As the fast-forwarding has to simulate the "real", manual execution of the training, we 
concluded that you have to automatically complete a condition when you fast-forward it. This conclusion is no longer true when a step has
two transitions: when you complete all conditions of one transitions then the step deactivates, and conditions of the other transitions do 
not become complete.

That's why we have separated the automatic completion from the fast-forwarding. When the `FastForward()` method is invoked, it
shouldn't complete the condition; instead, the `Autocompleter` property has to handle it. The `IAutocompleter.Complete()` method is called 
just before the `FastForward()` method, if necessary.

```csharp
public class PointedConditionAutocompleter : IAutocompleter<PointedConditionData>
{
    // As for a process and configurator, 
    // an entity passes the data to its autocompleter as a method parameter.
    public void Complete(PointedConditionData data)
    {
        // The pointer property rotates the object to point at target.
        data.Pointer.Value.FastForwardPoint(data.Target);            
        // Set the flag.
        data.IsCompleted = true;
    }
}
```

## Assemble Everything Together

Now we only need to declare the data, process, and autocompleter in the new condition and initialize it properly:

```csharp
// We need to mark the condition as serializable to serialize its data.
[DataContract(IsReference = true)]
// Inherit from the Condition abstract class and declare the target data type.
public class PointedCondition : Condition<PointedConditionData>
{
    [JsonConstructor]
    // Make sure that you initialize the data properly.
    // One way to do it is to always call the constructor overload that implements it.
    public PointedCondition() : this(new ScenePropertyReference<PointingProperty>(), new ScenePropertyReference<ColliderWithTriggerProperty>())
    {
    }

    // The Data property is already declared in the abstract class, just make sure to create an instance of it.
    public PointedCondition(ScenePropertyReference<PointingProperty> pointer, ScenePropertyReference<ColliderWithTriggerProperty> target)
    {
        Data = new EntityData()
        {
            Pointer = pointer,
            Target = target
        };
    }

    // An entity should always return the same instance of a process.
    // You can use ActiveOnlyProcess class if there is only the `Active` stage to handle.
    private readonly IProcess<PointedConditionData> process = new ActiveOnlyProcess<PointedConditionData>(new ActiveProcess());

    protected override IProcess<PointedConditionData> Process
    {
        get
        {
            return process;
        }
    }

    // The ICompletableEntity interface declares the Autocompleter property.
    // Any condition is ICompletableEntity.
    private readonly IAutocompleter<PointedConditionData> autocompleter = new PointedConditionAutocompleter();

    protected override IAutocompleter<PointedConditionData> Autocompleter
    {
        get
        {
            return autocompleter;
        }
    }
}
```