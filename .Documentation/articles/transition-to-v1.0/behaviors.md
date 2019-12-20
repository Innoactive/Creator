# Behaviors

This page provides a high-level step-by-step guide on how to rewrite old behaviors to be compatible with the Innoactive Creator v1.0. We kept this guide very practical; see the [General Concepts](general-concepts.md) page for the overview.

## Review the Old Behavior

Let's take a look at a behavior that uses an older version of the Innoactive Creator: it has serialized properties to define its target object, scale, and duration. When activation starts, it starts the coroutine that scales the object to a given scale over given number of seconds. When you fast-forward it, it stops the coroutine and immediately sets the scale to the required value.

```csharp
// This behavior linearly changes scale of a Target object over Duration seconds, until it matches TargetScale.
[DataContract(IsReference = true)]
[DisplayName("Scale Object")]
public class ScalingBehavior : Behavior
{
    // Training object to scale.
    [DataMember]
    public SceneObjectReference Target { get; private set; }

    // Target scale.
    [DataMember]
    [DisplayName("Target Scale")]
    public Vector3 TargetScale { get; private set; }

    // Duration of the animation in seconds.
    [DataMember]
    [DisplayName("Animation Duration")]
    public float Duration { get; private set; }

    // A coroutine responsible for scaling the target.
    private IEnumerator coroutine;
    
    // Handle data initialization in the constructor.
    [JsonConstructor]
    public ScalingBehavior() : this(new SceneObjectReference(), Vector3.one, 0f)
    {
    }

    public ScalingBehavior(SceneObjectReference target, Vector3 targetScale, float duration)
    {
        Target = target;
        TargetScale = targetScale;
        Duration = duration;
    }
    
    // Called on activation of the training entity. Define activation logic here.
    // You have to call `SignalActivationFinished()` after you've done everything you wanted to do during the activation.
    protected override void PerformActivation()
    {
        // Start coroutine which will scale our object.
        coroutine = ScaleTarget();
        CoroutineDispatcher.Instance.StartCoroutine(coroutine);
    }

    // Called on deactivation of the training entity. Define deactivation logic here.
    // You have to call `SignalDeactivationFinished()` after you've done everything you wanted to do during the deactivation.
    protected override void PerformDeactivation()
    {
        SignalDeactivationFinished();
    }

    // This method is called when the activation has to be interrupted and completed immediately.
    protected override void FastForwardActivating()
    {
        // Stop the scaling coroutine,
        CoroutineDispatcher.Instance.StopCoroutine(coroutine);

        // Scale the target manually,
        Target.Value.GameObject.transform.localScale = TargetScale;

        // And signal that activation is finished.
        SignalActivationFinished();
    }
    
    // It requires no additional action.
    protected override void FastForwardActive()
    {
    }

    // Deactivation is instanteneous.
    // It requires no additional action.
    protected override void FastForwardDeactivating()
    {
    }
    
    // Coroutine which scales the target transform over time and then finished the activation.
    private IEnumerator ScaleTarget()
    {
        float startedAt = Time.time;

        Transform scaledTransform = Target.Value.GameObject.transform;

        Vector3 initialScale = scaledTransform.localScale;

        while (Time.time - startedAt < Duration)
        {
            float progress = (Time.time - startedAt) / Duration;

            scaledTransform.localScale = Vector3.Lerp(initialScale, TargetScale, progress);
            yield return null;
        }

        scaledTransform.localScale = TargetScale;

        SignalActivationFinished();
    }
}
```

## Extract the Data

Let's start the conversion of this behavior by extracting its data into a separate class:

```csharp
// Internally, we don't draw entities: we only draw their data. That's why we have to move the attribute from the behavior's declaration here.
[DisplayName("Scale Object")]
// Declare the class to be serializeable (so we can save and load it).
[DataContract(IsReference = true)]
// When you implement a data class for behavior, you have to implement the IBehaviorData interface.
public class ScalingBehaviorData : IBehaviorData
{
    // Declare the properties as before, but the setters have to be public now.
    // Training object to scale.
    [DataMember]
    public SceneObjectReference Target { get; set; }

    // Target scale.
    [DataMember]
    [DisplayName("Target Scale")]
    public Vector3 TargetScale { get; set; }

    // Duration of the animation in seconds.
    [DataMember]
    [DisplayName("Animation Duration")]
    public float Duration { get; set; }

    // Any IData has to implement the Metadata property.
    // We use it internally in the Step Inspector.
    public Metadata Metadata { get; set; }

    // Any behavior has a name that can be set in the Step Inspector.
    public string Name { get; set; }

    // There is no need to declare any attributes on the inherited properties, because we did it in the interface declaration already.
}
```

## Extract the Process

Now we need to define the process of the scaling behavior.
Since the scaling behavior performs only during the `Activating` stage, we need to implement only one stage process:

```csharp
// We have to declare the type of data which this process can modify. We do it by implementing the generic IStageProcess<TData> interface.
public class ScalingBehaviorActivatingProcess : IStageProcess<ScalingBehaviorData>
{
    private float startedAt;
    private Transform scaledTransform;
    private Vector3 initialScale;

    // This method is a good place to initialize the process.
    public void Start(ScalingBehaviorData data)
    {
        startedAt = Time.time;
        scaledTransform = data.Target.Value.GameObject.transform;
        Vector3 initialScale = scaledTransform.localScale;
    }

    // The Update() method contains the code identical to the old coroutine. It is updated once per Unity frame and remembers  its state between frames.
    public IEnumerator Update(ScalingBehaviorData data)
    {
        while (Time.time - startedAt < data.Duration)
        {
            float progress = (Time.time - startedAt) / data.Duration;

            scaledTransform.localScale = Vector3.Lerp(initialScale, data.TargetScale, progress);
            // Wait for next frame.
            yield return null;
        }
    }

    // The Vector3.Lerp is imprecise, so we manually set the scale to the target value after the required amount of time has passed.
    public void End(ScalingBehaviorData data)
    {
        scaledTransform.localScale = data.TargetScale;
    }

    // To fast-forward the behavior, we should set the scale to the target value. But because the End() method is called immediately afterwards, we don't need to duplicate the code. 
    public void FastForward(ScalingBehaviorData data)
    {
    }
}
```

## Assemble Everything Together

Finally, let's declare the data and the process in the behavior itself:

```csharp
// We still have to declare the behavior as a data contract, because we need to serialize its data.
[DataContract(IsReference = true)]
// Inherit from the abstract Behavior<TData> class and define which type of data is used.
public class ScalingBehavior : Behavior<ScalingBehaviorData>
{
    [JsonConstructor]
    // Make sure that you initialize the data properly.
    // One way to do it is to always call the constructor overload that implements it.
    public ScalingBehavior() : this(new SceneObjectReference(), Vector3.one, 0f)
    {
    }

    // The Data property is already declared in the abstract class, just make sure to create an instance of it.
    public ScalingBehavior(SceneObjectReference target, Vector3 targetScale, float duration)
    {
        Data = new EntityData()
        {
            Target = target,
            TargetScale = targetScale,
            Duration = duration,
        };
    }

    // An entity should always return the same instance of a process.
    // You can use already existing Process class instead of implementing your own.
    // It takes a stage process for every stage except for Inactive.
    private readonly IProcess<ScalingBehaviorData> process = new Process<ScalingBehaviorData>(
        new ScalingBehaviorActivatingProcess(), 
        // A process can't be null. 
        //Use EmptyStageProcess<TData>
        new EmptyStageProcess<ScalingBehaviorData>(), 
        new EmptyStageProcess<ScalingBehaviorData>()
    );

    protected override IProcess<ScalingBehaviorData> Process
    {
        get
        {
            return process;
        }
    }
}
```

## Keep the Codebase Clean

Since entities can consist of up to 8 classes now, it can clog your namespaces easily. At Innoactive, we use two approaches to fight it: if an entity can be reused and extended then we create a separate namespace for it (for example, `Innoactive.Hub.Training.Behaviors.ScalingBehavior`). If an entity is highly specific and shouldn't be reused then we use nested classes:

```csharp
// This behavior linearly changes scale of a Target object over Duration seconds, until it matches TargetScale.
[DataContract(IsReference = true)]
public class ScalingBehavior : Behavior<ScalingBehavior.EntityData>
{
    [DisplayName("Scale Object")]
    [DataContract(IsReference = true)]
    public class EntityData : IBehaviorData
    {
        // Implementation omitted.
    }

    private class ActivatingProcess : IStageProcess<EntityData>
    {
        // Implementation omitted.
    }

    // Constructors omitted.

    private readonly IProcess<EntityData> process = new Process<EntityData>(
        new ActivatingProcess(), 
        new EmptyStageProcess<EntityData>(), 
        new EmptyStageProcess<EntityData>()
    );

    protected override IProcess<EntityData> Process 
    {
        get 
        {
            return process; 
        }
    }
}
```