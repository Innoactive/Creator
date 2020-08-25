# Project Setup

## VR Headset SDK

You need to add an SDK package for every VR headset you want to use with your application.

If you use an Oculus headset, go to `Unity > Edit > Project Settings... > XR Plug-in Management`. Press `Install XR Plug-in Management`. Let Unity import assets and click `Install Oculus XR Plugin`.

If you use Windows Mixed Reality, go to `Unity > Edit > Project Settings... > XR Plug-in Management`. Press `Install XR Plug-in Management`. Let Unity import assets and click `Install Windows XR Plugin`.

![XR Plugin Management](../images/xr-plugin-management.png "XR Plug-in Management settings window")

If you use an HTC Vive, Valve Index, or similar headsets, then there is no SteamVR/OpenVR XR Plugin yet. Unity Technologies works on it and will release it soon. For now, go to `Unity > Edit > Project Settings... > Player > XR Settings > Deprecated Settings`. Toggle `Enable Virtual Reality Supported`. Let Unity import assets. Click on the `+` button under the `Virtual Reality SDKs` and add `OpenVR` to the list.

![XR Deprecated Settings](../images/xr-deprecated.png "XR Settings window (deprecated)")

