# Getting Started as a Designer

This guide explains how to design new training applications with the Innoactive Creator. To learn how to extend the Innoactive Creator, refer to the [developer's guide](developer.md).

## Examples

You can look up examples as described in [the evaluator's guide](evaluator.md).

## Initial Setup

1. [Create a new Unity Project](#create-a-new-unity-project)
1. [Check Prerequisites](#prerequisites)
1. [Import the Training Template](#import-the-training-template)
1. [Setup the Scene](#setup-the-scene)

### Create a new Unity Project

 Follow the [instructions](../setup-guides/index.md) to create a new project in Unity.

### Import the Training Template

A template is a copy of the `Innoactive Creator` adjusted to the needs of your company or project. Normally, you would receive a `.unitypackage` with it from a template developer; in this tutorial we will use `Innoactive Base Template` instead. Download it from the [Innoactive Developer Portal](http://developers.innoactive.de/creator/releases/): scroll down to the `Innoactive Creator` section, click `Download` button for the latest version and pick the `Innoactive-Creator-vX.Y.Z-Unity-XR.unitypackage`.

Locate the downloaded `.unitypackage` with a file explorer and drag and drop it into the `Project` tab in the Unity Editor. The `Import Unity Package` window will pop up; click `All` and then `Import`.

### Setup the Scene

The training template contains preconfigured scenes which you could copy and reuse.

Copy the `Simplified` scene from the imported template folder (`Innoactive/Creator/Base-Template`) to the `Assets` folder (`Ctrl` + `D` or `Edit` > `Duplicate`, then drag and drop it to the desired directory). If you put it into a subdirectory, it should not be inside the `Innoactive/Creator/Base-Template` folder. Make sure to open the copied scene by double clicking it.

## Create a Basic Training Application

In this section we will create an application for the following training course:

1. Grab the sphere.
2. Bring it to the cube.
3. Watch as the cube flies into the sky.

### Populate the Scene

Add some floors and walls to the scene to prevent the trainee and the training scene objects from falling into the void. You need some colliders without rigidbodies: default Unity **planes** are good enough (`GameObject > 3D Object > Plane`).

Place a **sphere** and a **cube** in trainee's reach, and an **empty object** somewhere in the sky. Use default Unity objects: `GameObject > 3D Object > Sphere`, `GameObject > 3D Object > Cube`, and `Game Object > Create Empty`.

### Add Training Properties

A training property specifies what its object can do. For example, a `GrabbableProperty` indicates that the object can be grabbed. A training property configures the object automatically, with no additional actions required.

> Learn more about [Training Properties](../innoactive-creator/training-scene-object.md#training-scene-object-properties).

* Add a `Grabbable Property` component to the **sphere** game object.
* Add a `Transform In Range Detector Property` component to the **cube** game object.
* Add a `Training Scene Object` component to the **empty** game object. It doesn't require additional properties.

### Assign Unique Names

Every object you want to use in your training is identified by its unique name. To define a unique name you need to change the `Unique Name` property of the `Training Scene Object` component. Do it as follows:

* `Sphere` for the `Training Scene Object` component of the **sphere**.
* `Cube` for the `Training Scene Object` component of the **cube**.
* `The Sky` for the `Training Scene Object` component of the **empty** game object.

Be aware that unique names are case-sensitive.

> Learn more about [Training Scene Objects and Unique Names](../innoactive-creator/training-scene-object.md).

### Save Your Progress

Save the scene (press `CTRL` + `S` on your keyboard) to preserve your progress.

### Create Training Course

#### Training Course Anatomy

The training course is a linear sequence of chapters. Each chapter starts where the previous ends: if a trainee has to drill a hole in a wall in a first chapter, the hole will be there when you load the second chapter. You can start a training course from any chapter.

Each chapter consists of steps that are connected to each other via transitions. Every step consists of a collection of behaviors and transitions. Behaviors are actions that execute independently from the trainee. For example, a behavior can play a sound or move an object from one location to another.

A transition may contain multiple conditions. With conditions, you can define what trainees have to do to progress through the course. When all conditions are completed, the next step (defined in the transition) starts.

#### Training Course Creation Wizard

To create a training course, select the `Innoactive > Creator > Create New Course...` menu option. This will open the Training Creation Wizard. Type the name of your training and press the `Create` button.

The created training course will be set as the selected one for the current scene.

> Learn more about the [Training Course Creation Wizard](../innoactive-creator/course-creation-wizard.md).

#### Workflow Editor

Now the [Workflow Editor](../innoactive-creator/workflow-editor.md) is open and you can view and modify your training course. On the left, the list of chapters is displayed. You can use as many as you want, but let us stick to a single one in this tutorial. You can see the chapter workflow on the right. An empty chapter has only a starting point.

To add a step, click with the right mouse button anywhere on the empty area and choose the `Add step` option. To remove a step, click on the step with the right mouse button and choose the `Delete` option. You can drag a step around the canvas with the left mouse button.

Now, add three new steps to the training.

We need to connect them with transitions: Connect the starting point of the chapter to the first step by dragging the transition origin (a white circle) to the target step. Repeat to connect the first step to the second one, and the second one to the third one. Leave the outgoing transition of the third step as it is. It will automatically lead to the end of the chapter as it has no target step.

If you want to add an outgoing transition to a step, press the small white round button with a `+` sign on right of the step node. Make sure to only have one outgoing transition per step in this tutorial.

To delete a transition, right-click the transition's starting point and choose `Delete transition`. Please note that a step will automatically create a new transition if you reduce the number of transitions to zero.

Now, click on the first step to open it in the [Step Inspector](../innoactive-creator/step-inspector.md).

#### Step Inspector

The [Step Inspector](../innoactive-creator/step-inspector.md) allows you to set a step name and description, as well as to create and edit behaviors, transitions, and conditions. The name and description of the step have no effect on the training course itself. Use them to keep notes for yourself and other trainers. To add a behavior, click on an `Add Behavior` button in the step's view. To delete a behavior, click on the `[x]` button next to it. The same applies to transitions and conditions.

1. Now, rename the first step of the training course to `Grab sphere`. Add a single `Grab Object` condition to its transition to the second step. Drag the previously created `Sphere` into the object field named `Grabbable object`. Add a description:

    > This step will be completed when the trainee grabs the training scene object with the name `Sphere`, which has the `Grabbable Property` attached.

2. Open the second step in the [Step Inspector](../innoactive-creator/step-inspector.md). Rename it to `Bring to cube`. Add an `Object Nearby` condition to its transition to the third step. Set the `Range` to `1.5`, drag `Cube` and `Sphere` into the `First object` and `Second object` fields. Add a description:

    > This step will be completed when the center point of the `Sphere` is 1.5 units away from the center point of the `Cube`, which has a `Transform In Range Detector Property`.

3. Finally, open the third step. Rename it to `Move cube`. Add `Move Object` behavior to it. Drag the `Cube` into the `Object to move` field, `The Sky` into `Final position provider`, and set `Duration in seconds` to 5. Leave the transition to the end of the chapter without conditions. Add a description.

    > The `Cube` will change its position and rotation over five seconds, until its position and rotation match the ones of `The Sky`.

#### Save and Load

The [Workflow Editor](../innoactive-creator/workflow-editor.md) and [Step Inspector](../innoactive-creator/step-inspector.md) automatically save all changes you make.

You can open any selected training course by clicking the `Open in Workflow Editor` button in the `Inspector` tab of the `[TRAINING_CONFIGURATION]` game object. You can find this game object in your scene.

> Learn more about the [Training Configuration](../innoactive-creator/training-configuration.md).

### Make It Run

Find the `[TRAINING_CONFIGURATION]` object in the scene. Make sure the `Selected Training Course` field displays the correct training course. Save the scene and press the `Play` button.

Now try out the training course you created. Grab the sphere and move it near the cube to see the latter fly away!

### Complete Example

You can find our tutorial result alongside the other examples in the `Assets/Innoactive/Examples` directory. Load the scene called `SimpleExample` in the `Assets/Innoactive/Examples/Scenes/Simple` directory.

## Advanced Localization Example

Duplicate and move the `Default` scene from the imported template folder (`Innoactive/Creator/Base-Template/Scenes`) into any other directory. Repopulate it with the same training scene objects as in the first part of this guide.

Take a look at the `[COURSE_CONTROLLER]` scene object. You will notice that `Course Mode` is set to `Default`. This prefab provides a trainer with real time controls for the training execution during runtime. With it, a trainer is able to see the current training status, start, reset, and mute the training, pick a chapter and skip a step, choose a language and the training mode to use.

If you set it to `Standalone`, this prefab provides the trainee with real time controls for the training execution in VR. This is convenient if you want to use a standalone VR headset like the Oculus Quest.

> The `[COURSE_CONTROLLER]` prefab is located in `Innoactive/Creator/Base-Template/Resources/CourseController/Prefabs`, if you want to use it in your own scene.

Note that there is no `TrainingLoader` game object in the scene. Instead, the training is managed by a controller script attached to the camera's overlay. It automatically loads the active training course. Just make sure that the correct training course is selected on the `[TRAINING_CONFIGURATION]` game object.

### Audio Hints and Localization

The `Default` template scene accepts `.json` files as a source of localization data. Create two localization files, one for English and one for German:

#### **en.json**

```json
{
    "grab_sphere": "Please, grab the sphere using the side button of your controller.",
    "put_sphere": "Please, move the sphere closer to the cube.",
    "move_cube": "Behold! The mighty flying cube!",
    "training_complete": "Congratulations! The training is complete."
}
```

#### **de.json**

```json
{
    "grab_sphere": "Bitte nimm die Kugel auf, indem du die seitlichen Knöpfe am Controller gedrückt hältst.",
    "put_sphere": "Bring die Kugel nun bitte zu dem Würfel.",
    "move_cube": "Obacht! Der mächtige Würfel fliegt davon!",
    "training_complete": "Glückwunsch! Du hast das Training erfolgreich absolviert."
}
```

The localization files must be named by the two-letter ISO code of the respective language (`en.json` or `de.json`). Save them to the `[YOUR_PROJECT_ROOT_FOLDER]/Assets/StreamingAssets/Training/[YOUR_COURSE_NAME]/Localization` folder. The script automatically loads all available localizations and displays them in the language dropdown menu. If there is no respective language pack, the localization file is ignored.

### Audio Behavior

In the [Step Inspector](../innoactive-creator/step-inspector.md), you can add either `Play TTS Audio` or `Play Audio File` behavior to a step. It has two parameters:

* `Localization key` is a path to a localized text. If `Play TTS Audio` is used, this localized text is used to generate audio. If `Play Audio File` is used, it uses the text as a [resource path](https://docs.unity3d.com/ScriptReference/Resources.Load.html).
* `Default resource path` and `Default text` are used instead of localized text if `Localization key` is empty or the value is not found.
* `Execution stages` specifies when the audio should be played: at the beginning of the step (`Before Step Execution`), at the end of the step (`After Step Execution`), or both.
* If `Is blocking` is toggled on, step will wait until this behavior is complete. Toggle it on for important information that a trainee has to hear, and toggle it off for optional voice lines, like hints or advices.

Both types of audio behaviors use localized strings. With `Play Audio File`, it allows you to define audio clip resources independently for every supported language. With `Play TTS Audio`, you can provide different text for every language to generate audio from. Add the following to your training:

1. In `Grab sphere` step, add a `Play TTS Audio` behavior with `Localization key` set to `grab_sphere`.
2. In `Bring to cube` step, add a `Play TTS Audio` behavior with `Localization key` set to `put_sphere`.
3. In `Move cube` step, add a `Play TTS Audio` behavior with `Localization key` set to `move_cube`.
4. In `Move cube` step, add another `Play TTS Audio` behavior with `Execution stages` set to `After Step Execution` and `Localization key` set to `training_complete`. Mark it as a blocking behavior.

### Complete Example

You can find our tutorial result alongside the other examples in the `Assets/Innoactive/Examples` directory. Load the scene called `LocalizationExample` in the `Assets/Innoactive/Examples/Scenes/Advanced` directory.

## Default Behaviors

A more detailed description of default behaviors can be found [here](../innoactive-creator/default-behaviors.md).

## Default Conditions

A more detailed description of default conditions can be found [here](../innoactive-creator/default-conditions.md).
