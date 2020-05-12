# Unity XR FAQ

Unity XR Interactions is a brand new VR framework. Starting from the v2.0, this is the single VR framework that we support, as we expect it to become the standard solution. We compiled our own experience with it into this chapter in a list of questions and answers.

## Project Setup

> Q: Which headsets does Unity XR support?

A: See the Unity Technologies's [blog post](https://blogs.unity3d.com/2020/01/24/unity-xr-platform-updates/) to get the answer from first hands.

> Q: Where can I find the documentation for Unity XR Interactions?

A: You can find it [here](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@0.9/manual/index.html).

> Q: How do I use it with an Oculus headset?

A: Go to `Unity > Edit > Project Settings... > XR Plugin Management`. Press `Install XR Plugin Management`. Let Unity import assets and click `Install Oculus XR Plugin`.

> Q: How do I use it with Windows Mixed Reality?

A: Go to `Unity > Edit > Project Settings... > XR Plugin Management`. Press `Install XR Plugin Management`. Let Unity import assets and click `Install Windows XR Plugin`.

> Q: How do I use it with an HTC Vive, Valve Index, or similar headsets?

A: There is no SteamVR/OpenVR XR Plugin yet. Unity Technologies works on it and will release it soon. 

For now, go to `Unity > Edit > Project Settings... > Player > XR Settings > Deprecated Settings`. Toggle `Enable Virtual Reality Supported`. Let Unity import assets. Click on the `+` button under the `Virtual Reality SDKs` and add OpenVR to the list.

> Q: Why does it not work after I have changed the build target?

A: Unity uses separate sets of player settings for each platform. Just repeat steps from the answers above for the current build target. You have to do it only once per target platform.

## Scene Setup

> Q: Why am I flying over the ground?

A: Set the `Tracking Origin Mode` to `Floor` in the `XR Rig Inspector` component of the `[XR_Setup]` game object.

> Q: Why cannot I teleport onto a plane?

A: Add the `Teleportation Area` or the `Teleportation Anchor` component to the plane game object.

> Q: Why I cannot see an object highlight in the scene?

A: In Unity XR, you can see it only through VR headset when the application is running.

## Snap Zones

> Q: How to create a snap zone?

A: Add a `Snapzone Property` component to a game object, a collider, and set the collider's `Is Trigger` field to `true`.

> Q: Why my snapped object has a wrong rotation and position?

A: Snap zone resets local position and rotation of a snapped object and its children. This is a Unity XR bug and Unity Technologies is working on it.

> Q: Can my Snap Zone have interactable children?

A: Yes, as long as they have different interaction layer. Alternatively, you could disable all interactable children before snapping an object. You can do it with Enable and Disable behaviors that we have included in the release package.

> Q: How do I set up a highlight for a snap Zone?

A: Given that you have a game object or a prefab that should serve as a highlight, drag it to the `Shown Highlight Object` field of the `Snap Zone` component.