# Inspection Training Template

The Inspection Training Template is a demo template used to demo the Innoactive Creator.

## Prerequisites

The following [prerequisites](../miscellaneous/prerequisites.md) have to be satisfied.

## Instructions

* [Create a new Unity project](../miscellaneous/unity-setup.md#how-to-set-up-unity)
* [Download the Innoactive Creator](#download-the-inspection-training-template)
* [Import the Inspection Training Template](#import-the-inspection-training-template)
* [Play the training course](#play-the-training-course)

### Download the Inspection Training Template

The `Inspection Training Template` can be found at [Innoactive Hub Developer Portal](http://developers.innoactive.de/components/).

![Innoactive Creators](../images/training-modules.png "")

>Make sure to download the file called `inspection-training-template...`

### Import the Inspection Training Template

Once you have a new Unity project and downloaded the `Inspection Training Template`, proceed with importing the package into the Unity project by following these steps:

1. In the Unity's toolbar select: `Assets` > `Import Package` > `Custom Package...`.

    ![Import Unity Package](../images/import-unity-package.png "How to import custome package")

2. In the file explorer, locate and select the `Inspection Training Template` package.
3. An `Import Unity Package` dialog box will appear. Select `All` and then `Import`.

    ![Import Template Window](../images/inspection-template/template-import-package.png "Import Template Window")

4. **_Optional_** [Clear the console](https://docs.unity3d.com/Manual/Console.html).

>If after clearing the console you still get an error asking to change the `Api Compatibility level`, please refer to this [section](../miscellaneous/unity-setup.md#api-compatibility-level) to fix it.

### Play the training course

Once the `Inspection Training Template` package is imported, we can proceed with trying it out.
Unity has several default editor windows. Identify the `Project window`. There is where all assets (files) used in our project are located.

>Learn more about the [Project window](https://docs.unity3d.com/Manual/ProjectView.html)

Inside the `Project window` select:  `Assets` > `Example` > `ExampleInspection`.

![Inspection Training Template project](../images/inspection-template/inspection-training-template-project.png "Inspection Training Template project structure")

>Make sure to double click the file `ExampleInspection` to open it.

You should see how some editor windows changed their content. Click on the `Play button` or `Ctrl` + `P`. Unity should automatically switch focus to the `Game window`. The `Game window` is where the application is rendered. This view is exactly the view the application will have once built into an executable.

>Learn more about the [Game window](https://docs.unity3d.com/Manual/GameView.html)

![Inspection Training](../images/inspection-template/inspection-training-template-scene.png "Inspection Training")

In the `Game window` select `Start Training` and follow the instructions until completing the training course.

>See other default [Editor windows](https://docs.unity3d.com/Manual/UsingTheEditor.html)
