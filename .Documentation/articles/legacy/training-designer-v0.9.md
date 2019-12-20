> The latest guide for training designers can be found [here](../getting-started/designer.md).

# What is a training application

The Innoactive Creator is a part of the Innoactive Hub SDK. It's designed for large enterprises who train people to do manufacturing tasks. With it, you can train employees remotely, safely, and cost-efficiently. Our solution covers the complete lifecycle of virtual training applications, including their creation, maintenance, and distribution to the final user.

A training application has a scene and a training course. A scene is an environment for a trainee to interact with, and a training course is a program that puts objects in motion and guides the trainee. Normally, you need to be a software engineer to define a training course. With the Innoactive Creator SDK, you do it with an intuitive graphical editor.

A training course is made out of chapters. Each chapter starts where the previous ends: if a trainee has to drill a hole in a wall in a first chapter, the hole will be there when you load the second chapter. Chapters consist of individual steps that in turn have any number of behaviors and an exactly one  transition. Each transition can have multiple conditions.

When a training application starts a new step, it executes all its behaviors. Afterwards, it waits until the trainee completes all conditions of the transition. When it happens, the application proceeds to the step at the other end of the transition. If there are no conditions, the step completes at the same time as its behaviors. 
 
Behaviors and conditions communicate with objects in a scene through their training properties: for example, to make use of `Grabbed` condition, you need to attach a `GrabbableProperty` to the target object.

# Initial setup

## Create a new Unity project

Create a new project in Unity. Choose a very short path to the root folder of the project (less than 28 symbols): otherwise, the Unity fails to properly load the project. If you need a longer project path, see the workaround in the [appendix](#appendix).

## Import the training template

A template is a slight modification of the Training Module SDK adjusted to your company's needs. Normally, you would receive a `.unitypackage` file from your template developer, and for introductory purposes you can use the Innoactive Template (it is included in this project, and you can download it separately from our [Releases](http://developers.innoactive.de/components/#training-module) page.

Locate a `.unitypackage` with the training template (if you've downloaded the template from a browser, you should find it in the downloads folder) and drag'n'drop it into the project view in the Unity Editor so the `Import Unity Package` window will pop up. Click `All` and then `Import`.

## Setup the scene

The training template contains preconfigured scenes. All you have to do is a minor setup as instructed and populate it with training objects you need. Copy the `Simplified` scene from the the imported template folder to the `Assets` folder. If you put it into a subdirectory, it should not be inside the template's folder.

# Create a basic training

In this chapter we will create a training for simple tasks:

1. Grab the sphere.
2. Bring it to the cube.
3. Watch as the cube flies into the sky.

## Populate the scene

Add some floors and walls to the scene to prevent trainees and the training inventory from falling into the void. You need some colliders without rigidbodies: default Unity planes are good enough (`GameObject > 3D Object > Plane`). Place a sphere and a cube in trainee's reach, and an empty object somewhere in the sky. Use default Unity objects: `GameObject > 3D Object > Sphere`, `GameObject > 3D Object > Cube`, and `Game Object > Create Empty`. 

### Add training properties

A training property specifies what its object can do. For example, a `GrabbableProperty` indicates that the object can be grabbed. A training property configures the object automatically, with no additional actions required.

* Add a `Grabbable Property` component to the sphere game object.
* Add a `Transform In Range Detector Property` component to the cube game object.
* Add a `Training Object` component to the empty game object. It doesnt require additional properties.

### Assign unique names

Every object you want to use in your training is identified by its unique name. To define a unique name you need to change the `Unique Name` property of the `Training Object` component. Do it as follows:

* `Sphere` for the `TrainingObject` component of the sphere.
* `Cube` for the `TrainingObject` component of the cube.
* `The Sky` for the `TrainingObject` component of the empty game object.

Be aware that unique names are case-sensitive.

### Save your progress

All set! Save the scene (Ctrl+S) to preserve your progress.

## Create training

### Training course anatomy

The training course is a linear sequence of chapters. Each chapter starts where the previous ends: if a trainee has to drill a hole in a wall in a first chapter, the hole will be there when you load the second chapter. You can start a training from any chapter.

Each chapter consists of steps that are connected to each other via transitions. Every step consists of a collection of behaviors and transitions. Behaviors are actions that execute independently from the trainee. For example, a behavior can play an audio or move an object from one point to another.

A transition may contain multiple conditions. With conditions, you can define what trainees have to do to progress through the training. When all conditions are completed, the next step (defined in the transition) starts.

### Training Creation Wizard

To create a training course, select the `Innoactive > Training > Create New...` menu option. This will open the Training Creation Wizard. Type the name of your training and press the `Create` button.

The `Current Scene Default` option will set the course as the selected one for the current scene if ticked.

### Workflow Editor

Now the Workflow Editor is open and you can view and modify your training course. On the left, the list of chapters is displayed. You can use as many as you want, but let's stick to a single one in this tutorial. You can see the chapter workflow on the right. Currently, it has a starting point and nothing else.

To add a step, click with the right mouse button anywhere on the empty area and choose the `Add Step` option. To remove a step, click on the step with the right mouse button and choose the `Delete Step` option. You can drag'n'drop a step around the canvas with the left mouse button.

Now, add three new steps to the training. 

We need to connect them with transitions: first, add an outcoming transition for each step by pressing the small white round buttons with a `+` sign at every step node. Then connect the starting point of the chapter to the first step by dragging the transition origin (a white circle) onto a white circle with a `>` sign of the target step. Repeat to connect the first step to the second one, and the second one to the third one. Leave the outcoming transition of the third step as it is: as it has no target step, it leads to the end of the chapter.

To delete a transition, right-click the transition's starting point and choose `Delete Transition`. Please note that the transition from the chapter's starting point cannot be deleted.

Click on the first step to open it in the Step Inspector.

### Step Inspector

The Step Inspector allows you to rename a step and to change its description, behaviors, transitions, and conditions. The name and description of the step have no effect on the training course itself. Use them to keep notes for yourself and other trainers. To add a behavior, click on an `Add Behavior` button in the step's view. To delete a behavior, click on the `[x]` button next to it. The same applies for transitions and conditions.

Now, rename the first step of the training course to `Grab sphere`. Add a single `Grab Object` condition to its transition to the second step. Type `Sphere` into the field named `Grabbable`. Add a description:

> This step will be completed when the trainee will grab an object with the name `Sphere`, which has `Grabbable Property` attached.

Open the second step in the Step Inspector. Rename it to `Bring to cube`. Add a `Object Nearby` condition to its transition to the third step. Set the `Range` to `1.5`, `Distance Detector` to `Cube`, and `Target` to `Sphere`. Add a description:

> This step will be completed when the center point of the `Sphere` will be 1.5 units from the center point of the `Cube`, which has `Transform In Range Detector Property`.

Finally, open the third step. Rename it to `Move cube`. Add `Move Object` behavior to it. Set `Target` to `Cube`, `Position Provider` to `The Sky`, and `Duration` to 5 seconds. Leave the transition to the end of the chapter without conditions. Add a description:

> The `Cube` will change its position and rotation over five seconds, until its position and rotation match the ones of `The Sky` object.

### Save and load

You can check if you have unsaved changes to your training course in the top right corner of the workflow editor.

To save the current training course, you can click the `Save` button in the Workflow Editor.

> In its current state, the Training SDK may discard the changes when scripts are modified, the `Play` button is pressed, and when Unity Editor is closed. This may not be a comprehensive list.

You can load the selected training course by clicking the `Open in Workflow Editor` button on the `[TRAINING_CONFIGURATION]` game object.

## Make it run

Find the `[TRAINING_CONFIGURATION]` object in the scene. Make sure the `Selected Training Course` field displays the correct training course. Save the scene and press the `Play` button.

## Fully functional example

You can find the fully functional example at `Assets/Examples/Simple`. Just load the scene.

# Advanced example

Now, copy the `Default` scene from the imported template folder. Repopulate it with same training objects. 

If you take a look at [`[HUB-PLAYER-SETUP-MANAGER]`](http://docs.hub.innoactive.de/api/Innoactive.Hub.PlayerSetup.PlayerSetupManager.html) scene object, you will notice that `Spectator Cam Prefab Overload` is overriden with a custom prefab. It is used to provide the trainer with realtime controls for the training execution. Using this overlay, a trainer is able to see the current training status, start, reset, and mute the training, pick a chapter and skip a step, choose a language and the training mode to use.

Note that there is no `TrainingLoader` game object on the scene. Instead, the training is managed by a controller script attached to the camera's overlay. It automatically loads the active training course. Just make sure that the correct training course is selected on the `[TRAINING_CONFIGURATION]` game object.

# Audio hints and localization

The `Advanced` template scene accepts `.json` files as a source of localization data. Create two localization files, one for English and one for German: 

### **en.json**

```json
{
    "grab_sphere": "Please, grab the sphere using the side button of your controller.",
    "put_sphere": "Please, move the sphere closer to the cube.",
    "move_cube": "Behold! The mighty flying cube!",
    "training_complete": "Congratulations! The training is complete."
}
```

### **de.json**

```json
{
    "grab_sphere": "Bitte nimm die Kugel auf, indem du die seitlichen Knöpfe am Controller gedrückt hältst.",
    "put_sphere": "Bring die Kugel nun bitte zu dem Würfel.",
    "move_cube": "Obacht! Der mächtige Würfel fliegt davon!",
    "training_complete": "Glückwunsch! Du hast das Training erfolgreich absolviert."
}
```

The localization files must be named by the two-letter ISO code of the respective language (`en.json` or `de.json`). Save them to the `[YOUR_PROJECT_ROOT_FOLDER]/Assets/StreamingAssets/Training/[YOUR_COURSE_NAME]/Localization` folder. The script automatically loads all available localizations and displays them in the language dropdown menu. If there is no respective language pack, the localization file is ignored. 

> You can add language packs there: `Windows Settings > Time and Language > Language > Add a language`

## Localized strings

The Training SDK uses the `LocalizedString` class to handle the localization. A localized string tries to find the value by a given `key`. If the `key` is not presented in the localization, the `defaultText` value is used instead.

You may leave the `key` empty and use the `defaultText` field if you don't want to use localization functionality.

## Audio behavior

In the Step Inspector, you can add either `Play TTS Audio` or `Play Audio File` behavior to a step. It has two parameters:

* `Localization Key` is a path to a localized text. If `Play TTS Audio` is used, this localized text is used to generate audio. If `Play Audio File` is used, it uses the text as a [resource path](https://docs.unity3d.com/ScriptReference/Resources.Load.html).
* `Default` is used instead of localized text if `Localization Key` is empty or the value isn't found.
* `ActivationMode` specifies when the audio should be played: at the beginning of the step (`Activation`), at the end of the step (`Deactivation`), or both.
* If `Is Blocking` is toggled on, step will wait until this behavior is complete. Toggle it on for important information that a trainee has to hear, and toggle it off for optional voice lines, like hints or advices.

Both types of audio behaviors use localized strings. With `Play Audio File`, it allows you to define audio clip resources independently for every supported language. With `Play TTS Audio`, you can provide different text for every language to generate audio from. Add the following to your training:

1. In `Grab sphere` step, add a `Play TTS Audio` behavior with `Localization Key` set to `grab_sphere`.
2. In `Bring to cube` step, add a `Play TTS Audio` behavior with `Localization Key` set to `put_sphere`.
3. In `Move cube` step, add a `Play TTS Audio` behavior with `Localization Key` set to `move_cube`.
4. In `Move cube` step, add another `Play TTS Audio` behavior with `ActivationMode` set to `Deactivation` and `Localization Key` set to `training_complete`. Mark it as a blocking behavior.

Don't forget to save the changes. Make sure you saved this training with the filename `DefaultTraining.json` (case-sensitive) inside the following folder: `Assets/StreamingAssets/Training/DefaultTraining`.

## Fully functional example

You can find the fully functional example at `Assets/Examples/Advanced`. Just load the scene.

# Default behaviors

* **Delayed Activation:** Wait for `Delay in seconds` and then activate `Behavior to activate`. If `Is Blocking` is not toggled on, this behavior will not prevent completion of the step.
* **Enable Object:** Enables `Target` object for the duration of the step and then disables it.
* **Disable Object:** Disables `Target` object for the duration of the step and then enables it.
* **Lock Object:** Prevents user and physics interactions with the `Target` object for the duration of the step and then unlocks it.
* **Unlock Object:** Allows user and physics interactions with the `Target` object for the duration of the step and then locks it.
* **Move Object:** Moves `Target` object the way its position and rotation will match position and rotation of `Position provider` object after `Duration` seconds.
* **Play Audio:** Plays `AudioData` at the beginning or at the end of the step, depending on `ActivationMode` value.

# Default conditions

* **Grab Object:** Completed when `Grabbable` with `GrabbableProperty` is grabbed.
* **Snap Object:**  Completed when `Snappable` with `SnappableProperty` is snapped to `Zone to snap to` with `SnapZoneProperty`. If there is no `Zone to snap to`, the condition is complete when `Snappable` is snapped to any snap zone.
* **Touch Object:** Completed when `Touchable` with `TouchableProperty` is touched. 
* **Ungrab Object:** Completed when `Grabbable` with `GrabbableProperty` is released.
* **Use Object:** Completed when `Usable` with `UsableProperty` is used.
* **Move Object into Collider:** Completed when pivot point of `Target` object is inside of `Collider` with `ColliderWithTriggerProperty`.
* **Object Nearby:** Completed when pivot point of `Target` object is in `Range` units from `Distance Detector` with `TransformInRangeDetectorProperty`.
* **Timeout:** Completed after `Wait for seconds` seconds.

# Appendix

## Workaround for long project paths

If it is not possible to have a total project path with not more than 28 symbols, assign a drive letter to its root folder. Do the following:

1. Press Win+R.
2. Type `cmd.exe` in and press enter.
3. Type in the following command, where `z` is a free drive letter:

`subst z: C:\path\to\your\project`

Use backslashes (` \ `) and not the forward slashes (` / `) as a directory separator. 
