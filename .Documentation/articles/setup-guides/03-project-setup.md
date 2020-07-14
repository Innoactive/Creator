# Project Setup

## API Compatibility Level

The Innoactive Creator requires `.Net API compatibility level` to be set to `.NET 4.X`. By default, Unity sets it to `.NET 2.0 Standard`. You will see the following error in the Unity Editor console:

![.Net API compatibility level error](../images/unity-setup/net-api-level-error.png "An error message about the incompatible .Net API level")

You can fix it through the `Player Settings` panel. In the Unity Editor, select `Edit` > `Project Settings` > `Player Settings` option in the menubar at top. The `Player Settings` panel contains up to 6 different sections. Find `Other Settings` at the bottom. Inside that section, look up for the `Api Compatibility Level*` dropdown switch. Change it to `.NET 4.X`.

![Player Settings Panel](../images/unity-setup/player-settings-other-api-level.png "Api Compatibility Level - .Net 4.x")

You can learn more about the `Player Settings` panel in the Unity Editor's [documentation](https://docs.unity3d.com/Manual/class-PlayerSettings.html).

## VR Headset SDK

You need to add an SDK package for every VR headset you want to use with your application.

If you use an Oculus headset, go to `Unity > Edit > Project Settings... > XR Plug-in Management`. Press `Install XR Plug-in Management`. Let Unity import assets and click `Install Oculus XR Plugin`.

If you use Windows Mixed Reality, go to `Unity > Edit > Project Settings... > XR Plug-in Management`. Press `Install XR Plug-in Management`. Let Unity import assets and click `Install Windows XR Plugin`.

![XR Plugin Management](../images/xr-plugin-management.png "XR Plug-in Management settings window")

If you use an HTC Vive, Valve Index, or similar headsets, then there is no SteamVR/OpenVR XR Plugin yet. Unity Technologies works on it and will release it soon. For now, go to `Unity > Edit > Project Settings... > Player > XR Settings > Deprecated Settings`. Toggle `Enable Virtual Reality Supported`. Let Unity import assets. Click on the `+` button under the `Virtual Reality SDKs` and add `OpenVR` to the list.

![XR Deprecated Settings](../images/xr-deprecated.png "XR Settings window (deprecated)")

