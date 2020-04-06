# Unity XR FAQ

This chapter is a collection of questions that we had while we were exploring Unity XR. We share our findings in hope that they will help you, too.

*Q: Where can I find the documentation for Unity XR Interactions?*

A: You can find it [here](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@0.9/manual/index.html).

*Q: How do I use it with an Oculus headset?*

Go to `Unity > Edit > Project Settings... > XR Plugin Management`. Press `Install XR Plugin Management`. Let Unity import assets and click `Install Oculus XR Plugin`.

*Q: How do I use it with Windows Mixed Reality?*

Go to `Unity > Edit > Project Settings... > XR Plugin Management`. Press `Install XR Plugin Management`. Let Unity import assets and click `Install Windows XR Plugin`.

*Q: How do I use it with an HTC Vive, Valve Index, or similar headsets?*

There is no SteamVR/OpenVR XR Plugin yet. Unity Technologies works on it and will release it soon. 

For now, go to `Unity > Edit > Project Settings... > Player > XR Settings > Deprecated Settings`. Toggle `Enable Virtual Reality Supported`. Let Unity import assets. Click on the `+` button under the `Virtual Reality SDKs` and add OpenVR to the list.

*Q: Why am I flying over the ground?*

A: Set the `Tracking Origin Mode` to `Floor` in the `XR Rig Inspector` component of the `[XR_Setup]` game object.

*Q: Why cannot I teleport on a plane?*

A: Add the `Teleportation Area` or the `Teleportation Anchor` component to the plane game object.

*Q: Where is the Highlight behavior?*

A: We will implement it after the release of the Innoactive Creator v2.0.

*Q: How to create a snap zone?*

A: Add a `Snapzone Property` component to a game object, as well as a collider with `Is Trigger` field set to true.

*Q: How to set up a highlight for a snap Zone?*

A: Given that you have a game object or a prefab that will serve as a highlight, drag it to the `Shown Highlight Object` field of the `Snap Zone` component.

*Q: Why I cannot see a highlight in the scene?*

A: In Unity XR, you can see it only through VR headset when the application is running.

*Q: Why an object has a wrong rotation and position while snapped?*

A: Snap zone resets local position and rotation of a snapped object and its children. This is a Unity XR bug and Unity Technologies is working on it.

*Q: Can my Snap Zone have interactable children?*

A: Yes, as long as they have different interaction layer. Alternatively, you could disable all interactable children before snapping an object. You can do it with Enable and Disable behaviors.
