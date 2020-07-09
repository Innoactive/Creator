# Suspending Interactions

You can let your trainees to interact with a [training object](training-scene-object.md) by adding certain properties to it. By default, objects stay interactive throughout the whole training course: a trainee could push a button and flick a switch at any time. The training feels more real, but it can turn chaotic in complex courses and scenes. To prevent that, you can temporarily disable interactions for a property, and restore it only when your trainee needs it.

To disable user interactions with all properties from the start, attach the `LockObjectsOnSceneStart` component to any active object in the scene. You can suspend user interactions for individual training properties by using the `Lock` behavior. 

A step will automatically restore interactions with all properties that are referenced by its behaviors and conditions. You can find the list of unlocked properties in the `Unlocked Objects` tab of the Step Inspector, and you can manually add other properties to it.