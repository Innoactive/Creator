# Default Conditions

Conditions need to be active in order to be fulfilled. Condtions are always active as soon as the [step transition](https://developers.innoactive.de/documentation/creator/latest/articles/innoactive-creator/transitions.html) containing this condition is active.

The following conditions are part of the `Basic Conditions and Behaviors` and the `Basic Interactions` Component. The Innoactive Base Template provides them by default.

Take a look at the [Training Scene Object](training-scene-object.md) article if you have not read it yet. It will help you to understand how to handle training scene objects and training properties.

See [this article](step-inspector.md) to learn Step Inspector controls.

## Content

- [Grab Object](#grab-object)
- [Move Object into Collider](#move-object-into-collider)
- [Object Nearby](#object-nearby)
- [Release Object](#release-object)
- [Snap Object](#snap-object)
- [Timeout](#timeout)
- [Touch Object](#touch-object)
- [Use Object](#use-object)

------

## Grab Object

### Description

This condition is fulfilled when the trainee grabs the `Object`. 
The condition is also fulfilled if the trainee already grabs the `Object` before the step is activated, so, if a trainee is already holding 
the specified object in hand while this condition is active, it is fulfilled immediately.

### Interaction in VR

To grab the object in VR, by default, the trainee has to move one of their hands/controllers into the object (it’s collider) and press the trigger button. The button to trigger can be changed by the template developer as described here (TO BE DONE).


### Interaction in Desktop Mode

To grab the object in Desktop Mode, the trainee has to move the mouse on the object (so that the cast ray hit’s the objects collider) and press the left mouse button. The button to trigger can be changed by the template developer as described here (TO BE DONE).

### Interaction Highlights
Before the controller button or, respectively, mouse button is pressed, by default, the object is already highlighted to indicate that the trainee can now grab this object. This “on hover” effect can be changed by the template developer here (TO BE DONE). The training designer can also change this in the interactable highlighter property of the given object as described here. You can easily enable/disable this hover effect, or change the highlight color and transparency (TO BE DONE).

### Application Example

- Trainees need to grab objects to position them somewhere.
- Trainees need to grab a tool to use it.
- Trainees need to grab a component part to assemble it to another component part.


### Configuration

- #### Object

    The `Training Scene Object` to grab. The object needs to have the `Grabbable Property` and a collider component configured. The collider defines the area where the trainee can grab this object.

### Location of this Condition (for Developers)
This behavior is part of the <a href="https://github.com/Innoactive/Basic-Interaction-Component" target="_blank">Basic-Interaction</a> component. The file is located <a href="https://github.com/Innoactive/Basic-Interaction-Component/blob/develop/Runtime/Conditions/GrabbedCondition.cs" target="_blank">here</a>
 
------

## Move Object into Collider
### Description

This condition is fulfilled when the `Object` is within the specified `Collider` for the required amount of time (`Required seconds inside`) while this condition is active.

### Interaction in VR
### Interaction in Desktop Mode
### Interaction Highlights

### Application Example
- You train air traffic controllers to give certain signs by waving red paddles. By positioning the collider above the trainee's head and one to the trainees right side, you can validate if the trainee raised one red paddle over his head and one to the side.

### Configuration

- #### Object

    The Training Scene Object to grab. The object needs to have the `Grabbable Property` and a collider component configured. The collider defines the area where the trainee can grab this object.



- #### Collider

    The Training Scene Object with the collider inside which you want to move the Object to. Make sure that the Collider Object has a collider added and that the option ‘Is Trigger’ is enabled.

- #### Required seconds inside

    Set the time in seconds the Object should stay within the Collider.

### Location of this Condition (for Developers)

This behavior is part of the <a href="https://github.com/Innoactive/Basic-Conditions-And-Behaviors" target="_blank">Basic-Conditions-And-Behaviors</a> component. The file is located <a href="https://github.com/Innoactive/Basic-Conditions-And-Behaviors/blob/develop/Runtime/Conditions/PositionalConditions/ObjectInColliderCondition.cs" target="_blank">here</a>  

------

## Object Nearby

### Description

The `Object Nearby` condition triggers when the *tracked object* is within the specified *range* from the *reference object* for the required amount of time while this condition is active.

### Configuration

- #### Reference Object

    This field contains the `Training Scene Object` you want to measure the distance from.

- #### Tracked Object

    This field contains the `Training Scene Object` that should be in the radius of the *reference object*. Make sure you add at least the `Training Scene Object` component to this game object in the Unity Inspector. You would usually move this object close to the reference object.

- #### Range

    In this field, you can set the maximum distance between the *tracked object* and the *reference object* required to fulfill this condition.

- #### Required seconds inside

    In this field, you can set the time in seconds the *tracked object* should stay within the radius of the *reference object*.

------

## Release Object

### Description

The `Release Object` condition is fulfilled once the *Grabbable object* is released while this condition is active. It is fulfilled immediately if the condition becomes active and the *Grabbable object* is not being held.

### Configuration

- #### Grabbable object

    This field contains the `Training Scene Object` that is required to be released. Make sure you added the `Grabbable Property` component to the game object in the Unity Inspector. Besides, the object must have a collider component in order to be grabbable.

    [![Grabbable Properties](../images/conditions/grabbable_properties.png "")](../images/conditions/grabbable_properties.png)

------

## Snap Object

### Description

The `Snap Object` condition is fulfilled once the specified *Object to snap* is being snapped into the *Zone to snap into* (i.e. the game object is released within the snap zone) while this condition is active.

### Configuration**

- #### Object to snap

    This field contains the `Training Scene Object` that is required to be snapped. Make sure you added the `Snappable Property` component to the game object in the Unity Inspector. Besides, the object must have a collider component in order to be grabbed and snapped.

    [![Snappable Properties](../images/conditions/snappable_properties.png "")](../images/conditions/snappable_properties.png)

- #### Zone to snap into

    This field contains the `Training Scene Object` where the *Object to snap* is required to be snapped. Make sure you added the `Snap Zone Property` component to the snap zone game object in the Unity Inspector. Besides, the object must have a collider component with the `Is Trigger` property *enabled*.

    [![Snap Zone Properties](../images/conditions/snap_zone_properties.png "")](../images/conditions/snap_zone_properties.png)

------

## Timeout

### Description

The `Timeout` condition is fulfilled once the specified time is elapsed while this condition is active.

### Configuration

- #### Wait for seconds

    In this field, you can set the time in seconds that should elapse before this condition is fulfilled.

------

## Touch Object

### Description

The `Touch Object` condition is fulfilled when the *Touchable object* is touched by the controller while this condition is active. It is fulfilled immediately if the condition becomes active while the *Touchable object* is being touched.

### Configuration

- #### Touchable object

    This field contains the `Training Scene Object` that is required to be touched. Make sure you added the `Touchable Property` component to the target game object in the Unity Inspector. Besides, the object must have a collider component in order to be touchable.

    [![Touchable Properties](../images/conditions/touchable_properties.png "")](../images/conditions/touchable_properties.png)

------

## Use Object

### Description

The `Use Object` condition is fulfilled as the *Usable object* is used by pressing the `Use` button of the controller while being touched or grabbed when the condition is active.

### Configuration

- #### Usable object

    This field contains the `Training Scene Object` that is required to be used. Make sure you added the `Usable Property` component to the target game object in the Unity Inspector. Besides, the object must have a collider component, as it needs to be touched or grabbed.

    [![Usable Properties](../images/conditions/usable_properties.png "")](../images/conditions/usable_properties.png)
