# Snap Zones

A snap zone is an area defined by a trigger collider where the trainee can "snap" an held object at the pre-defined position by releasing it. 


## How to setup a simple snap zone

We need three game objects to setup a snap zone:


### 1. Snap zone game object

First, we need to create an empty game object. To do this, right click into the *Unity Scene Hierarchy* and select `Create Empty`.

[<img src="../images/snapzones/create-empty.png" alt="Create Empty" height="324">)](../images/snapzones/create-empty.png)

Second, this empty game object requires a collider component the size that the snap zone should ultimately have. The `Is Trigger` property must be **enabled**. 

[<img src="../images/snapzones/collider.png" alt="Add Collider" height="324">)](../images/snapzones/collider.png)

Third, it needs the `Snap Zone Property` component. To add it, click on the `Add Component` button in the bottom of the *Unity Inspector*. Then type in `Snap Zone Property` and select the appearing entry.

[<img src="../images/snapzones/add-property.png" alt="Add Snap Zone Property" height="324">)](../images/snapzones/add-property.png)

We can now change the `Unique Name` in the `Training Scene Object` component and also change the colors of the highlighted object. The `Valid Highlight Color` is shown when the snappable object is held inside the above added collider. The `Highlight Object Prefab` will be added later.

[<img src="../images/snapzones/change-zone-settings.png" alt="Change Snap Zone Settings" height="324">)](../images/snapzones/change-zone-settings.png)


### 2. Snappable game object

First, we need a game object that we want to snap into the snap zone. In this example, we create a cube by right clicking into the *Unity Scene Hierarchy* and select `3D Object > Cube`.

[<img src="../images/snapzones/create-cube.png" alt="Create Cube" height="324">)](../images/snapzones/create-cube.png)

Second, make sure that the game object has a collider component with the `Is Trigger` property being **disabled**.

[<img src="../images/snapzones/cube-collider.png" alt="Cube Collider" height="324">)](../images/snapzones/cube-collider.png)

Third, to make the game object snappable, we need to add the `Snappable Property` component.

[<img src="../images/snapzones/add-snappable-property.png" alt="Add Snappable Property" height="324">)](../images/snapzones/add-snappable-property.png)

We can now change the `Unique Name` in the `Training Scene Object` component.

[<img src="../images/snapzones/change-snappable-settings.png" alt="Change Snappable Settings" height="324">)](../images/snapzones/change-snappable-settings.png)


### 3. Highlight prefab of the snappable game object

For our Highlight prefab we probably want to use a model of our snappable object as a prefab. If we don't have a model prefab, we can simply create one by following these steps:

To create a highlight model of our snappable game object, we can duplicate it by right clicking on it and select `Duplicate`. After duplicating, we delete every component but the `Transform`, `Mesh Filter`, and `Mesh Renderer` in the copy. We also rename the newly created game object to be able to easily find it.

[<img src="../images/snapzones/duplicate-snappable.png" alt="Create Snappable Highlight Model" height="324">](../images/snapzones/duplicate-snappable.png)

To create the actual prefab, we simply drag & drop the game object from the *Unity Scene Hierarchy* tab into the desired opened folder of the *Unity Project* tab.

> Learn more about [Prefabs](https://docs.unity3d.com/Manual/Prefabs.html)

[<img src="../images/snapzones/snappable-prefab.png" alt="Create Snappable Prefab" height="324">](../images/snapzones/snappable-prefab.png)

Now we can add this prefab to the `Highlight Object Prefab` property in the `Snap Drop Zone` component of our snap zone game object. To do this, we drag & drop the newly created prefab from the *Unity Project* tab into the *Unity Inspector* tab of our snap zone game object.

[<img src="../images/snapzones/set-highlight-object.png" alt="Set Highlight Object Prefab" height="324">](../images/snapzones/set-highlight-object.png)

Finally, we should delete the duplicated object from our scene. Since we created a prefab from it, it's no longer necessary.

[<img src="../images/snapzones/delete-duplicate.png" alt="Delete Duplicate" height="324">](../images/snapzones/delete-duplicate.png)
