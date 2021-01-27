# XRI Simulator

The Unity's XRI Simulator comes in handy when you want to test your VR Application without headset.
The XRI Simulator requires to use of the latest XR Interaction Toolkit version along with the new input system. Currently, the Creator is not compatible.
Innoactive Creator automatically downloads and enables the XR Interaction Toolkit.


## Step 1: Configure the use of the 'new input system' for your Project
The XRI Simulator uses the Unity Input System to drive VR interactions. Make sure that the new input system is configured in your project.

> Open _Edit > Project Settings_ 

> Select the _Player_ tab in the opening window. 

> Scroll down to _Active Input Handling_ and set it to either `both` or `new input system`.


## Step 2: Check if you have required packages
The Simulator uses a default set of input actions and presets used with the XR Interaction Toolkit behaviors that use the new Input System. It also delivers Assets related to the simulation of XR HMD and controllers. Both needs to be imported.

> Open the Package Manager at Window > Package Manager 

> Select 'in Project' in the drop-down on the top. 

> Select the XR Interaction Toolkit and scroll the tab window down. Make sure that both `Default Input Actions` and `XR Device Simulator` are imported. 

> Check also if the folder _Assets/Samples_ exists in the Unity Project tab.

## Step 3: Switch XR Rig to XR Rig action-based


## Helpful Links:
[XR Device Simulator Controls](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@0.10/manual/samples.html#input-actions-asset-1)
[How To add XR Simulator](https://www.youtube.com/watch?v=d4bTpkvBwrs&feature=youtu.be)


