# Getting Started as an Evaluator

This guide explains how to set up a demo project which displays strengths and features of the Innoactive Creator. After finishing this guide, you could read the [designer's guide](designer.md) to learn how to use the Innoactive Creator in a professional capacity.

## Innoactive Creator Package

The `Innoactive Creator` package is a Unity project of a complete training application built with the `Innoactive Base Template`. It contains [multiple examples](../miscellaneous/example-descriptions.md) which highlight different features of the Innoactive Creator.

## Instructions

1. [Create a New Unity Project](#create-a-new-unity-project)
1. [Check Prerequisites](#prerequisites)
1. [Import the Examples](#import-the-examples)
1. [Open a Scene](#open-a-scene)
1. [Launch the Training Application](#launch-the-training-application)
1. [Explore the Project](#explore-the-project)
1. [Explore Further](#explore-further)

### Create a New Unity Project

Follow [this guide](../miscellaneous/unity-setup.md) to setup Unity Editor and create a new project for it.

### Prerequisites

Make sure that your setup satisfies the [prerequisites](../miscellaneous/prerequisites.md).

In addition, you need to install two `Windows 10 Language Packages`:

* **English**
* **German (Deutsch)**

You can do this in `Windows Settings` > `Time & Language` > `Language`.

### Import the Examples

Download the `Innoactive Creator` package at the [Innoactive Developer Portal](http://developers.innoactive.de/creator/releases/). Scroll down to the `Innoactive Creator` section, click `Download` button for the latest version and pick the `Innoactive-Creator-vX.Y.Z-Unity-XR.unitypackage`.

Locate the downloaded `.unitypackage` with a file explorer and drag and drop it into the `Project` tab in the Unity Editor. The `Import Unity Package` window will pop up; click `All` and then `Import`.

If you never worked with the Unity Editor before, refer to [this page](https://docs.unity3d.com/Manual/LearningtheInterface.html) to learn about its interface.

### Open a Scene

The project contains multiple example scenes which you can find in the `Assets/Innoactive/Examples/Scenes/Simple` and `Assets/Innoactive/Examples/Scenes/Advanced` subfolders. Open one of these folders in the `Project` view and double-click at a scene file to load it:

![Open Scene](../images/open-scene.png "Project view in the Unity Editor.")

### Connect a VR Headset

Connect a VR headset to your computer and make sure you installed the correct SDK according to our [prerequisites](../miscellaneous/prerequisites.md).

Take a look at the `[COURSE_CONTROLLER]` scene object. You will notice that `Course Mode` is set to `Default`. This prefab provides a trainer with real time controls for the training execution on the monitor. With it, a trainer is able to see the current training status, start, reset, and mute the training, pick a chapter and skip a step, choose a language and the training mode to use.

If you set it to `Standalone`, this prefab provides the trainee with real time controls for the training execution in VR. This is convenient if you want to use a standalone VR headset like the Oculus Quest.

![Course Controller](../images/course-controller.png "Inspector view of the Course Controller.")

### Launch the Training Application

You can simply run the current scene inside the Unity Editor instead of building a full application. For that, click at the `Play` button in the Unity Editor's toolbar (highlighted red):

![Play Button](../images/play-button.png "A screenshot of Unity Editor with a highlighted \"Play\" button.")

To stop it, simply click the same button again.

All `Advanced` scenes use a custom trainer's overlay and wait for you to click at `Start Training` button before executing the training course:

![Start Training](../images/start-training-button.png "Button that starts the training session.")

### Explore the Project

If you never used Unity before, you can learn basics [here](https://docs.unity3d.com/Manual/UsingTheEditor.html).

To open a training course for the current scene, select the `[TRAINING_CONFIGURATION]` game object in the [Hierarchy](https://docs.unity3d.com/Manual/Hierarchy.html) view and click `Open course in Workflow Editor`. It will open the Workflow Editor window.

![How to open a tranining course in the Workflow Editor](../images/open-training-course.png "Screenshot of the Hierarchy and Inspector views which supports the text above.")

### Explore Further

Read the [designer's guide](designer.md) to learn how to use the Innoactive Creator. Read the [developer's guide](developer.md) to learn how to program extensions for it.
