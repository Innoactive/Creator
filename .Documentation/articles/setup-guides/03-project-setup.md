# Project Setup

## XR Setup

You need to add an SDK package for every HMD you want to use with your application.

### XR Plugin Management

If you use an Oculus headset, go to `Unity > Edit > Project Settings... > XR Plug-in Management`. Press `Install XR Plug-in Management`. Let Unity import assets and click `Install Oculus XR Plugin`.

> Check out our [community](https://innoactive.io/creator/community) for a detailed explanation on [how to configure the project for using Oculus Quest and Oculus Quest 2](https://spectrum.chat/innoactive-creator/general/standalone-devices-oculus-quest-2~6d7dbafd-02b8-4340-b752-4caea2c66113).

If you use Windows Mixed Reality, go to `Unity > Edit > Project Settings... > XR Plug-in Management`. Press `Install XR Plug-in Management`. Let Unity import assets and click `Install Windows XR Plugin`.

![XR Plugin Management](../images/xr-plugin-management.png "XR Plug-in Management settings window")

### OpenVR (legacy)

If you use an HTC Vive, Valve Index, or similar headsets, then there is no SteamVR/OpenVR XR Plugin yet. Unity Technologies works on it and will release it soon. For now, go to `Unity > Edit > Project Settings... > Player > XR Settings > Deprecated Settings`. Toggle `Enable Virtual Reality Supported`. Let Unity import assets. Click on the `+` button under the `Virtual Reality SDKs` and add `OpenVR` to the list.

![XR Deprecated Settings](../images/xr-deprecated.png "XR Settings window (deprecated)")

> This configuration is deprecated and only available in Unity 2019 LTS

## Input System

The Innoactive Creator is compatible with the `legacy input system` and the `new input system`, but certain Unity features might work differently according to the chosen configuration.

The active input system can be adjusted as follows:

1. Open **Edit** > **Project Settings**
2. Select the **Player** tab in the opening window. 
3. Open the **Other Settings** section and scroll down to **Active Input Handling**.
4. Set it to your desired input system.

![XR Deprecated Settings](../images/project-setup/active-input-handling-setting.png "XR Settings window (deprecated)")

>  If `both` is selected, the `legacy input system` and the `new input system` will be active.