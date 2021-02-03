# XR Device Simulator

The [XR Device Simulator](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@0.10/manual/samples.html#xr-device-simulator) is a tool bundled within `XR Interaction Toolkit` starting from its version `1.0.0-pre.2`. It handles mouse and keyboard input from the user and uses it to drive simulated XR controllers and an XR head-mounted display (HMD).

The XR Device Simulator is only supported when using [Actions](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/Actions.html), for which the [new input system](https://blogs.unity3d.com/2019/10/14/introducing-the-new-input-system/) is required.

## Prerequisites

- XR Interaction Toolkit must be at least at version 1.0.0-pre.2.

> [How to change a package version.](https://docs.unity3d.com/Manual/upm-ui-update.html)

- The new input system must be enabled.

> [How to set up the new input system.](../setup-guides/03-project-setup.md#input-system)

If missing, import the...

## How to something-...


[![Open the Step Inspector](../images/xr/how-to-set-up-the-xr-simulator.gif "How to set up the XR Device Simulator.")](../images/xr/how-to-set-up-the-xr-simulator.gif)

[![Open the Step Inspector](../images/xr/xr-simulator.gif "XR Device Simulator.")](../images/xr/xr-simulator.gif)

![Delete Obsolete Rig](../images/xr/interaction-rig-loader.png "A pop-up allows you to delete the obsolete `[XR Setup]` object from the scene")

## Step 2: Check if you have required packages

The XR Device Simulator uses a default set of input actions and presets used with the XR Interaction Toolkit behaviors that use the new Input System. It also delivers Assets related to the simulation of XR HMD and controllers. Both needs to be imported.

> Open the Package Manager at Window > Package Manager 

> Select 'in Project' in the drop-down on the top. 

> Select the XR Interaction Toolkit and scroll the tab window down. Make sure that both `Default Input Actions` and `XR Device Simulator` are imported. 

> Check also if the folder _Assets/Samples_ exists in the Unity Project tab.

## Step 3: Change from XR Rig to XR Rig action-based

> Select 'Setup Training Scene' from the Innoactive Menu drop-down.

If your scene has the obsolete `[XR Setup]` object in the scene, a pop-up window allows you to delete it. You will not need it anymore.

![Delete Obsolete Rig](../images/xr/dialogXRRigDeletion.png "A pop-up allows you to delete the obsolete `[XR Setup]` object from the scene")

> delete the `[XR Setup]` object if it is in your object hierachy.

Check the object hierachy tab in unity. You should see an `[INTERACTION_RIG_LOADER]` and a `[TRAINEE]` object.

## Step 4: check the available Rigs
>Select the `[INTERACTION_RIG_LOADER]`.

Check the Unity Inspector. There should be an 'Interaction Rig Setup' script, which contains a list with available interaction Rigs. 
- XR Rig Legacy for the legacy input
- XR Rig for the new input system
- none in case you want to use your custom XR Rig already on the scene.

The script is 'smart'. In case Unity is setup to the new input system the list disables the incompatible rigs and only shows the compatible XR Rig.

![Available Rigs](../images/xr/availableRigs.jpg "The Interaction Rig Setup script shows which Rigs are currently configured and available.")

Enable/Disable available Rigs, you are also able to prioritize them by changing the position in the array. The topmost entry has the highest priority. The interaction rig will be spawned at the `[TRAINEE]` GameObject.

If you configured Unity's Active Input Handling in Step 1 to `both`, you will see both Rigs, XR legacy Rig and XR Rig, enabled.

## Step 5: Import Required Objects to Scene


> In Unity's project tab, search for 'XR Setup Action Based' and 'XR Device Simulator'. 

> Drag both files into the hiearachy.

## Step 6: Run XR Device Simulator / change back to VR

> Select thee Unity Play button.

You can run your training now in the XR Device Simulator. See section 'Helpful Links' for more information on how to control the scene with your keyboard and mouse.

If you want to switch back to running your training in VR, you need to delete the two objects you added in Step 5 from your object hierachy.

## Helpful Links:
[XR Device Simulator Controls](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@0.10/manual/samples.html#input-actions-asset-1)


