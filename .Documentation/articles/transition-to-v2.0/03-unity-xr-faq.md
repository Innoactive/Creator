# Unity XR FAQ

This chapter is a collection of questions that we had while we were exploring Unity XR. We share our findings in hope that they will help you, too.

*Q: Why am I flying over the ground?*

A: Set the `Tracking Origin Mode` to `Floor` in the `XR Rig Inspector` component of the `[XR_Setup]` game object.

*Q: Why cannot I teleport on a plane?*

A: Add the `Teleportation Area` component to the plane game object.

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

*Q: Can my Snap Zone have children game objects?*

A: Yes, as long as you disable them before snapping another object. You can do it with Enable and Disable behaviors.
