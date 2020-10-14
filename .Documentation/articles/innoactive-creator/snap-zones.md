# Snap Zones

A snap zone is an area where the trainee can "snap" an interactable object by releasing it within it. A trigger collider defines its boundaries. Unity physics ignore trigger colliders, but trigger colliders still fire events for scripts. It allows you to put an object inside of it, and that object will set off the snap zone mechanics.

[![Snap Zone](../images/snap-zones/snapzone-in-action.gif "Snap Zone")](../images/snap-zones/snapzone-in-action.gif)

### Snap Zone Configuration

By default, the trainee cannot see snap zones. However, they are visible for the training designer in the editor window as a semi-transparent volume. 

[![Snap Zone Configuration](../images/snap-zones/snapzone.png "Snap Zone Configuration")](../images/snap-zones/snapzone.png)

#### Snap Zone Preview

You can set the following values to make the snap zone show a preview version of the desired snap object:

- **Shown Highlight Object**: A prefab that contains only the meshes of the preview object.
- **Shown Highlight Object Color**: The color that the preview object will have.

[![Snap Zone Preview](../images/snap-zones/snap-zone-preview.png "Snap Zone Preview")](../images/snapzones/snap-zone-preview.png)

You can see this object preview only in the play time, not in the editor time.

#### Snap Zone Validation Preview

You can provide a trainee with visual feedback when he hovers an object near the snap zones:

- **Validation Hover Material**: The material that the preview object will have when a valid snappable object interacts with the snap zone.

[![Snap Zone Validation Preview](../images/snap-zones/snap-zone-validation-preview.png "Snap Zone Validation Preview")](../images/snap-zones/snap-zone-validation-preview.png)

You can create a snap zone either [automatically](#setup-a-simple-snap-zone-automatically) (recommended) or [manually](#manually-setup-a-simple-snap-zone).


## Setup a Simple Snap Zone Automatically

### I. Snappable game object

We need a game object that we want to snap into the snap zone. It should have at least one collider component. To make it snappable, add the `Snappable Property` component to it.

[![Add Snappable Property](../images/snap-zones/box-snappable.png "Add Snappable Property")](../images/snap-zones/box-snappable.png)

### II. Create snap zone automatically

To automatically create a matching snap zone, click the `Create Snap Zone` button of the `Snappable Property` in the `Inspector` window of the corresponding snappable game object. The snap zone is generated in the same position, rotation, and with the same volume as the snappable property object.

[![Create Snap Zone](../images/snap-zones/automatically-create.gif "Create Snap Zone")](../images/snap-zones/automatically-create.gif)



### III. Modify Default Snap Zone Settings (optional)

You can either change the settings directly in the new snap zone object or change the default settings in the `Snap Zone Settings` window. In order to open the default snap zone settings, **go to the Unity's toolbar** and select: `Innoactive` > `Settings` > `Snap Zone Settings`. All changes are saved automatically, and every snap zone that will be created via the `Create Snap Zone` button afterwards will get the new settings by default.

[![Create Snap Zone](../images/snap-zones/change-settings.gif "Create Snap Zone")](../images/snap-zones/change-settings.gif)



## Manually setup a simple snap zone

We need three game objects to set up a snap zone:

- [**Snap Zone**](#snap-zone): The object that will act as snap zone.
- [**Snappable Object**](#snappable-object): The object that we want to be snapped into the snap zone.
- [**Preview**](#preview): The object that will be shown as preview object on the snap zone (optional).

By default, a snap zone interacts with all interactable object. To change this behavior (for example, making a snap zone to only accept one type of object), the [`Interaction Layer Mask`](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@0.9/api/UnityEngine.XR.Interaction.Toolkit.XRBaseInteractor.html#UnityEngine_XR_Interaction_Toolkit_XRBaseInteractor_InteractionLayerMask) must be changed in both, the snap zone and interactable object. 

### Snap zone

1. Create an empty game object. To do this, right click into the *Unity Scene Hierarchy* and select `Create Empty`.
2. This empty game object requires a collider component the size that the snap zone should ultimately have. The `Is Trigger` toggle must be checked. 
3. Add the `Snap Zone Property` component. Click on the `Add Component` button in the bottom of the *Unity Inspector*, then type in `Snap Zone Property` and select the entry.

We can now change the volume color of our snap zone by changing the `Shown Highlight Object Color`. If you want to show a validation preview of the object hovering the snap zone you should enable the `Show Interactable Hover Meshes` option. Additionally, you can also customize the appearance of the validation preview by adding a material asset to the `Interactable Hover Mesh Material`.

[![Create Snap Zone Manually](../images/snap-zones/manually-create-snapzone.gif "Create Snap Zone Manually")](../images/snap-zones/manually-create-snapzone.gif)


### Snappable Object

1. We need a game object that we want to snap into the snap zone. In this example, we create a cube by right clicking into the *Unity Scene Hierarchy* and select `3D Object > Cube`.
2. Make sure that the game object has a collider component.
3. To make the game object snappable, we need to add the `Snappable Property` component.

[![Create Snappable Cube](../images/snap-zones/create-snappable-cube.gif "Create Snappable Cube")](../images/snap-zones/create-snappable-cube.gif)


### Preview

For our preview, we probably want to use a model of our snappable object. If we don't already have a prefab, we can create one by following these steps:

1. To create a preview model of our snappable game object, we can duplicate it by right-clicking on it and selecting `Duplicate`. Afterward, we delete every component but the `Transform`, `Mesh Filter`, `Mesh Renderer`, or `Skinned Mesh Renderer` available in the clone. We also rename the newly created game object to be easily distinguished.
2. To create the actual prefab, we drag & drop the game object from the *Unity Scene Hierarchy* tab into the desired opened folder of the *Unity Project* tab.
Learn more about [prefabs](https://docs.unity3d.com/Manual/Prefabs.html)
3. Now we can add this prefab to the `Show Highlight Object` property in the `Snap Zone` component of our snap zone game object. To do this, we drag & drop the newly created prefab from the *Unity Project* tab into the *Unity Inspector* tab of our snap zone.

Finally, we should delete the duplicated object from our scene. Since we created a prefab from it, it is no longer needed.

[![Create Highlight Prefab](../images/snap-zones/create-highlight-prefab.gif "Create Highlight Prefab")](../images/snap-zones/create-highlight-prefab.gif)
