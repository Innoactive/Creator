# Properties

Behaviors and conditions use references to interact with scene objects. Scene objects may have properties. Properties are [Unity components](https://docs.unity3d.com/Manual/Components.html) that provide training course entities with additional ways to interact with scene objects.

For example, a `TouchableProperty` emits an event when a trainee touches the object. This way, a `TouchedCondition` will detect when it must complete.

A `HighlightProperty` exposes the `Highlight()` and `Unhighlight()` methods. A `HighlightBehavior` will invoke these methods during the Activating and Deactivating stages.

The Innoactive Creator's properties have nothing in common with [C# properties](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/properties). We use both concepts, and we refer to both of them in our documentation.

## Scalable Property

With the current scaling behavior, training designers have to always keep in mind to which scale they should resize each object.

It would be easier to define the target scale once per a scene object, save it as a [prefab](https://docs.unity3d.com/Manual/Prefabs.html), and reuse it later.

For this, we would need to create a new property. It would provide a method that behaviors could invoke. Also, this property should allow designers to modify the target scale through the scene object's inspector.

### Create the Property

Create a new `.cs` file in the `Assets` folder with the following content:

```csharp
using Innoactive.Hub.Training.SceneObjects.Properties;
using UnityEngine;

public class ScalableProperty : TrainingSceneObjectProperty
{
    private Vector3 initialScale;
    
    // See Unity documentation.
    [SerializeField]
    private Vector3 targetScale;

    // See Unity documentation.
    private void Awake()
    {
        // Remember the initial scale when the application
        // has loaded the scene.
        initialScale = transform.localScale;
    }

    // A public method that we could call with our behavior.
    public void Scale(float progress)
    {
        // Linearly interpolate from initial scale to target scale 
        // depending on the progress.
        transform.localScale = Vector3.Lerp(initialScale, targetScale, progress);
    }
}
```

### Update the Data

Remove the `TargetScale` property with its attributes from the scaling behavior's data class. 

In the same class, replace

```csharp
    public SceneObjectReference Target { get; set; }
```

with

```csharp
    public ScenePropertyReference<ScalableProperty> Target { get; set; }
```

Note that this will break old training courses that use the scaling behavior.

### Update the Stage Process

Replace the behavior's Activating stage process with the following:

```csharp
public class ScalingBehaviorActivatingProcess : IStageProcess<ScalingBehaviorData>
{
    private float startedAt;

    public void Start(ScalingBehaviorData data)
    {
        startedAt = Time.time;
    }

    public IEnumerator Update(ScalingBehaviorData data)
    {
        while (Time.time - startedAt < data.Duration)
        {
            float progress = (Time.time - startedAt) / data.Duration;

            data.Target.Value.Scale(progress);
            
            yield return null;
        }
    }

    public void End(ScalingBehaviorData data)
    {
            data.Target.Value.Scale(1f);
    }

    public void FastForward(ScalingBehaviorData data)
    {
    }
}
```

### Update the Behavior

In the behavior's class itself, replace the behavior's constructor:

```csharp
public ScalingBehavior()
{
    Data = new ScalingBehaviorData()
    {
        Target = new ScenePropertyReference<ScalableProperty>(""),
        Duration = 0f,
    };
}
```

### Try It Out

Create a new training course and add an object with the `ScalableProperty`. Note that now you have to specify the target scale in the scene object's inspector and not in a training course.

## When to Use Properties

There are no strict rules whenever a code should belong to an entity or to a property. You could use this as a rule of thumb: behaviors and conditions should know *what* to do, and properties should know *how* to do it.

From the examples above, a `HighlightBehavior` knows when to highlight an object, but only a `HighlightProperty` knows if it is done with particles, or with a color change, or by showing a big glowing arrow.

If you have to set up the scene object in runtime, set up it in the `Awake()`, `Start()`, or `OnEnable()` method of your property class.

If your custom entity works only with specific objects, you could use properties to prevent training designers from making mistakes.

This concludes the core part of the template developer's documentation. Advanced topics are to be done.