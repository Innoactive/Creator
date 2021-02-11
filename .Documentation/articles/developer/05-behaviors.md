# Behaviors

This chapter explains how to create custom behaviors.

A behavior is something that a training application has to do for a trainee. For example, if a trainee has to interact with an object, a behavior could move that object closer, highlight it, or play an audio hint with instructions.

In the Innoactive Creator, all entities form a tree-like structure. For instance, steps are children of chapters, behaviors and transitions are children of steps, and conditions belong to transitions. Parent entities have full control over their children, notably their life cycles.

When a step starts activating, it activates its behaviors. After all behaviors are active, it activates transitions. Transitions, in turn, activate their conditions. The step checks until all conditions of any transitions complete. Then it starts deactivating, and deactivates the behaviors.

It means that if you want to do something at the beginning of a step, you have to implement its Activating stage process. If you want to do something at the end of the step after conditions are met, implement the Deactivating stage process.

Use behaviors to prepare the environment to conditions, or to clean it up afterwards. We do not support behaviors with Active stage processes. We advise against implementing them, as we have never tested that case.

## Making a "Scale Object" Behavior

If you just spawn a new object in your virtual reality application, it may disorient your user. Instead, you could scale an object up: a brief animation is enough for a user to adjust. We could create a behavior that scales a given object to a given value. A training designer would be able to set the animation's duration.

We need to define the behavior's data. It would contain a target scale, an animation's duration, and a reference to a target object. Assuming that we want to scale an object at the beginning of a step, we need to implement the stage process for the Activating stage, which would change the scale of the object every frame. When it has to fast-forward, then it must set the scale to the target value immediately.

### Data

We need a C# class file which will contain the data class.

The Unity Editor only recognizes files inside the project's `Assets` folder. Create it there. Name it `ScalingBehaviorData` or any other way you prefer, and make sure to change the file's extension to `.cs`.

Open this file in your favorite IDE or text editor. Make sure that the file is empty. Insert the following:

```csharp
using UnityEngine;
using System.Runtime.Serialization;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Behaviors;
using Innoactive.Creator.Core.Attributes;
using Innoactive.Creator.Core.SceneObjects;
```

This way we declare which [namespaces](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/namespaces/) we use in this file. Leave this list untouched throughout this subsection.

Now, we have to declare it, as well as the properties that we will need. A data class of any behavior has to implement the `IBehaviorData` interface.

```csharp
// When you implement a data class for a behavior, you have to implement the IBehaviorData interface.
public class ScalingBehaviorData : IBehaviorData
{
    // The object to scale.
    public SceneObjectReference Target { get; set; }

    // Target scale.
    public Vector3 TargetScale { get; set; }

    // Duration of the animation in seconds.
    public float Duration { get; set; }

    // Interface members:

    // Any IData has to implement the Metadata property.
    // We use it internally in the Step Inspector.
    public Metadata Metadata { get; set; }

    // Any behavior has a name that can be set in the Step Inspector.
    public string Name { get; set; }
}
```

Any data object has to be serializable. It means that it could be converted to a plain text and back. We use serialization to save and load training courses, and to draw them in the Innoactive Creator editors. 

This is why the `Target` property is a `SceneObjectReference` and not `ISceneObject`: we cannot serialize scene objects, but we can serialize references to them. We will get back to scene object references in the chapter about [properties](09-properties.md).

To mark classes and properties as serializable, we use `[DataContract]` and `[DataMember]` attributes from the `System.Runtime.Serialization` namespace. 

```csharp
// Declare the class to be serializable (so we could save and load it).
// The parameter is mandatory.
[DataContract(IsReference = true)]
public class ScalingBehaviorData : IBehaviorData
{
    // Declare the property to be serializable.
    [DataMember]
    public SceneObjectReference Target { get; set; }

    [DataMember]
    public Vector3 TargetScale { get; set; }

    [DataMember]
    public float Duration { get; set; }

    // No need to declare any attributes on the inherited properties, because we did it in the base interface already.

    public Metadata Metadata { get; set; }

    public string Name { get; set; }

}
```

We could use the data class already, but the Step Inspector would label the `TargetScale` property without spacing between the two words. To fix it, use the `[DisplayName]` attribute from the `Innoactive.Hub.Training.Attributes` namespace:

```csharp
// The step inspector draws data objects, not entities. 
// This is why we have to attribute the data class.
[DisplayName("Scale Object")]
[DataContract(IsReference = true)]
public class ScalingBehaviorData : IBehaviorData
{
    [DataMember]
    public SceneObjectReference Target { get; set; }

    [DataMember]
    [DisplayName("Target Scale")]
    public Vector3 TargetScale { get; set; }

    [DataMember]
    [DisplayName("Animation Duration")]
    public float Duration { get; set; }

    public Metadata Metadata { get; set; }

    public string Name { get; set; }
}
```

Each behavior or condition has a help button in their header. The button is linked to a webpage.
If you would like to add your own link, add a `[HelpLink]` attribute to the behavior class.

[![Help Button](../images/developer/help-attribute.png)](../images/developer/help-attribute.png "The HelpLink attribute allows to insert guidance for your training designers.")


```csharp
// The step inspector draws data objects, not entities. 
// This is why we have to attribute the data class.
[DisplayName("Scale Object")]
// Optionally, add an HTML-link to an own help page.
[HelpLink("https://spectrum.chat/innoactive-creator")]
[DataContract(IsReference = true)]
public class ScalingBehaviorData : IBehaviorData
{
    [DataMember]
    public SceneObjectReference Target { get; set; }

    [DataMember]
    [DisplayName("Target Scale")]
    public Vector3 TargetScale { get; set; }

    [DataMember]
    [DisplayName("Animation Duration")]
    public float Duration { get; set; }

    public Metadata Metadata { get; set; }

    public string Name { get; set; }
}
```


## Stage Process

Now we need to define the process of the scaling behavior. It will read and modify the data as it will take the target and apply a new scale to it over a given duration in seconds.

Assuming that we want to scale an object at the beginning of a step, we need to implement the stage process of the Activating stage. If we would want to scale an object at the end, we would implement a Deactivating stage process. If you want training designer to choose, check the source code of the `PlayAudioBehavior.cs`.

`IStageProcess` is a [generic](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/) interface. To make a stage process compatible with a data class, you have to pass the data class as a generic parameter of the interface when implementing it.

As in the [previous](#data) section, prepare a new C# class file. Name it `ScalingBehaviorActivatingProcess.cs`. Make sure it is empty, and copy the following:

```csharp
// Declare namespaces to use, as in the previous subsection.
 using Innoactive.Creator.Core;
 using System.Collections;
 using UnityEngine;
 
 // We have to declare the type of data which this process can modify.
 // We do it by implementing the generic IStageProcess<TData> interface.
public class ScalingBehaviorActivatingProcess : Process<ScalingBehaviorData>
{
    // Always runs when we enter this stage.
    public override void Start()
    {
    }
 
    // Starting from the next frame, 
    // the Innoactive Creator will call it every frame until it will complete it.
    public override IEnumerator Update()
    {
    }
 
    // Always runs whenever we have finished the Update or executed the FastForward.
    public override void End()
    {
    }
 
    // We call it when we had no time to complete Update,
    // so we have to fake it.
    public override void FastForward()
    {
    }
}
```

We just declared all four methods that belong to the `IStageProcess` interface. Now we have to write the code that will scale the target scene object.

At the start of the stage process, we will record the current time and initial scale. The target, as any other Unity scene object, has a transform that stores its position, rotation, and scale. We will retrieve the reference to it, too. 

In `Update()`, we will [linearly interpolate](https://en.wikipedia.org/wiki/Linear_interpolation) between the initial and target scale depending on the time passed: every frame, the object will shrink or grow towards the target scale. Afterwards, we will set the scale to the precise value in the `End()` method. If the stage process has to fast-forward, the `Update()` method will not iterate completely. We will handle it in the in the `FastForward()` method by assigning the target scale to the object.

```csharp
public class ScalingBehaviorActivatingProcess : Process<ScalingBehaviorData>
{
    // Private fields which all methods in the class can access.
    private float startedAt;
    private Transform scaledTransform;
    private Vector3 initialScale;

    // Record the initial values.
    // The same instance of a stage process could be used multiple times,
    // so you have to make sure that you reset everything in this method.
    public override void Start()
    {
        // Data.Target is a reference to a scene object.
        // Data.Target.Value is the actual scene object.
        scaledTransform = Data.Target.Value.GameObject.transform;
        startedAt = Time.time;
        initialScale = scaledTransform.localScale;
    }

    public override IEnumerator Update()
    {
        // Time.time returns the time elapsed 
        // since the start of the application.
        while (Time.time - startedAt < Data.Duration)
        {
            // Calculate the progress from 0 to 1.
            float progress = (Time.time - startedAt) / Data.Duration;

            // Linearly interpolate between the initial and target scale,
            // based on the progress.
            scaledTransform.localScale = Vector3.Lerp(initialScale, Data.TargetScale, progress);

            // Wait for the next frame.
            yield return null;
            // This method will continue to execute from this line
            // the next time we invoke it.
        }
    }

    // Vector3.Lerp is imprecise, so we manually set the scale to the target value after the required amount of time has passed.
    public override void End()
    {
        scaledTransform.localScale = Data.TargetScale;
    }

    // To fast-forward the behavior, we should set the scale to the target value. But since the End() method is called immediately afterwards, we don't need to duplicate the code. 
    public override void FastForward()
    {
        // scaledTransform.localScale = data.TargetScale;
    }

    // Declare the constructor. It calls the base method to bind the data object with the process.
    public ScalingBehaviorActivatingProcess(ScalingBehaviorData data) : base(data)
    {
    }
}
```

## Assemble the Behavior

Prepare a new C# class file for the behavior class with  the following:

```csharp
using UnityEngine;
using System.Runtime.Serialization;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Behaviors;
using Innoactive.Creator.Core.Attributes;
using Innoactive.Creator.Core.SceneObjects;
```

To create a behavior, we have to inherit from the `Behavior` abstract class and define the data and the process.

The base class has the basic `Data` property already. Create an instance of your data class in the behavior's constructor and assign default values to its fields.

To define a process, you have to override a method that corresponds to the target stage: `GetActivatingProcess()`, `GetActiveProcess()`, or `GetDeactivatingProcess()`. An entity could contain multiple processes, up to one per stage. By default, these methods return `EmptyStageProcess` processes, which do nothing. An entity always expects a process instance from these calls: they should never return `null`.

```csharp
// We have to declare the behavior as a data contract, too,
// So the serializer would reach the data.
[DataContract(IsReference = true)]
// Inherit from the abstract Behavior<TData> class and define which type of data it uses.
public class ScalingBehavior : Behavior<ScalingBehaviorData>
{
    // Any serializable class must include a public parameterless constructor.
    // (Classes with no declared constructors have one by default).
    // Setup the Data property here.
    public ScalingBehavior()
    {
        // The default base constructor has created the Data object already.
        // Now we need to setup its values.
        Data.Duration = 0f;
        Data.TargetScale = Vector3.one;
        // Make sure to always initialize scene object references.
        Data.Target = new SceneObjectReference("");
    }

    // Each entity has three virtual methods where you can declare the stage process
    // that that entity should use.
    // By default, these methods return empty processes that do nothing.
    public override IProcess GetActivatingProcess()
    {
        // Always return a new instance of a stage process.
        return new ScalingBehaviorActivatingProcess(Data);
    }
}
```

This behavior is not displayed in the Step Inspector yet. We will explain how to do it in the next chapter.

## Keep the Code Clean

Since you need at least three classes per behavior, multiple behaviors in the same namespace could make it hard to navigate.

You can solve it in two ways. First way would be to use a single namespace per behavior:

```csharp
namespace My.Behaviors.Scaling
{
    [DisplayName("Scale Object")]
    [DataContract(IsReference = true)]
    public class Data : IBehaviorData
    {
        // Implementation omitted.
    }
    
    public class ActivatingProcess : Process<Data>
    {
        // Implementation omitted.
    }
    
    [DataContract(IsReference = true)]
    public class Behavior : Behavior<Data>
    {
        // Implementation omitted.
    }
}
```

The other way is to declare data and processes as private classes:

```csharp
[DataContract(IsReference = true)]
public class ScalingBehavior : Behavior<ScalingBehavior.EntityData>
{
    [DisplayName("Scale Object")]
    [DataContract(IsReference = true)]
    public class EntityData : IBehaviorData
    {
        // Implementation omitted.
    }

    private class ActivatingProcess : Process<EntityData>
    {
        // Implementation omitted.
    }

    // Implementation omitted.
}
```

[To the next chapter!](06-menu-items.md)
