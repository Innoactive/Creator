# Examples Description

The [Innoactive Creator package](http://developers.innoactive.de/creator/releases/) contains the Innoactive Creator core, components, the basic template, and examples. You can find more information on how to import this package in this [tutorial](../getting-started/evaluator.md).

The `Assets/Examples/Scenes` folder contains example scenes that showcase different features of the Innoactive Creator. Every scene contains a course that you could use as a guideline for creating your own courses.

The `Assets/Examples/Scenes/Simple/SimpleExample` scene is identical to the course you would create by following the [designer's tutorial](../getting-started/designer.md). Read through it before you continue reading this article.

## Localization Example

See the `LocalizationExample` scene inside the `Assets/Examples/Scenes/Advanced/` folder.

This example extends the training course from the tutorial by adding audio behaviors to it. These behaviors will generate audio instructions for trainee using a text-to-speech engine. This example provides voice lines for English and German. You can choose a language though UI of the running training application.

The `Assets/StreamingAssets/Training/LocalizationExample` folder contains the course file, and a `Localization` subfolder. Each language has its own localization file, which you can view and edit though a plain text editor of your choice.

## Branching Example

See the `BranchingExample` scene inside the `Assets/Examples/Scenes/Advanced/` folder.

The course of this example splits in two branches: a trainee could place the cube first, and then the sphere, or the other way around. With branched workflows, you can teach your trainees processes that have no strict order, or have different ways to complete the task.

The sample training course requires the trainee to snap the sphere and the cube under specified locations. The trainee can decide which object to snap first. The course completes when the trainee places both objects where they belong to.

### Steps

#### Snap Objects

This step has two outgoing transitions with different conditions: one triggers when the trainee snaps the sphere to the target snapzone, the other when he snaps the cube. Depending on which object the trainee snaps first, the course will proceed to a different step.

#### Snap Sphere/Snap Cube

If the trainee snaps the cube first, the application will proceed to the `Snap Sphere` step. Otherwise, it will proceed to the `Snap Cube` step. These steps are almost identical: they differ only in their target objects and snap zones.

#### End

Both of the previous steps converge into a single step. This step simply let the trainee know that the course is complete.

### Training Scene Objects

The training course uses four training scene objects: interactable cube and sphere, and two snap zones.

The cube, as wall as the sphere, has a `Snappable` property attached to its game object. This property lets trainees to touch, grab, and snap its object to a specified snap zone.

Both snap zones have the `Snap Zone` property. This property allows other objects to attach to its owner object. You can customize the snap zone by editing the following fields:

1. To display a highlight (also known as "preview", "ghost", or "shadow"), create a prefab and assign it to the `Shown Highlight Object` field. Use prefabs without any scripts and avoid using objects from the scene: the snap zone creates a copy of the object, and a rogue copy of a script can resolve in unexpected results. By default, no object is set, and the highlight is hidden.
1. By default, the snap zone displays the exact copy of the object, which may confuse your trainee. You can change the highlight's material to make it look different. Assign the new material to the `Interactable Hover Mesh Material` field.
1. To show the highlight whenever the trainee brings an object to the snap zone, set the `Show Interactable Hover Mesh` checkbox to true.

#### Restrict Interaction Between Objects

By default, you could snap any object to any snap zone. You can change that with the `Interaction Layer Mask` field. The snap zone then will only accept objects that have at least one [layer](https://docs.unity3d.com/Manual/Layers.html) that you have specified. To set the layer for the snappable object, select it and change its `InteractionLayerMask` field's value. In our example, the cube has every layer except for the `Sphere` layer, and the sphere has every layer except for the `Cube` layer.

Make sure that your objects still have a layer on which they can interact with the VR controller. Otherwise, your trainee would not be able to touch or grab it. To learn more, check the Unity Technologies's [documentation](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@0.9/manual/index.html#interactionlayermask).

## Looping Example

See the `LoopingExample` scene inside the `Assets/Examples/Scenes/Advanced/` folder.

The training course for this example loops between two steps until a specific condition is met. The trainee has to grab the sphere and bring it next to the cube. If he drops it midway, the application will ask him to grab the sphere again.

### Steps

#### Intro 

This step plays the introduction for the trainee. Once the application finishes the voice line, it will proceed to the next step.

#### Grab Sphere

In this step, the trainee has to grab the sphere.

#### Release Sphere

In this step, the trainee has to release the step next to the cube. The first transition has two conditions: it checks both if the trainee has released the sphere, and if the sphere is next to the cube. The second transition has only one condition, which is identical to the first condition of the first transition: it checks if the trainee has released the sphere.

First transition points to the end step. The second transition points to the `Grab Again` step. Note that the application resolves transitions in the listed order. If we would swap order of the transitions, we would have been stuck in an endless loop.

#### Grab Again

This step plays a voice line and points back to the `Grab Sphere` step.

#### End

This step plays a short message of accomplishment for the trainee.

### Training Scene Objects

#### Cube

The cube is not interactable. It has the `Transform In Range Detector` property which checks for objects near it.

#### Sphere

The sphere has a `Touchable` property and a `Grabbable` property to allow the trainee to interact with it.

## Modes Example

See the `ModesExample` scene inside the `Assets/Examples/Scenes/Advanced/` folder.

A training application can support training modes. With modes, the application plays the same course in slightly different ways. For example, a training mode could change how application highlights objects for the trainee. A training mode could disable highlights and audio hints completely. A trainee could use such a mode for additional challenge.

In this example, the trainee must snap a cube to a snap zone. The training application has three modes: the mode when the trainee always can see highlights, the mode when the trainee can see only highlights nearby, and the mode with no highlights at all.

The runtime configuration of the application defines which modes to use. See the `[TRAINING_CONFIGURATION]` game object: its `Configuration` dropdown is set to the `ModesExampleRuntimeConfiguration`. Other examples use the  `InnoactiveRuntimeConfiguration`.

## Chapters Example

See the `ChaptersExample` scene inside the `Assets/Examples/Scenes/Advanced/` folder.

To structure your course better, you could split it in separate chapters in the chapters view on the left side of the course editor window. Chapters will execute one by one in the listed order.