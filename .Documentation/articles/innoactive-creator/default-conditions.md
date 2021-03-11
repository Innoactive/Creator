# Default Conditions

Conditions need to be active in order to be fulfilled. As soon as a step is active, all containing Conditions are active as well (see [step transition](https://developers.innoactive.de/documentation/creator/latest/articles/innoactive-creator/transitions.html)).

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

#### Interaction in VR

To grab the object in VR, by default, the trainee has to move one of their hands/controllers into the object (it’s collider) and press the trigger button. The button to trigger can be changed by the template developer as described here (TO BE DONE).


#### Interaction in Desktop Mode

To grab the object in Desktop Mode, the trainee has to move the mouse on the object (so that the cast ray hit’s the objects collider) and press the left mouse button.

#### Interaction Highlights
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

#### Interaction in VR
In order to move the object into a collider, trainees need to move their hands/controllers into the object (it's collider) and press the trigger button. While holding the trigger button and moving the arm, the object stays grabbed by trainees hands. Trainees move the object into the specified collider and release it. The button to trigger can be changed by the template developer as described here (TO BE DONE).

#### Interaction in Desktop Mode
In order to move the object into a collider in Desktop Mode, trainees have to move the mouse on the object (so that the cast ray hit’s the objects collider) and press the left mouse button. The object is now grabbed, which is represented by an on-screen hand holding a miniature version of the object. In order to release the object into the collider, trainees move the mouse on the collider and press the lef mouse button. The on-screen hand disappears and the released object is now placed into the selected collider.

#### Interaction Highlights

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

This condition is fulfilled when the `Object` is within the specified `Range` of a `Reference object`.

#### Interaction in VR

In order to move the object nearby a collider, trainees need to move their hands/controllers into the object (it's collider) and press the trigger button. While holding the trigger button and moving the arm, the object stays attached to the trainees' hands. Trainees approach with the object the specified collider and release it. The button to trigger can be changed by the template developer as described here (TO BE DONE).

#### Interaction in Desktop Mode

In order to approach the sollider with an object in Desktop Mode, trainees have to move the mouse on the object (so that the cast ray hit’s the objects collider) and press the left mouse button. The object is now grabbed, which is represented by an on-screen hand holding a miniature version of the object. In order to release the object nearby the collider, trainees move the mouse on the collider and press the left mouse button. The on-screen hand disappears and the released object is now placed into the selected collider. `Object nearby` is difficult to simulate in Desktop Mode and is hence solved similar to `Move Object into collider`. 

#### Interaction Highlights

### Application Example

- The trainee is supposed to hold a RFID tag included in his gloves (represented by his controller) near a scanner.
- If a trainee moves to close to a machinery while it is operating, the emergency shut down is triggered.


### Configuration

- #### Object

    The `Training Scene Object` that should be in the radius of the `Reference object`. Make sure you add at least the `Training Scene Object` component to this game object in the Unity Inspector. 
    
- #### Reference Object

   The `Training Scene Object` you want to measure the distance from.


- #### Range

    In this field, you can set the maximum distance between the *Object* and the *Reference object* required to fulfill this condition.The distance is calculated as the Euclidean norm between the transform’s positions of Object and Reference Object.

- #### Required seconds inside

    In this field, you can set the time in seconds the `Object` should stay within the radius `Range` of the `Reference object`.

### Location of this Condition (for Developers)

This behavior is part of the <a href="https://github.com/Innoactive/Basic-Conditions-And-Behaviors" target="_blank">Basic-Conditions-And-Behaviors</a> component. The file is located <a href="https://github.com/Innoactive/Basic-Conditions-And-Behaviors/blob/develop/Runtime/Conditions/PositionalConditions/ObjectInRangeCondition.cs" target="_blank">here</a>  

------

## Release Object

### Description

This condition is fulfilled when the `Object` is released by the trainee’s controller. If a trainee is already holding the specified object in hand while this condition is active, it is fulfilled immediately.

#### Interaction in VR

In order to release the object in VR, trainees need to have an object grabbed with their hand/controllers (see [Grab Condition](https://developers.innoactive.de/documentation/creator/latest/articles/innoactive-creator/default-conditions.html#grab-object)). By releasing the trigger button, the object is released at the current position. The button to trigger can be changed by the template developer as described here (TO BE DONE).

#### Interaction in Desktop Mode

In order to release the object in Desktop mode, trainees need to have an object grabbed in Desktop Mode. An on-screen hand visually represents holding the object. By touching the `release button` next to the on-screen hand, the object is released and placed at the position it was grabbed before. 

#### Interaction Highlights

### Application Example

### Configuration

- #### Object

    the `Training Scene Object` to release. The object needs to have the Grabbable Property and a collider component configured. 

### Location of this Condition (for Developers)
This behavior is part of the <a href="https://github.com/Innoactive/Basic-Interaction-Component" target="_blank">Basic-Interaction</a> component. The file is located <a href="https://github.com/Innoactive/Basic-Interaction-Component/blob/develop/Runtime/Conditions/ReleasedCondition.cs" target="_blank">here</a>
 

------

## Snap Object

### Description

This condition is fulfilled when the `Object` is released into the `Zone to snap into`, which means the collider of the Object and collider of the Zone overlap. Adapt the collider size of the snap zone to increase or decrease the area where trainees can release the `Object`. Increasing the collider size of the snap zone, decreases the required *snap* precision and simplifies the trainees' interaction in VR. 
After the trainee releases the `Object` (see [Release Condition](https://developers.innoactive.de/documentation/creator/latest/articles/innoactive-creator/default-conditions.html#release-object)), it is moved to the snap Zones SnapPoint. To adjust this position, change the position of the SnapPoint child object of the `Zone to snap into` object.

####Snap zone Generator
####Manual snap zone generation
####Snap Zone settings

#### Interaction in VR
#### Interaction in Desktop Mode
#### Interaction Highlights

Snap zones are restricted to which objects can be snapped to them, which means, any object can be valid (i.e. it can be snapped to this zone) or invalid (it can not be snapped to this zone) for a snap zone. In case you are moving a valid object into a zone (c.f. above, colliders and stuff), the snap zone color changes to ‘Validation Color’, giving the trainee a positive feedback. In case you are moving an invalid object into a zone, the snap zone color changes to ‘Invalid Color’, giving the trainee the feedback that this is the wrong object for this zone. 
Which colors and which materials are to be used can be changed in the Snap Zones Settings. They can be found in tab “Innoactive” -> “Settings” -> “Snap Zones”.

### Application Example

- Teach trainees where to position certain component parts in a car.

### Configuration

- #### Object

    the Training Scene Object to place (snap). The object needs to have the `Grabbable Property` and a collider component configured. 
 

- #### Zone to snap into

    This field contains the `Training Scene Object` where the `Object` is required to be snapped. Make sure you added the `Snap Zone Property` component to the snap zone game object in the Unity Inspector. Besides, the object must have a collider component with the `Is Trigger` property *enabled*.

### Location of this Condition (for Developers)
This behavior is part of the <a href="https://github.com/Innoactive/Basic-Interaction-Component" target="_blank">Basic-Interaction</a> component. The file is located <a href="https://github.com/Innoactive/Basic-Interaction-Component/blob/develop/Runtime/Conditions/SnappedCondition.cs" target="_blank">here</a>
 
------

## Timeout

### Description

This condition is fulfilled when the time specified in `Wait (in seconds)` has elapsed.

### Application Example

### Configuration

- #### Wait (in seconds)

    Set the time in seconds that should elapse before this condition is fulfilled.

### Location of this Condition (for Developers)

This behavior is part of the <a href="https://github.com/Innoactive/Basic-Conditions-And-Behaviors" target="_blank">Basic-Conditions-And-Behaviors</a> component. The file is located <a href="https://github.com/Innoactive/Basic-Conditions-And-Behaviors/blob/develop/Runtime/Conditions/TimeoutCondition.cs" target="_blank">here</a>  


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
