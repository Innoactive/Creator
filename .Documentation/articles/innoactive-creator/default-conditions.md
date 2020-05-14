# Default Conditions

The following conditions are part of the Basic Conditions and Behaviors Component. The Innoactive Base Template provides them by default.

Take a look at the [Training Scene Object](training-scene-object.md) article if you have not read it yet. It will help you to understand how to handle training scene objects and training properties.

See [this article](step-inspector.md) to learn Step Inspector controls.

- **Default Conditions:**
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

The `Grab Object` condition is fulfilled once the specified `Training Scene Object` is grabbed while this condition is active. It is fulfilled immediately if the condition becomes active while the `Training Scene Object` is already being held.

### Configuration

- **Grabbable object**\
    This field contains the `Training Scene Object` with a `Grabbable Property` that is required to be grabbed.
    Besides, the object must have a collider component in order to be grabbed.

    [![Grabbable Properties](../images/conditions/grabbable_properties.png "")](../images/conditions/grabbable_properties.png)

------

## Move Object into Collider

### Description

The `Move Object into Collider` condition is fulfilled if the target object is within the specified collider for the required amount of time while this condition is active.

### Configuration

- **Object to collide into**\
    This field contains the `Training Scene Object` with the collider inside which you want to move the target object. Make sure you added the `Collider With Trigger Property` component to the game object with the collider. Besides, the collider component needs the `Is Trigger` property to be *enabled*.

    [![Object with trigger collider](../images/conditions/object_into_colliders_properties_1.png "")](../images/conditions/object_into_colliders_properties_1.png)

- **Target object**\
    This field contains the `Training Scene Object` that you want to move into the *Object to collide into*. Make sure you added the `Training Scene Object` component to the game object in the Unity Inspector. Besides, the object must have a collider component with the `Is Trigger` property *disabled*.

    [![Target object with collider](../images/conditions/object_into_colliders_properties_2.png "")](../images/conditions/object_into_colliders_properties_2.png)

- **Required seconds inside**\
    In this field, you can set the time in seconds the target object should stay within the collider.
  
------

## Object Nearby

### Description

The `Object Nearby` condition triggers when the *first object* is within the specified *range* from the *second object* for the required amount of time while this condition is active.

### Configuration

- **First object**\
    This field contains the `Training Scene Object` you want to measure distance from. 

- **Second object**\
    This field contains the `Training Scene Object` you want to be in the radius of the *first object*. Make sure you added at least the `Training Scene Object` component to the target game object in the Unity Inspector.

- **Range**\
    In this field, you can set the maximum distance between the *first* and *second object* required to fulfill this condition.

- **Required seconds inside**\
    In this field, you can set the time in seconds the *second object* should stay within the radius of the *first object*.

------

## Release Object

### Description

The `Release Object` condition is fulfilled once the *Grabbable object* is released while this condition is active. It is fulfilled immediately if the condition becomes active and the *Grabbable object* is not being held.

### Configuration

- **Grabbable object**\
    This field contains the `Training Scene Object` that is required to be released. Make sure you added the `Grabbable Property` component to the game object in the Unity Inspector. Besides, the object must have a collider component in order to be grabbable.

    [![Grabbable Properties](../images/conditions/grabbable_properties.png "")](../images/conditions/grabbable_properties.png)

------

## Snap Object

### Description

The `Snap Object` condition is fulfilled once the specified *Object to snap* is being snapped into the *Zone to snap into* (i.e. the game object is released within the snap zone) while this condition is active.

### Configuration**

- **Object to snap**\
    This field contains the `Training Scene Object` that is required to be snapped. Make sure you added the `Snappable Property` component to the game object in the Unity Inspector. Besides, the object must have a collider component in order to be grabbed and snapped.

    [![Snappable Properties](../images/conditions/snappable_properties.png "")](../images/conditions/snappable_properties.png)

- **Zone to snap into**\
    This field contains the `Training Scene Object` where the *Object to snap* is required to be snapped. Make sure you added the `Snap Zone Property` component to the snap zone game object in the Unity Inspector. Besides, the object must have a collider component with the `Is Trigger` property *enabled*.

    [![Snap Zone Properties](../images/conditions/snap_zone_properties.png "")](../images/conditions/snap_zone_properties.png)

> Learn more about the [setup of snap zones](../miscellaneous/snapzones.md).

------

## Timeout

### Description

The `Timeout` condition is fulfilled once the specified time is elapsed while this condition is active.

### Configuration

- **Wait for seconds**\
    In this field, you can set the time in seconds that should elapse before this condition is fulfilled.

------

## Touch Object

### Description

The `Touch Object` condition is fulfilled when the *Touchable object* is touched by the controller while this condition is active. It is fulfilled immediately if the condition becomes active while the *Touchable object* is being touched.

### Configuration

- **Touchable object**\
    This field contains the `Training Scene Object` that is required to be touched. Make sure you added the `Touchable Property` component to the target game object in the Unity Inspector. Besides, the object must have a collider component in order to be touchable.

    [![Touchable Properties](../images/conditions/touchable_properties.png "")](../images/conditions/touchable_properties.png)

------

## Use Object

### Description

The `Use Object` condition is fulfilled as the *Usable object* is used by pressing the `Use` button of the controller while being touched or grabbed when the condition is active.

### Configuration

- **Usable object**\
    This field contains the `Training Scene Object` that is required to be used. Make sure you added the `Usable Property` component to the target game object in the Unity Inspector. Besides, the object must have a collider component, as it needs to be touched or grabbed.

    [![Usable Properties](../images/conditions/usable_properties.png "")](../images/conditions/usable_properties.png)
