# Training Scene Objects

The `Training Scene Object` is the basic unit of a training course. If an object in the scene has this component, then the training course is aware of it and can interact with it, for example by checking its position. Only game objects with this component can be used in behaviors and conditions. 

The `Unique Name` property of the `Training Scene Object` helps to identify and reference specific game objects. When adding the `Training Scene Object` component to a game object, the `Unique Name` property is by default identical to the game object's current name if no other `Training Scene Object` has the same `Unique Name`.

## Setup a Training Scene Object

1. Pick, add, or create a game object and select it in the scene.
2. In the Unity Inspector, click on the button `Add Component` at the bottom.
3. Type in `Training Scene Object` and select the appearing `Training Scene Object` entry.
4. The component has been added.
5. *(Optional)* Change the `Unique Name` field and enter another unique name.

[<img src="../images/scene-objects/add-component.png" alt="Add Component" width="330">](../images/scene-objects/add-component.png) [<img src="../images/scene-objects/change-unique-name.png" alt="Change Unique Name" width="330">](../images/scene-objects/change-unique-name.png)

## Reference a Training Scene Object

You can reference any game object in your scene by selecting it in the Hierarchy view and dragging it into the field of a behavior or condition.

[<img src="../images/scene-objects/insert-unique-name.png" alt="Insert Unique Name" width="330">](../images/scene-objects/insert-unique-name.png)

> Learn more about the [Hierarchy view](https://docs.unity3d.com/Manual/Hierarchy.html).

You can click on the small button next to the object field. It will open a new window with every game object in the scene. Select an object from this list to assign it to the object field.

[<img src="../images/scene-objects/selecting-from-list.png" alt="Selecting from list" width="330">](../images/scene-objects/selecting-from-list.png)

## Training Scene Object Properties

Properties are components that can be added to a `Training Scene Object` in order to make additional capabilities of the object visible to the training course. For example, a `Grabbable Property` allows the training course to know if the object is being grabbed by the user or not.


Properties also act as an abstraction of the underlying interaction framework: adding the `Grabbable Property` to a game object will  automatically add the necessary components for the object to be grabbed in VR. The training course, however, has no knowledge of what determines if the object has been grabbed or not: it only talks to the property, without having to worry about how things work under the hood.

Naturally, to be able to grab an object you must be able to touch it first. Prerequisite properties are automatically added as well, so in this case a `Touchable Property` will be automatically added to the `Training Scene Object`. If the game object was no `Training Scene Object` before, it will also automatically get this component once you add any property.

Behaviors and conditions often require an object to have one or more specific properties. For example, a `Snap Object` condition will require a `Training Scene Object` with the `Snappable Property` component, and another one with the `Snap Zone Property`.

The Innoactive Creator implements the following default properties:

* `Touchable Property`: The training course is aware of when the object is touched.
* `Grabbable Property`: The training course knows whether the object is being grabbed or not. Requires a `Touchable Property` as well.
* `Usable Property`: The training course knows if the object is being used while grabbed (by pressing the "Use" button on the controller). Requires a `Grabbable Property` as well.
* `Snappable Property`: The training course can check if this object has been snapped to an object with the `Snap Zone Property`. Requires a `Grabbable Property` as well.
* `Snap Zone Property`: The training course knows that objects can be snapped here.
* `Transform In Range Detector Property`: The training course can measure distance of transforms from this object's position. Useful to check the distance between two `Training Scene Objects`.
* `Collider With Trigger Property`: The training course is aware of this object's trigger collider, and can for example verify if another `Training Scene Object` is in it.
* `Highlight Property`: The training course is aware that this object can be highlighted.

More properties can be implemented as necessary by the template developer.

Find more information on how [behaviors](default-behaviors.md) and [conditions](default-conditions.md) use these properties.

## Reference a Training Property Object

Behaviors and conditions reference propreties of objects in the scene. For example, a `Touch Condition` checks if an object was touched, and that object must have a `Touchable Property`.

## Automatic configuration of Scene Objects and Training Properties

If you assign a GameObject without the `Training Scene Object` component to a Scene Object field in the Step Inspector, then the Creator will fail to execute the course. To prevent it, the Step Inspector will display an error message above the Scene Object. If you press the `Fix it` button next to the error, then the Creator will add all necessary components to the target scene object.

The same applies to `Training Properties`.

[![Scene Object reference flow](../images/scene-objects/automatic-property-management.gif "Scene Object reference flow.")](../images/scene-objects/automatic-property-management.gif)
