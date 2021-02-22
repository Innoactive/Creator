# Desktop Mode

The Desktop Mode is part of our Innoactive Creator PRO offer and allows you to run your training applications without the need of a VR headset. You can perform the training using keyboard and mouse on a regular desktop PC. The training course created in Innoactive Creator does not need to be adapted to run in Desktop mode. Just build your training for both VR and non-VR and you can switch even during runtime between a training experience in VR and a training experience using keyboard and mouse.

## Setup in Unity

Define if you want to start in Desktop Mode or in VR when launching your training application. You have to configure two game objects in your training scene.

First the _[COURSE_CONTROLLER]_:

[![Course Controller](../images/pro/03-course-controller.png "Course controller settings.")](../images/pro/03-course-controller.png)

When using the _Desktop Mode_ you have to make sure that _Desktop_ is chosen in the _Course Controller Setup_. For VR (and the possibility to switch to Desktop during runtime) choose _Advanced_. You do not have to make any changes on any other component on the _Course Controller_ game object, everything will be automateically configured.

Second the _[INTERACTION_RIG_LOADER]_:

[![Interaction Rig](../images/pro/03-rig-loader.png "Interaction rig settings.")](../images/pro/03-rig-loader.png)

Moving the _Novice Desktop Mode_ rig to the top will ensure that the desktop mode (camera and interactions) will be loaded. Choosing any XR rig (_XR Rig_ is recommended) will load a VR configured camera rig and controllers as soon as the application starts.

_Note:_ For testing in Unity you can switch between settings, but before you build your final application make sure you have the desired starting configuration set in both objects.

## How to use the _novice_ Desktop Mode

The novice Desktop mode is targeted 'conservative' users, not used to playing computer games or any advanced interaction with keyboard and mouse. We intentionally reduced the freedom of control to reduce complexity and increased the usability by simplifying the interaction. We will soon release the _advanced_ desktop mode providing more control. 
In this section, we will describe how to control a training in _novice_ desktop mode.

### Moving around and rotating view

The _novice_ desktop mode does not support to move around your training scene.
To rotate your viewport, simply click anywhere in your scene, keep the mouse button pressed and move your mouse around. 

<!-- Add a gif here? -->

_Note:_ As a developer you can choose the mouse button in the _Mouse Controller_ of the _Desktop Mode Rig_ prefab (e.g. _[NOVICE_DESKTOP_MODE_RIG]_).

[![Mouse Controller](../images/pro/03-mouse-controller.png "Mouse controller settings.")](../images/pro/03-mouse-controller.png)

### Interacting with objects

### Switching between modes

You can switch between VR-mode and Desktop Mode through the course controller menu on the bottom of your screen. 

## Advanced Mode
An advanced desktop mode with more features and more possibilities to interact with your scene will come soon.

## Extending the Desktop Mode
A detailed documentation on how to extend the desktop mode will come soon.
