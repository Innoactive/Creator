# Default Conditions

Conditions need to be active in order to be fulfilled. As soon as a step is active, all containing Conditions are active as well (see [step transition](transitions.md)).

The following conditions are part of the `Basic Conditions and Behaviors` and the `Basic Interactions` component. The Innoactive Creator package provides them by default.

Take a look at the [Training Scene Object](training-scene-object.md) article if you have not read it yet. It will help you understand how to handle training scene objects and training properties.

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
- [Teleport](#teleport)

------

## Grab Object

### Description

This condition is fulfilled when the trainee grabs the `Object`. 
The condition is also fulfilled if the trainee already grabbed the Object before the step was activated, so, if the trainee is already holding
the specified object.

#### Interaction in VR

To grab the object in VR, trainees have to move one of their hands/controllers into the object (more precisely its collider) and press the trigger button. The trigger button can be changed by the template developer as described [here](../xr/index.md).

Before the controller button is pressed, the object is already highlighted to indicate that the trainee can now grab this object. The training designer can change this in the interactable highlighter script of the given object. You can enable/disable this interaction effect, change the highlight color and transparency for `On Touch`, `On Grab`, and `On Use`.


#### Interaction in Desktop Mode

To grab the object in Desktop Mode, the trainee has to move the mouse over the object (so that the ray hits the object's collider) and press the left mouse button.

Before the mouse button is pressed, the object is already highlighted to indicate that the trainee can now grab this object. This “on hover” effect can be changed by the template developer in the `Hover Highlight Handler` of the Desktop Mode Rig prefab. 



### Application Example

- Trainees need to grab objects to position them somewhere.
- Trainees need to grab a tool to use it.
- Trainees need to grab a component part to assemble it to another component part.


### Configuration

- **Object**

    The `Training Scene Object` to grab. The object needs to have the `Grabbable Property` and a collider component configured. The collider defines the area where the trainee can grab this object.

### Location of this Condition (for Developers)
This behavior is part of the [Basic-Interaction](https://github.com/Innoactive/Basic-Interaction-Component) component. The file is located [here](https://github.com/Innoactive/Basic-Interaction-Component/blob/develop/Runtime/Conditions/GrabbedCondition.cs).
 
------

## Move Object into Collider

### Description

This condition is fulfilled when the `Object` is within the specified `Collider` for the required amount of time (`Required seconds inside`) while this condition is active.

#### Interaction in VR
In order to move the object into a collider, trainees need to move their hands/controllers into the object (more precisely its collider) and press the trigger button. While holding the trigger button and moving the arm, the object stays grabbed by trainees hands. Trainees move the object into the specified `Collider` and release it. The button to trigger can be changed by the template developer as described [here](../xr/index.md).

#### Interaction in Desktop Mode
In order to move the object into a collider in Desktop Mode, trainees have to move the mouse over the object (so that the cast ray hits the objects collider) and press the left mouse button. The object is now grabbed, which is represented by an on-screen hand holding a miniature version of the object. In order to release the object into the `Collider`, trainees move the mouse on the `Collider` and press the left mouse button. The on-screen hand disappears and the released object is now placed into the selected `Collider`.


### Application Example
- Train air traffic controllers to give certain signs by waving red paddles. By positioning one collider above the trainee's head and one to the trainees right side, you can validate if the trainee raised one red paddle over his head and one to the side.

### Configuration

- **Object**

    The `Training Scene Object` to grab. The object needs to have the `Grabbable Property` and a collider component configured. The collider defines the area where the trainee can grab this object.



- **Collider**

    The `Training Scene Object` with the collider you want to move the `Object` to. Make sure that it has a collider added and that the option `Is Trigger` is enabled.

- **Required seconds inside**

    Set the time in seconds that the `Object` should stay inside the `Collider`.

### Location of this Condition (for Developers)

This behavior is part of the [Basic-Conditions-And-Behaviors](https://github.com/Innoactive/Basic-Conditions-And-Behaviors) component. The file is located [here](https://github.com/Innoactive/Basic-Conditions-And-Behaviors/blob/develop/Runtime/Conditions/PositionalConditions/ObjectInColliderCondition.cs).

------

## Object Nearby

### Description

This condition is fulfilled when the `Object` is within the specified `Range` of a `Reference object`.

#### Interaction in VR

In order to move the object nearby a collider, trainees need to move their hands/controllers into the object (more precisely its collider) and press the trigger button. While holding the trigger button and moving the arm, the object stays attached to the trainees' hands. Trainees approach with the object the specified collider (`Reference Object`) and release it. The button to trigger can be changed by the template developer as described [here](../xr/index.md).

#### Interaction in Desktop Mode

In order to approach the collider with an object in Desktop Mode, trainees have to move the mouse over the object (so that the cast ray hits the objects collider) and press the left mouse button. The object is now grabbed, which is represented by an on-screen hand holding a miniature version of the object. In order to release the object nearby the collider, trainees move the mouse over the collider and press the left mouse button. The on-screen hand disappears and the released object is now placed into the selected collider. `Object nearby` is difficult to simulate in Desktop Mode and is hence solved similar to `Move Object into Collider`. 


### Application Example

- The trainee is supposed to hold an RFID tag that is integrated into their gloves (represented by his controller) near a scanner.
- If a trainee moves too close to a machinery while it is operating, the emergency shut down is triggered.


### Configuration

- **Object**

    The `Training Scene Object` that should be in the radius of the `Reference Object`. Make sure you add at least the `Training Scene Object` component to this game object in the Unity Inspector. 
    
- **Reference Object**

   The `Training Scene Object` you want to measure the distance from.


- **Range**

    In this field, you can set the maximum distance between the *Object* and the *Reference object* required to fulfill this condition. The distance is calculated as the Euclidean norm between the transform’s positions of Object and Reference Object.

- **Required seconds inside**

    In this field, you can set the time in seconds the `Object` should stay within the radius `Range` of the `Reference Object`.

### Location of this Condition (for Developers)

This behavior is part of the [Basic-Conditions-And-Behaviors](https://github.com/Innoactive/Basic-Conditions-And-Behaviors) component. The file is located [here](https://github.com/Innoactive/Basic-Conditions-And-Behaviors/blob/develop/Runtime/Conditions/PositionalConditions/ObjectInRangeCondition.cs).

------

## Release Object

### Description

This condition is fulfilled when the `Object` is released by the trainee’s controller. If the trainee is not already holding the specified object in hand while this condition is active, it is fulfilled immediately.

#### Interaction in VR

In order to release the object in VR, trainees need to have an object grabbed with their hand/controllers (see [Grab Condition](#grab-object)). By releasing the trigger button, the object is released at the current position. The button to trigger can be changed by the template developer as described [here](../xr/index.md).

#### Interaction in Desktop Mode

In order to release the object in Desktop mode, trainees need to have an object grabbed in Desktop Mode. An on-screen hand visually represents holding the object. By touching the `release button` next to the on-screen hand, the object is released and placed at the position it was grabbed before. 


### Application Example

### Configuration

- **Object**

    The `Training Scene Object` to release. The object needs to have the `Grabbable Property` and a collider component configured. 

### Location of this Condition (for Developers)
This behavior is part of the [Basic-Interaction](https://github.com/Innoactive/Basic-Interaction-Componen) component. The file is located [here](https://github.com/Innoactive/Basic-Interaction-Component/blob/develop/Runtime/Conditions/ReleasedCondition.cs).
 

------

## Snap Object

### Description

This condition is fulfilled when the `Object` is released into the `Zone to snap into`, which means the collider of the Object and collider of the Zone overlap. Adapt the collider size of the snap zone to increase or decrease the area where trainees can release the `Object`. Increasing the collider size of the snap zone, decreases the required *snap* precision and simplifies the trainees' interaction in VR. 
After the trainee releases the `Object` (see [Release Condition](#release-object)), it is moved to the snap Zones SnapPoint. To adjust this position, change the position of the SnapPoint child object of the `Zone to snap into` object.

#### Snap Zone Generator
For any snappable object you can generate a snap zone that can snap exactly this object and makes it possible to use as a `Zone to snap into`. To do so, navigate to the `Snappable Property` in Unity's Inspector and click on the button `Create Snap Zone`. 

[![Snap Zone Generator](../images/default-conditions/snapzonegenerator.png "")](../images/default-conditions/snapzonegenerator.png)


#### Manual Snap Zone Creation
Instead of the automatic generation as described above, you can do those steps also manually. Please refer to available documention on the `XRSocketInteractor` from Unity or related sources. You can also make changes to the automatically created snap zone to adapt them to your needs. Please note, that these changes might impact the training process logic and can lead to breaking Creator logic. Do so on your own risk.

#### Feed Forward for Snap Zones

Snap zones are restricted to which objects can be snapped to them, which means, any object can be valid (i.e. it can be snapped to this zone) or invalid (it can not be snapped to this zone) for a snap zone. In case you are moving a valid object into a zone (c.f. above, colliders and stuff), the snap zone color changes to ‘Validation Color’ (green), giving the trainee a positive feedback. In case you are moving an invalid object into a zone, the snap zone color changes to ‘Invalid Color’ (red), giving the trainee the feedback that this is the wrong object for this zone. 
Which colors and which materials are to be used can be changed in the Snap Zones parameters and settings.

#### Snap Zone Parameters and Settings
To change the highlight color or validation hover material of a dedicated snap zone, navigate to the snap zone object in the Unity inspector. In the script `Snap Zone` you will find these parameters among others. 

[![Snap Zone Parameters](../images/default-conditions/snapzoneparameters.png "")](../images/default-conditions/snapzoneparameters.png)


To change the colors and materials of all snap zones in the scene, select them in the Creator snap zone settings and press 'Apply settings in current scene'.

[![Snap Zone Settings](../images/default-conditions/snapzonesettings.png "")](../images/default-conditions/snapzonesettings.png)


The snap zone settings can be found in tab “Innoactive” -> “Settings” -> “Snap Zones”.

### Application Example

- Teach trainees where to position certain component parts in a car.

### Configuration

- **Object**

    The `Training Scene Object` to place (snap). The object needs to have the `Snappable Property` and a collider component configured. 
 

- **Zone to snap into**

    This field contains the `Training Scene Object` where the `Object` is required to be snapped. Make sure you added the `Snap Zone Property` component to the snap zone game object in the Unity Inspector. Besides, the object must have a collider component with the `Is Trigger` property *enabled*.

### Location of this Condition (for Developers)
This behavior is part of the [Basic-Interaction](https://github.com/Innoactive/Basic-Interaction-Component) component. The file is located [here](https://github.com/Innoactive/Basic-Interaction-Component/blob/develop/Runtime/Conditions/SnappedCondition.cs).
 
------

## Timeout

### Description

This condition is fulfilled when the time specified in `Wait (in seconds)` has elapsed.

### Application Example

### Configuration

- **Wait (in seconds)**

    Set the time in seconds that should elapse before this condition is fulfilled.

### Location of this Condition (for Developers)

This behavior is part of the [Basic-Conditions-And-Behaviors](https://github.com/Innoactive/Basic-Conditions-And-Behaviors) component. The file is located [here](https://github.com/Innoactive/Basic-Conditions-And-Behaviors/blob/develop/Runtime/Conditions/TimeoutCondition.cs). 


------

## Touch Object

### Description

This condition is fulfilled when the `Object` is touched by the trainee’s controller.  If a trainee is already touching the specified object while this condition is active, it is fulfilled immediately.

#### Interaction in VR
In order to touch an object in VR, trainees have to move one of their hands/controllers into the object (more precisely its collider). 

#### Interaction in Desktop Mode
In order to touch an object in desktop mode, trainees have to move the mouse over the object (so that the ray hits the object's collider). The object is visually highlighted to indicate that it is interactive. Touch the object by clicking it with the left mouse button.

### Application Example
- The trainee needs to push a button on a maschine.

### Configuration

- **Object**

    The `Training Scene Object` to be touched. The object needs to have the `Touchable Property` and a collider component configured. 

### Location of this Condition (for Developers)
This behavior is part of the [Basic-Interaction](https://github.com/Innoactive/Basic-Interaction-Component) component. The file is located [here](https://github.com/Innoactive/Basic-Interaction-Component/blob/develop/Runtime/Conditions/TouchedCondition.cs).
 
 

------

## Use Object

### Description

This condition is fulfilled when the `Object` is used by pressing the *Use* button of the controller while being touched or grabbed.

#### Interaction in VR
To use an object in VR, trainees have to move one of their hands/controllers into the object (more precisely its collider) and press the trigger button. The trigger button can be changed by the template developer as described [here](../xr/index.md).

#### Interaction in Desktop Mode
In order to use an object in desktop mode, it needs to be grabbed before (see [Grab Condition](#grab-object)). A grabbed object is displayed in miniature size inside an on-screen hand. Trainees click the button next to it, titled *use object*, to use the previously selected object.

### Application Example
- The trainee picks up a drill and drills in screws.

### Configuration

- **Object**

    The `Training Scene Object` that is required to be used.The `Object` needs to have the `Usable Property` and a collider component configured.


### Location of this Condition (for Developers)
This behavior is part of the [Basic-Interaction](https://github.com/Innoactive/Basic-Interaction-Component) component. The file is located [here](https://github.com/Innoactive/Basic-Interaction-Component/blob/develop/Runtime/Conditions/UsedCondition.cs).

## Teleport

### Description

This condition is fulfilled when the trainee teleports to the referenced `Teleportation Point`. Previous teleportation actions made into the `Teleportation Point` are not considered.

The provided `Teleportation Property` is based on the Unity XR Interaction Toolkit's `Teleportation Anchor`. For further reference, please check out the XR Interaction Toolkit  [documentation](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@1.0/api/UnityEngine.XR.Interaction.Toolkit.TeleportationProvider.html).

#### Configuring the Teleportation Point

The `Teleportation Property` can be set as a **Default Teleportation Anchor** by clicking on the `Set Default Teleportation Anchor` button. You can find it when selecting the `Teleportation Point` and viewing it in the Unity Inspector.

[![Teleportation Property](../images/default-conditions/teleportationproperty.PNG "")](../images/default-conditions/teleportationproperty.PNG)

This will configure the attached `Teleportation Anchor`. It will provide a visual element in the Unity Editor that helps the training designer to place the `Teleportation Point` in the scene. This visual element will also be shown in the virtual world during training execution to guide the trainee.

[![Setting a Default Teleportation Anchor](../images/default-conditions/defaultteleportationanchor.gif "Snap Zone")](../images/default-conditions/defaultteleportationanchor.gif)

#### Interaction in VR

To teleport in VR use the teleport controller and point at the highlighted `Teleportation Point`. This will teleport you to this point.

#### Interaction in Desktop Mode

To teleport in Desktop Mode, click on the highlighted `Teleportation Point`. This will teleport you to this point.

### Application Example
- The trainee has to move to another workstation to continue with the process.

### Configuration

- **Teleportation Point**

    The `Teleportation Property` is used as the location point for the trainee to teleport to.

### Location of this Condition (for Developers)
This behavior is part of the [Basic-Interaction](https://github.com/Innoactive/Basic-Interaction-Component) component. The file is located [here](https://github.com/Innoactive/Basic-Interaction-Component/blob/develop/Runtime/Conditions/TeleportCondition.cs).
