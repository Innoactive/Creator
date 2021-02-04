# XR Device Simulator

The [XR Device Simulator](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@0.10/manual/samples.html#xr-device-simulator) is a tool bundled within `XR Interaction Toolkit` starting from its version `1.0.0-pre.2`. It handles mouse and keyboard input from the user and uses it to drive simulated XR controllers and an XR head-mounted display (HMD).

The XR Device Simulator is only supported when using [Actions](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/Actions.html), for which the [new input system](https://blogs.unity3d.com/2019/10/14/introducing-the-new-input-system/) is required.

[![Open the Step Inspector](../images/xr/xr-simulator.gif "XR Device Simulator.")](../images/xr/xr-simulator.gif)

## Prerequisites

- The `new input system` must be enabled.

> [How to set up the new input system.](../setup-guides/03-project-setup.md#input-system)

- The `XR Interaction Toolkit`package must be at least at version `1.0.0-pre.2`.

> [How to change a package version.](https://docs.unity3d.com/Manual/upm-ui-update.html)

- Make sure you have imported the `Default Input Actions` and `XR Device Simulator` [samples](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@0.10/manual/samples.html).

![Delete Obsolete Rig](../images/xr/xr-samples.png "XR Interaction Toolkit Samples")


## Initial Setup

If in your scene you are missing the `[INTERACTION_RIG_LOADER]` then you need to [Setup the Training Configuration](../innoactive-creator/training-configuration.md#setup-the-training-configuration-in-the-current-scene), you can do this by selecting in the toolbar `Innoactive` > `Setup Training Scene`.

[![Open the Step Inspector](../images/xr/how-to-set-up-the-xr-simulator.gif "How to set up the XR Device Simulator.")](../images/xr/how-to-set-up-the-xr-simulator.gif)

The `[INTERACTION_RIG_LOADER]` contains a list of available XR rigs, including an `XR Simulator`. The `[INTERACTION_RIG_LOADER]` will spawn the first available rig going from top to bottom of the list, the list can be rearranged.

![Delete Obsolete Rig](../images/xr/interaction-rig-loader.png "A pop-up allows you to delete the obsolete `[XR Setup]` object from the scene")

> In order to try out the simulator, make sure to position the XR Simulator at the top of the list.

## Simulator Controls (Input Actions)

The XR Device Simulator uses a sample [Input Action Set](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@0.10/manual/samples.html#input-actions-asset-1), you can find and [edit](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/ActionAssets.html#editing-input-action-assets) it in the `Assets\Samples\XR Interaction Toolkit\[version]\XR Device Simulator Controls` folder.

The `XR Device Simulator Controls` is divided in two `Action Maps`: 

- **Main**

Actions like `Manipulate Left Hand`, `Manipulate Right Hand`, `Manipulate Head`, and `Toggle Coursor Lock` can be found here.

![Delete Obsolete Rig](../images/xr/xr-simulator-controls-main.png "A pop-up allows you to delete the obsolete `[XR Setup]` object from the scene")

- **Input Controls**

Actions like `Grip`, `Trigger`, `Primary Button`, `Primary Touch`, and `Menu`can be found here.

![Delete Obsolete Rig](../images/xr/xr-simulator-controls-input-controls.png "A pop-up allows you to delete the obsolete `[XR Setup]` object from the scene")

## Disclaimer

The `XR Device Simulator` is a tool developed and maintained by `Unity Technologies`, it is not distributed with the `Innoactive Creator`.

Do you have more questions? Let us know about your experience in our [community](https://innoactive.io/creator/community?tab=posts).