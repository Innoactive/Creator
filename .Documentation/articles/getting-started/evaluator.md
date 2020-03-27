# Getting Started as an Evaluator

This guide explains how to set up a demo project which displays strengths and features of the Innoactive Creator. After finishing this guide, you could read the [designer's guide](designer.md) to learn how to use the Innoactive Creator in a professional capacity.

## Innoactive Creator Examples

The `Innoactive Creator Examples` is a Unity project of a complete training application built with the `Innoactive Training Template`. It contains [multiple examples](../miscellaneous/example-descriptions.md) which highlight different features of the Tranining Creator.

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

Download the `Innoactive Creator Examples` package at the [Innoactive Developer Portal](http://developers.innoactive.de/components/#training-module). Scroll down to the `Innoactive Creator` section, click `Download` button for the latest version and pick the `innoactive-creator-examples-vX.Y.Z.unitypackage`.

Locate the downloaded `.unitypackage` with a file explorer and drag and drop it into the `Project` tab in the Unity Editor. The `Import Unity Package` window will pop up; click `All` and then `Import`.

If you never worked with the Unity Editor before, refer to [this page](https://docs.unity3d.com/Manual/LearningtheInterface.html) to learn about its interface.

### Open a Scene

The project contains multiple example scenes which you can find in the `Assets/Examples/Simple` and `Assets/Examples/Advanced` subfolders. Open one of these folders in the `Project` view and double-click at a scene file to load it:

![Open Scene](../images/open-scene.png "Project view in the Unity Editor")

### Launch the Training Application

You can simply run the current scene inside the Unity Editor instead of building a full application. For that, click at the `Play` button in the Unity Editor's toolbar (highlighted red):

![Play Button](../images/play-button.png "A screenshot of Unity Editor with a highlighted \"Play\" button.")

To stop it, simply click the same button again.

Connect a VR headset to your computer for the best experience; otherwise, the VRTK simulator will activate. It will lock the mouse cursor, and you would need to press `F4` to interact with the application's interface. Find the rest of VRTK Simulator controls [here](../miscellaneous/vrtk-keymap.md).

All `Advanced` scenes use a custom trainer's overlay and wait for you to click at `Start Training` button before executing the training course:

![Start Training](../images/start-training-button.png "Button that starts the training session.")

### Explore the Project

If you never used Unity before, you can learn basics [here](https://docs.unity3d.com/Manual/UsingTheEditor.html).

To open a training course for the current scene, select the `[TRAINING_CONFIGURATION]` game object in the [Hierarchy](https://docs.unity3d.com/Manual/Hierarchy.html) view and click `Open course in Workflow Editor`. It will open the Workflow Editor window.

![How to open a tranining course in the Workflow Editor](../images/open-training-course.png "Screenshot of the Hierarchy and Inspector views which supports the text above.")

### Explore Further

Read the [designer's guide](designer.md) to learn how to use the Innoactive Creator. Read the [developer's guide](developer.md) to learn how to program extensions for it.