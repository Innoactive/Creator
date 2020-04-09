# Example Project Documentation

You can download the `Innoactive Creator Examples` package [here](http://developers.innoactive.de/components/#training-module). It contains the `Innoactive Creator`, `Innoactive Training Template`, and examples in a single project. You can find more information on how to import this package in this [tutorial](../getting-started/evaluator.md).

The `Assets/Examples/` folder contains some examples courses that showcase different features of the Innoactive Creator and can provide a guideline for building your own courses.

The subfolders are organized as follows.
- **Simple**: Contains a scene with the simple training course that is created in the [tutorial](../getting-started/designer.md).
- **Advanced**: Contains multiple scenes showcasing a variety of courses. You can find them listed below.
- **Source**: Contains custom code for the example courses.

Below the advanced examples are described in detail, it is assumed that the reader is familiar with the [tutorials](../getting-started/designer.md) for this project and knows the basics of working with the Innoactive Creator. A basic knowledge of Unity is required, but the trickier setup steps are explained.

## Localization Example

**Scene:** `Assets/Examples/Advanced/LocalizationExample`

This example shows how localization works and also is the sample solution for the training course you can develop in the tutorial. For more detailed information about it head over to the [tutorial](../getting-started/designer.md). The trainee has to bring the sphere next to the cube, then the cube will fly to the sky.

At the moment, this is the only example course fully localized in English and German. This means that the training folder located at `Assets\StreamingAssets\Training\LocalizationExample` contains not only the course JSON file, but also a `Localization` subfolder with the localization JSON files. The `Play TTS Audio` behaviors in this course correctly reference the localization keys.

It is possible to select English or German before clicking the `Start Training` button. The voiceover will be in the selected language.

## Branching Example

**Scene:** `Assets/Examples/Advanced/BranchingExample`

This example showcases how it is possible to branch a step using multiple transitions, each leading to a separate step. This makes it possible to introduce choices in a training course, or task that can be completed in any order.

The sample training course requires the trainee to snap the sphere and the cube to their matching snapzones; the trainee can decide which object to snap first and the course will be completed when both objects are snapped.

### Step Description

**Snap Objects**: This step has two outgoing transitions with different activation conditions: one activates when the sphere is snapped to its target snapzone, the other when the cube is snapped. Depending on which object the trainee snaps first, the course will proceed to a different step.

**Snap Sphere/Snap Cube**: These are the follow-up steps from *Snap Objects*. They require the trainee to place the object that hasn't been snapped yet, e.g. if the sphere was snapped in the previous step, the trainee will now be required to snap the cube in the *Snap Cube* step.

**End**: Both the previous steps converge in the *End* step, which simply acknowledges that the course is completed.

### Training Scene Objects

The training course makes use of four training scene objects: the two interactable objects, Cube and Sphere, and the two corresponding snap zones. They are setup as follows:

**Cube/Sphere**: In order to be snapped, these need to receive a `Snappable Property`. Adding one will automatically make them touchable and grabbable by adding the corresponding property. Aside from the Innoactive Creator properties, these are simple meshes with collider.

**Snap Zones**: These need, along with being training scene objects, a `Snap Zone Property` component and a properly configured snap zone. This can be accomplished by using the following components and configuration:

* A `SnapZone` component.
    - The `Shown Highlight Object` can be assigned an object used to display a preview of the object in the final snapped position. We could use directly the cube and the sphere, but we decided to use copies of those called `CubeSnapTarget` and `SphereSnapTarget`. This makes sure no logic is present on those object (e.g. `Training Scene Object` component) and allows to use a different transparent material. This is optional and if not set, no object will be shown.
    - The `Interactable Hover Mesh Material` can be assigned a material which will be used for showing the highlight object on hovering in the snap zone. This is optional and if not set, a default material will be created.
    - The `Show Interactable Hover Mesh` checkbox ensures snap targets are visible, when an object is hovering in the snap zone.
    - All other settings, except colors, are set to default.
* A [collider](https://docs.unity3d.com/Manual/CollidersOverview.html) with the `Is Trigger` checkbox ticked: the object will snap if inside this collider. Setting it to a trigger makes it possible to move through it instead of processing collisions. The colliders are respectively a `Box Collider` for the cube and a `Sphere Collider` for the sphere, both set to exactly the size of the snap target.
* A [`Rigidbody`](https://docs.unity3d.com/Manual/class-Rigidbody.html) component.

#### Restricting Objects that can interact with a Snap Zone
In our example, we want the *Cube* to only be snapped in its corresponding snap zone called _CubeSnapZone_ but not in the Snap Zone designated for the *Sphere*, called _SphereSnapZone_ and vice versa.

For this, every `SnapZone` has a field called `Interaction Layer Mask`. In here you can set which objects can be snapped into this snap zone. It uses Unity's [Layers](https://docs.unity3d.com/Manual/Layers.html), so you can add new ones for your own need.

Additionally, you have to assign the correct layer to the interactable objects - the cube and the sphere. Both objects have an `Interactable Object` component which has an `InteractionLayerMask` field. Only interactors which have one of the allowed layers can interact with this object. For the *Cube* everything but the layer _Sphere_ is selected and for the *Sphere* everything but the _Cube_ layer is checked. *Note:* It is recommended to only unselect the layers you do not want to allow interaction with otherwise you might remove controller interaction.


> Learn more about [`Interaction Layer Masks`](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@0.9/manual/index.html#interactionlayermask).

## Looping Example

**Scene:** `Assets/Examples/Advanced/LoopingExample`

This example showcases how it is possible to feed a branching step back to a previous step, in order to loop a sequence until a certain condition is met.

In this course, the trainee has to bring the sphere next to the cube. If the sphere is dropped before the destination, the course will loop back to a previous step, where the trainee is asked to pick up the sphere.

### Step Description

**Intro**: An introductory voice over which will automatically proceed to the next step once done. This is a separate step in order to be outside of the loop, we don't want it repeated multiple times during the course.

**Grab Sphere**: This step asks the trainee to grab the sphere. This is the first step of the loop, in fact it is possible to reach it both from the *Intro* step and from the *Grab Again* step.

**Release Sphere**: Asks the trainee to release the sphere. Has two transitions: if the trainee releases the sphere *and* the sphere is in range of the cube, it proceeds to the *End* step, else it leads to the *Grab Again* step.

**Grab Again**: The trainee dropped the sphere while it was too far from the cube. This step does nothing more than pointing out this failure before automatically transitioning back to the *Grab Sphere* step for another try.

**End**: The course is successfully completed, a short message plays and the course ends.

### Training Scene Objects

The setup is rather simple in this example: the only training scene objects are the cube and the sphere. Both are meshes with colliders, with the following Innoactive Creator components added.

**Cube**: The cube is not interactable. In addition to the `Training Scene Object` components, it only needs a `Transform In Range Detector Property` to allow the *Release Sphere* step to check the distance between the sphere and the cube.

**Sphere**: The sphere is just a standard `Training Scene Object` with a `Touchable Property` and a `Grabbable Property`.

## Modes Example

**Scene:** `Assets/Examples/Advanced/ModesExample`

Training courses can have different modes, for example to change the difficulty. Modes are part of the template's runtime configuration. The `Innoactive Template` features two modes, "Default" and "No audio hints". The latter removes all voiceover instructions from a training course.

In this example, the trainee must snap a cube to a snapzone. The trainer can however select from different training modes: the snapzone highlight could be visible all the time, so it is clear what has to be done, or only when the cube hovers next to it, requiring the trainee to know where to go. Finally, it is possible to have no highlight at all, so the cube must be placed in the correct spot without any affordance or feedback.

In order to build the new modes for this example, a new runtime configuration had to be created. This is a job for a template developer, but it is possible to see, on the `[TRAINING_CONFIGURATION]` game object, that the `ModesExampleRuntimeConfiguration` is selected instead of the default `InnoactiveRuntimeConfiguration`.

This configuration overrides the default one, providing the three highlighting modes "Always", "When Near" and "Never" instead of the default Innoactive Template modes. Setting a different configuration in the `[TRAINING_CONFIGURATION]` game object will change the modes available for the training course.

The training mode can be selected from the drop-down list next to the `Start Training`/`Skip Step` button.

### Training Scene Objects

**Cube**: In order to be snapped, it needs to have a `Snappable Property`. Adding one will automatically make it touchable and grabbable as well.

**CubeSnapZone**: It is a snapzone set up the standard way, a explained in the [branching example](#branching-example).

### Step Description

**Grab Cube**: This is just an introductory step. The introduction is rather long, but the `Play TTS Audio` behavior has its `Is blocking` checkbox unchecked. This means it will be interrupted as soon as the trainee completes the step by grabbing the cube.

**Snap Cube**: This step checks that the cube is snapped in place. It works exactly as described earlier, but it's worth noting it has a `Play TTS Audio` behavior set to play in the execution stage `After Step Execution`. This means it will not play at the beginning of the step, but rather when the step is completed by satisfying the condition. This eliminates the necessity to have a further *End* step. Note that this behavior has its `Is blocking` checkbox checked. If it didn't, the step would complete immediately, the behavior would be skipped, and no audio would play.

## Chapters Example

**Scene:** `Assets/Examples/Advanced/ChaptersExample`

This training course is built in multiple chapters. The first chapter requires the trainee to put the sphere in the box. In the second chapter, the trainee will snap the cube to the box in order to lock it. When this is done, the lid will descend and close the box. In the final chapter the trainee's success is celebrated with some confetti. 

### Training Scene Objects

**Sphere**: A grabbable interactable sphere mesh with collider, meaning it is a `Training Scene Object` with a `Touchable Property` and a `Grabbable Property`.

**Cube**: In order to be snapped, it needs to have a `Snappable Property`. Adding one will automatically make it touchable and grabbable as well. Aside from the properties, it has a cube mesh with collider.

**CubeSnapZone**: Standard snapzone, set up like in the [branching example](#branching-example). Note this snapzone should not be visible during the first chapter, but the game object is activated in the scene. We will need to deactivate it during the first step in the first chapter, *Place Sphere*.

**Box**: The object itself only contains the collider (set as trigger) used to detect if the sphere is inside. This is a box collider the same size as the interior of the box. In order to work in the workflow editor, this also requires a `Collider With Trigger Property`. The child objects are meshes with colliders that compose the sides and bottom of the box.

**BoxLid**: This training scene object needs no specific property, but the training course has to be aware of it in order to move it. The child objects are meshes with colliders that represent the lid.

**BoxLidTarget** An empty game object whose position is used by the training course as the final position of the box lid.

### Step Description

**Fill Box/Place Sphere**: The first chapter contains only this step, which ends when the `Move Object into Collider` condition is completed. The condition is set with *Box* as the `Object to collide into`, and *Sphere* as the `Target object` to move in the collider. `Required seconds inside` is set to 0 seconds, so the condition will complete as soon as the sphere's collider intersects with the trigger inside the box. This step also deactivates the *CubeSnapZone* as soon as the course starts, so it won't be visible until the second chapter.

**Lock Box/Snap Cube**: In the second chapter, the trainee is required to snap the cube in front of the box. This step is similar to the snapping steps in other examples, but note how it immediately enables the *CubeSnapZone*, making it visible in the scene.

**Lock Box/Lock Box**: After the trainee is successful, this step takes care of animating the *BoxLid* via a `Move Object` behavior. The object will be moved to the position of `BoxLidTarget` and will reach the final position after 3 seconds.

**Celebration/Confetti**: After the box has closed the final chapter is entered. In this chapter the success of the trainee is celebrated with confetti which is done in a single step with the custom `Spawn Confetti Behavior`.
