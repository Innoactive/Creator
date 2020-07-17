# Snap Zones

A snap zone is a pre-defined area determined by a trigger collider where the trainee can "snap" an interactable object by releasing it within that zone. 

[![Snap Zone](../images/snap-zones/snapzone-in-action.gif "Snap Zone")](../images/snap-zones/snapzone-in-action.gif)

### Snap Zone Configuration

By default, the trainee cannot see snap zones. However, they are visible for the training designer in the editor window as a volume. 

[![Snap Zone Configuration](../images/snap-zones/snapzone.png "Snap Zone Configuration")](../images/snap-zones/snapzone.png)

#### Snap Zone Preview

You can configure the snap zone to show a preview version (playmode only) of the desired snap object as follows:

- **Shown Highlight Object**: A prefab that contains only the meshes of the preview object.
- **Shown Highlight Object Color**: The color that the preview object will have.

[![Snap Zone Preview](../images/snap-zones/snap-zone-preview.png "Snap Zone Preview")](../images/snapzones/snap-zone-preview.png)

#### Snap Zone Validation Preview

Additionaly, you can configure a snap zone to provide visual feedback to the trainee when hovering an object near the snap zone, it can be configured as follows:

- **Validation Hover Material**: The material that the preview object will have when a valid snappable object interacts with the snap zone.

[![Snap Zone Validation Preview](../images/snap-zones/snap-zone-validation-preview.png "Snap Zone Validation Preview")](../images/snap-zones/snap-zone-validation-preview.png)

You can create a snap zone either [automatically](#automatically-setup-a-simple-snap-zone) (recommended) or [manually](#manually-setup-a-simple-snap-zone).

------

## Automatically setup a simple snap zone

### I. Snappable game object

We need a game object that we want to snap into the snap zone. It needs to have at least one collider component. To make it snappable, we need to add the `Snappable Property` component.

[![Add Snappable Property](../images/snap-zones/box-snappable.png "Add Snappable Property")](../images/snap-zones/box-snappable.png)


### II. Create snap zone automatically

To automatically create a matching snap zone, click the `Create Snap Zone` button of the `Snappable Property` in the `Inspector` window of the corresponding snappable game object.

[![Create Snap Zone](../images/snap-zones/automatically-create.gif "Create Snap Zone")](../images/snap-zones/automatically-create.gif)

> The snap zone is generated in the same position, rotation, and with the same volume as the snappable property object.


### III. Adjust snap zone (optional)

You can either change the settings directly in the new snap zone object or change the default settings in the `Snap Zone Settings` window. In order to open the default snap zone settings, **go to the Unity's toolbar** and select: `Innoactive` > `Creator` > `Windows` > `Snap Zone Settings`. All changes are saved automatically, and every snap zone that will be created via the `Create Snap Zone` button afterwards will get the new settings by default.

[![Create Snap Zone](../images/snap-zones/change-settings.gif "Create Snap Zone")](../images/snap-zones/change-settings.gif)


------

## Manually setup a simple snap zone

In order to properly set up a snap zone we require 3 game objects:

- [**Snap Zone**](#snap-zone): The object that will act as snap zone.
- [**Snappable Object**](#snappable-object): The object that we want to be snapped into the snap zone.
- [**Preview**](#preview): The object that will be shown as preview object on the snap zone (optional).

> By default, a snap zone can interact with any interactable object, to change this behavior (for example, making a snap zone to only accept one type of object), the [`Interaction Layer Mask`](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@0.9/api/UnityEngine.XR.Interaction.Toolkit.XRBaseInteractor.html#UnityEngine_XR_Interaction_Toolkit_XRBaseInteractor_InteractionLayerMask) must be changed in both, the snap zone and interactable object. 

### Snap zone

1. We need to create an empty game object. To do this, right click into the *Unity Scene Hierarchy* and select `Create Empty`.
2. This empty game object requires a collider component the size that the snap zone should ultimately have. The `Is Trigger` property must be **enabled**. 
3. It needs the `Snap Zone Property` component. To add it, click on the `Add Component` button in the bottom of the *Unity Inspector*. Then type in `Snap Zone Property` and select the appearing entry.

We can now change the volume color of our snap zone by changing the `Shown Highlight Object Color`. 
If we want to show a validation preview of the object hovering the snap zone we can make sure the `Show Interactable Hover Meshes` option is enabled. Additionally, we can also customize the appearance of the validation preview by adding a material asset to the `Interactable Hover Mesh Material`.

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
    > Learn more about [Prefabs](https://docs.unity3d.com/Manual/Prefabs.html)
3. Now we can add this prefab to the `Show Highlight Object` property in the `Snap Zone` component of our snap zone game object. To do this, we drag & drop the newly created prefab from the *Unity Project* tab into the *Unity Inspector* tab of our snap zone.

Finally, we should delete the duplicated object from our scene. Since we created a prefab from it, it is no longer needed.

[![Create Highlight Prefab](../images/snap-zones/create-highlight-prefab.gif "Create Highlight Prefab")](../images/snap-zones/create-highlight-prefab.gif)
