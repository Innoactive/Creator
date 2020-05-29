# Prerequisites

The `Innoactive Creator` needs the following setup to work correctly:

* One of the following officially supported **Unity** versions
    * **Unity 2019.3** or later

    > We do not guarantee stability with other Unity versions and recommend using Unity 2019.3.

* Set **.Net API compatibility level** in **Unity**

    You can choose the .NET profile for your player build via the `API Compatibility Level` option in the [Player Settings](https://docs.unity3d.com/Manual/class-PlayerSettings.html).

    > A .Net profile defines the API surface a code can use for the .NET class libraries.

    Set the .Net API compatibility level to **.NET 4.X**.

    > Learn more about the [.NET API compatibility level and how to change it](unity-setup.md#api-compatibility-level).

* A specific SDK depending on the used HMD:

    If you use an Oculus headset, go to `Unity > Edit > Project Settings... > XR Plug-in Management`. Press `Install XR Plug-in Management`. Let Unity import assets and click `Install Oculus XR Plugin`.

    If you use Windows Mixed Reality, go to `Unity > Edit > Project Settings... > XR Plug-in Management`. Press `Install XR Plug-in Management`. Let Unity import assets and click `Install Windows XR Plugin`.

    ![XR Plugin Management](../images/xr-plugin-management.png "XR Plug-in Management settings window")

    If you use an HTC Vive, Valve Index, or similar headsets, then there is no SteamVR/OpenVR XR Plugin yet. Unity Technologies works on it and will release it soon. For now, go to `Unity > Edit > Project Settings... > Player > XR Settings > Deprecated Settings`. Toggle `Enable Virtual Reality Supported`. Let Unity import assets. Click on the `+` button under the `Virtual Reality SDKs` and add `OpenVR` to the list.

    ![XR Deprecated Settings](../images/xr-deprecated.png "XR Settings window (deprecated)")

* **Windows 10** (For **Microsoft Text to Speech (TTS)** service)
* For every language that you want to use with `TTS`, install the appropriate `Windows 10 Language Package` (`Windows Settings` > `Time & Language` > `Language`).

Learn more about [TTS](setup-text-to-speech.md)
