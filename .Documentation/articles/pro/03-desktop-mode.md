# Desktop Mode

**Note:** Desktop Mode is currently in preview and might have feature as well as code rework until it is officially released. 

The desktop mode is part of the Innoactive Creator Pro and allows you to run your training applications without the need of a VR headset. You can perform the training using only a regular desktop PC or laptop. The training course created in Innoactive Creator does not need to be adapted to run in desktop mode. Just build your training application for both VR and non-VR and you can switch even during runtime between a training experience in VR and without an HMD.

## Setup in Unity

Define if you want to start in desktop mode or in VR when launching your training application. You have to configure two game objects in your training scene.

First the **[COURSE_CONTROLLER]**:

[![Course Controller](../images/pro/03-course-controller.png "Course controller settings.")](../images/pro/03-course-controller.png)

When using the _desktop mode_ you have to make sure that _Desktop_ is chosen in the _Course Controller Setup_. For VR (and the possibility to switch to Desktop during runtime) choose _Advanced_. You do not have to make any changes on any other component on the _Course Controller_ game object, everything will be automatically configured.

Second the **[INTERACTION_RIG_LOADER]**:

[![Interaction Rig](../images/pro/03-rig-loader.png "Interaction rig settings.")](../images/pro/03-rig-loader.png)

Moving the _Novice Desktop Mode_ rig to the top will ensure that the desktop mode (camera and interactions) will be loaded. Choosing any XR rig (_XR Rig_ is recommended) will load a VR configured camera rig and controllers as soon as the application starts.

_Note:_ For testing in Unity you can switch between settings, but before you build your final application make sure you have the desired starting configuration set in both objects.

## How to use the _novice_ Desktop Mode

The novice Desktop mode is targeted on 'conservative' users, not used to playing computer games or to any advanced interaction with keyboard and mouse. We intentionally reduced the freedom of control to reduce complexity and increased the usability by simplifying the interaction. In this section, we will describe how to navigate and interact when using the _novice_ desktop mode.

### Moving around and rotating view

The _novice_ desktop mode does not support moving around your training scene (locomotion) but to rotate your viewport around your current position.
In order to rotate your viewport, simply click anywhere in your scene, keep the mouse button pressed and drag your mouse to any direction. 

[![Looking around](../images/pro/03-mouse-viewport-control.gif "Looking around in the scene.")](../images/pro/03-mouse-viewport-control.gif)

_Note:_ As a developer you can choose the mouse button in the _Mouse Controller_ of the _Desktop Mode Rig_ prefab (e.g. _[NOVICE_DESKTOP_MODE_RIG]_).

[![Mouse Controller](../images/pro/03-mouse-controller.png "Mouse controller settings.")](../images/pro/03-mouse-controller.png)

### Interacting with objects

In _novice_ desktop mode, users interact via left mouse button. Moving the mouse pointer over interactable objects will highlight them to communicate a possible interaction with such objects.

[![Selecting objects](../images/pro/03-mouse-over-highlighting.gif "Selecting objects.")](../images/pro/03-mouse-over-highlighting.gif)

Clicking an interactable object triggers the interaction. Depending on the current training course step, the configured interaction will automatically be completed. Therefore, if in your current step you have to grab a sphere _or_ touch a cube, clicking on the sphere will actually grab it and touching the cube will trigger "touching". As said before, this very simplified interaction system is a balance between functionality and usability with a weight on usability.

Complex interaction such as [_snap object_](../innoactive-creator/default-conditions.md#snap-object) are divided into two interaction steps: clicking on the respective object to snap will pick the object up and place it into an on-screen hand that represents the trainee's hand holding an object. A textual instruction now informs you to select the snap zone. By selecting the snap zone, the object is dropped or placed from your on-screen hand to the desired position. You can also interact with objects while they are inside your hand, like using or touching it, by clicking the respective action buttons appearing next to the hand. 

[![Inventory](../images/pro/03_inventory.png "Hand field and additional hints.")](../images/pro/03_inventory.png)

### Switching between modes

You can switch between VR-mode and desktop mode through the trainer menu on the bottom of your screen. Switching the mode will reload your current scene and switching is not possible while a training course is running.

[![Switching modes](../images/pro/03-trainer-menu.png "Switching between VR and Desktop Mode.")](../images/pro/03-trainer-menu.png)

## Advanced Mode

An advanced desktop mode with more features and more possibilities to interact with your scene is coming in a future release.

## Extending the Desktop Mode

A detailed documentation on how to extend the desktop mode will come soon.
