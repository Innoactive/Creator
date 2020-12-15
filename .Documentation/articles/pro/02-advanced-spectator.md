# Advanced Spectator

The advanced spectator is an improved version of the standard spectator. It extends the spectator with a smoother movement of the camera when watching the trainee carry out the training. Additionally, you can add view points within the scene and observe the trainee from a third person perspective.

## Activate the advanced spectator

To make use of the improved spectator feature you simply have to choose the _Advanced_ course controller in the _[COURSE_CONTROLLER_SETUP]_ object in your Unity training scene.

[![Set advanced spectator](../images/pro/advanced-spectator.png "Set advanced spectator.")](../images/pro/advanced-spectator.png)

## Add additional cameras

You can add additional cameras to the scene which will function as view points to observe the trainee from a non-first-person view. In the _[COURSE_CONTROLLER_SETUP]_ you find the _Advanced Spectator Controller_ which has a utility button to spawn a new _Spectator Camera_ in the scene. You can change its view point name in the _Spectator Camera Dummy_ (e.g. "Top View", "Above work station" etc.) which will be shown during runtime. Do not worry about the _Camera_ component on the game object as it will be removed when your training application starts.

[![Add spectator camera](../images/pro/advanced-spectator-add-camera.png "Add a spectator camera.")](../images/pro/advanced-spectator-add-camera.png)

## How to control the spectator

With the improved spectator more functionality is added which you can control through your keyboard and which can be set in the _Project Settings_.

[![Spectator settings](../images/pro/advanced-spectator-settings.png "Spectator settings.")](../images/pro/advanced-spectator-settings.png)
