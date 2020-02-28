# Snap Zones

A snap zone is an area defined by a trigger collider where the trainee can "snap" an held object at the pre-defined position by releasing it. 

You can create a snap zone either [automatically](#a-automatically-setup-a-simple-snap-zone) (recommended) or [manually](#b-manually-setup-a-simple-snap-zone).

------

## A) Automatically Setup a Simple Snap Zone

### I. Snappable Game Object

We need a game object that we want to snap into the snap zone. It needs to have at least one collider component. To make it snappable, we need to add the `Snappable Property` component.

[![Add Snappable Property](../images/snapzones/box-snappable.png "Add Snappable Property")](../images/snapzones/box-snappable.png)


### II. Create Snap Zone Automatically

To automatically create a matching snap zone, click the `Create Snap Zone` button of the `Snappable Property` in the `Inspector` window of the corresponding snappable game object.

[![Create Snap Zone](../images/snapzones/automatically-create.gif "Create Snap Zone")](../images/snapzones/automatically-create.gif)


### III. Adjust Snap Zone (Optional)

You can either change the settings directly in the new snap zone object or change the default settings in the `Snap Zone Settings` window. In order to open the default snap zone settings, go to the Unity's toolbar and select: `Innoactive` > `Creator` > `Utilities` > `Snap Zone Settings...`. If you click `Save Settings`, every snap zone that will be created via the `Create Snap Zone` button afterwards will get the new settings by default. `Save and Apply` additionally applies these settings to all snap zones in the current scene.

[![Create Snap Zone](../images/snapzones/change-settings.gif "Create Snap Zone")](../images/snapzones/change-settings.gif)


------

## B) Manually Setup a Simple Snap Zone

We need three game objects to setup a snap zone:


### I. Snap Zone Game Object

1. We need to create an empty game object. To do this, right click into the *Unity Scene Hierarchy* and select `Create Empty`.
2. This empty game object requires a collider component the size that the snap zone should ultimately have. The `Is Trigger` property must be **enabled**. 
3. It needs the `Snap Zone Property` component. To add it, click on the `Add Component` button in the bottom of the *Unity Inspector*. Then type in `Snap Zone Property` and select the appearing entry.

We can now change the `Unique Name` in the `Training Scene Object` component and also change the colors of the highlighted object. The `Valid Highlight Color` is shown when the snappable object is held inside the above added collider. The `Highlight Object Prefab` will be added later.

[![Create Snap Zone Manually](../images/snapzones/manually-create-snapzone.gif "Create Snap Zone Manually")](../images/snapzones/manually-create-snapzone.gif)


### II. Snappable Game Object

1. We need a game object that we want to snap into the snap zone. In this example, we create a cube by right clicking into the *Unity Scene Hierarchy* and select `3D Object > Cube`.
2. Make sure that the game object has a collider component.
3. To make the game object snappable, we need to add the `Snappable Property` component.

We can now change the `Unique Name` in the `Training Scene Object` component.

[![Create Snappable Cube](../images/snapzones/create-snappable-cube.gif "Create Snappable Cube")](../images/snapzones/create-snappable-cube.gif)


### III. Highlight Prefab of the Snappable Game Object

For our Highlight prefab we probably want to use a model of our snappable object as a prefab. If we don't have a model prefab, we can simply create one by following these steps:

1. To create a highlight model of our snappable game object, we can duplicate it by right clicking on it and select `Duplicate`. After duplicating, we delete every component but the `Transform`, `Mesh Filter`, and `Mesh Renderer` in the copy. We also rename the newly created game object to be able to easily find it.
2. To create the actual prefab, we simply drag & drop the game object from the *Unity Scene Hierarchy* tab into the desired opened folder of the *Unity Project* tab.
    > Learn more about [Prefabs](https://docs.unity3d.com/Manual/Prefabs.html)
3. Now we can add this prefab to the `Highlight Object Prefab` property in the `Snap Drop Zone` component of our snap zone game object. To do this, we drag & drop the newly created prefab from the *Unity Project* tab into the *Unity Inspector* tab of our snap zone game object.

Finally, we should delete the duplicated object from our scene. Since we created a prefab from it, it's no longer necessary.

[![Create Highlight Prefab](../images/snapzones/create-highlight-prefab.gif "Create Highlight Prefab")](../images/snapzones/create-highlight-prefab.gif)
