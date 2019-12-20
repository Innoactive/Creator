# Prerequisites

The `Innoactive Creator` needs the following setup to work correctly:

* One of the following officially supported **Unity** versions
    * **Unity 2017.4**
    * **Unity 2018.4**

    > We do not guarantee stability with other Unity versions and recommend using Unity 2018.4 LTS.

* Set **.Net API compatibility level** in **Unity**

    You can choose the .NET profile for your player build via the `API Compatibility Level` option in the [Player Settings](https://docs.unity3d.com/Manual/class-PlayerSettings.html).

    > A .Net profile defines the API surface a code can use for the .NET class libraries.

    Unity [upgraded](https://blogs.unity3d.com/2018/03/28/updated-scripting-runtime-in-unity-2018-1-what-does-the-future-hold/) the supported API compatible level between the `2017` and `2018` versions. It means that there are two potential configurations for the `Innoactive Creator`:

    If you use **Unity 2017.4**, set the .Net API compatibility level to **.NET 2.0**.  
    If you use **Unity 2018.4**, set the .Net API compatibility level to **.NET 4.X**.

    > Learn more about the [.NET API compatibility level and how to change it](unity-setup.md#api-compatibility-level).

* A specific SDK depending on the used HMD:
    * If you use the **HTC Vive**, import **SteamVR SDK** in your Unity project separately.
    * If you use an **Oculus** HMD, import the **Oculus SDK** in your Unity project.

* **Windows 10** (For **Microsoft Text to Speech (TTS)** service)
* For every language that you want to use with `TTS`, install the appropriate `Windows 10 Language Package` (`Windows Settings` > `Time & Language` > `Language`).

>Learn more about [TTS](setup-text-to-speech.md)
