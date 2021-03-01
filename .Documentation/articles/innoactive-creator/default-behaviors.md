# Default Behaviors

The following behaviors are part of the Basic Conditions and Behaviors Component. The Innoactive Base Template provides them by default.

Take a look at the [Training Scene Object](training-scene-object.md) article if you have not read it yet. It will help you to understand how to handle training scene objects and training properties.

See [this article](step-inspector.md) to learn Step Inspector controls.

## Content

- [Audio/Play Audio File](#audioplay-audio-file)
- [Audio/Play TextToSpeech Audio](#audioplay-texttospeech-audio)
- [Behavior Sequence](#behavior-sequence)
- [Delay](#delay)
- [Disable Object](#disable-object)
- [Enable Object](#enable-object)
- [Hightlight Object](#hightlight-object)
- [Lock Object](#lock-object)
- [Move Object](#move-object)
- [Unlock Object](#unlock-object)

------

## Audio/Play Audio File

### Description

This Behavior plays an audio clip loaded from the `Resources` folder in your project’s asset folder. The Innoactive Creator supports all audio file formats supported by Unity, which are 

- aif
- wav
- mp3
- ogg

### Application Example

- The trainee is supposed to react to alarming signs: the trainee hears a suspicious clicking sound during his training, which he is supposed to identify as a sign of danger. He then has to react accordingly.

- Feedback of interactions: feedback sounds for using tools, e.g. drill sound for a drill.


### Configuration

- #### Localization key

    Use this if there are localization files (JSON) for preferred languages in your project which contains a key-value mapping for various file paths or string values. 

    Leave this field empty and use the `Default resource path` below if you want to play an audio clip from a specific file instead.

    ##### Example

    You have the following localization file `EN.json`:
    
    ```json
    {
        "humming_sound": "Sounds/Teleporter/humming",
        "teleport_sound": "Sounds/teleport",
        "teleport_complete": "Congratulations! You successfully used the teleporter!"
    }
    ```

    If you want to hear the teleport sound, you have to enter the key *"teleport_sound"* into the `Localization key` text field.

- #### Default resource path

    Relative file path from the Resources folder. Omit the file extension (see example).
    Use this field if you do not want to use localization files specified in `Localization Key`.


    ##### Example
     
    File to be played: `Assets/.../Resources/Sounds/click-sound.ogg`  
    Default resource path: `Sounds/click-sound`  

- #### Execution stages

    By default, steps execute behaviors in the beginning, in their activation stage. This can be changed with the `Execution stages` dropdown menu:

    - `Before Step Execution`: The step invokes the behavior during its activation.
    - `After Step Execution`: Once a transition to another step has been selected and the current step starts deactivating, the behavior is invoked.
    - `Before and After Step Execution`: Execution at activation and deactivation of a step.

- #### Wait for completion

    By default, the step waits for the audio file to finish. If you want the step to interrupt the audio in case the trainee completes the condition(s), uncheck this option. 
    
    Note: this might lead to an audio file not even being started.

### Location of this behavior (for developers):

This behavior is part of the <a href="https://github.com/Innoactive/Basic-Conditions-And-Behaviors" target="_blank">Basic-Conditions-And-Behaviors</a> component. The file is located <a href="https://github.com/Innoactive/Basic-Conditions-And-Behaviors/blob/develop/Runtime/Behaviors/PlayAudioBehavior.cs" target="_blank">here</a>

------

## Audio/Play TextToSpeech Audio


### Description

This behavior reads digital text aloud. The behavior can convert localized text into speech by fetching values from the localization files (.json) using the provided localization keys. 
If the key in `Localization Key` does not exist or the text field is empty, the text entered into the `Default text` field will be used.

### Application Example

- You want to give your trainees an audio instruction of what they need to accomplish in their training.
- You train the same process in multiple countries requiring instructions in multiple languages.


### Configuration

The default language is set to ‘English’. Consult [the Text-to-Speech documentation](https://developers.innoactive.de/documentation/creator/latest/articles/developer/12-text-to-speech.html) to learn how to configure the Text-to-Speech Engine (TTS).


- #### Localization key

    Use this if there are localization files (JSON) for preferred languages in your project which contains a key-value mapping for various file paths or string values.

    Leave this field empty and use the `Default text` below if you want a text-to-speech engine to read your typed text.

    ##### Example

    You have the following localization file `EN.json`:

    ```json
    {
        "grab_sphere": "Please, grab the sphere using the side button of your controller.",
        "put_sphere": "Please, move the sphere closer to the cube.",
        "move_cube": "Behold! The mighty flying cube!",
        "training_complete": "Congratulations! The training is complete."
    }
    ```

    If you want to hear the spoken text *"Behold! The mighty flying cube!"*, you have to enter the key *"move_cube"* into the `Localization key` text field.

- #### Default text

    The text entered into this text field is used, if either the `Localization key` text field is empty or the provided localization key is invalid (e.g. the key does not exist or the localization file is not loaded).

    Use this field if you do not want to use localization files specified in `Localization Key`.

- #### Execution stages

    By default, steps execute behaviors in the beginning, in their activation stage. This can be changed with the `Execution stages` dropdown menu:

    - `Before Step Execution`: The step invokes the behavior during its activation.
    - `After Step Execution`: Once a transition to another step has been selected and the current step starts deactivating, the behavior is invoked.
    - `Before and After Step Execution`: Execution at activation and deactivation of a step.

- #### Wait for completion

    By default, the step waits for the audio file to finish. If you want the step to interrupt the audio in case the trainee completes the conditions, uncheck this option. 
    
    Note: this might lead to an audio file not even being started.

### Location of this behavior (for developers):

This behavior is part of the <a href="https://github.com/Innoactive/Basic-Conditions-And-Behaviors" target="_blank">Basic-Conditions-And-Behaviors</a> component. The file is located <a href="https://github.com/Innoactive/Basic-Conditions-And-Behaviors/blob/develop/Runtime/Behaviors/PlayAudioBehavior.cs" target="_blank">here</a>


------

## Behavior Sequence

### Description

This behavior contains a list of child behaviors which will be activated one after another. A child behavior in the list will not be activated until the previous child behavior has finished its life cycle.

### Application Example

- the trainee places a package to the assembly line. Use a behavior sequence to move the package first to the right on the assembly line, then to the left onto the next assembly line.

- the trainee should watch a short tutorial where trainers explain the next steps which need to be accomplished. For example, give instructions to push button A, then highlight button A, then give instructions to push button B, then highlight button B. Afterwards, give the instruction to execute the sequence shown in the tutorial.


### Configuration

- #### Repeat

    if checked, the behavior sequence restarts from the top of the child behavior list as soon as the life cycle of the last child behavior in the list has finished.

- #### Child behaviors

    List of all queued behaviors. Add behaviors to the list using the *"Add Behavior"* button.

- #### Wait for completion

    if checked, the behavior sequence will finish the life cycle of each child behavior in the list before it transitions to another step. Even when the *"Repeat"* option is enabled, the execution will transition to the next step after the child behavior list has been completed. 
    Uncheck this option, If you want to interrupt the sequence as soon as all conditions of a transition are fulfilled.

### Location of this behavior (for developers):

This behavior is part of the <a href="https://github.com/Innoactive/Basic-Conditions-And-Behaviors" target="_blank">Basic-Conditions-And-Behaviors</a> component. The file is located <a href="https://github.com/Innoactive/Basic-Conditions-And-Behaviors/blob/develop/Runtime/Behaviors/BehaviorSequence.cs" target="_blank">here</a>


------

## Delay

### Description

This behavior completes after the specified amount of time. Even when trainees fulfill the required conditions to transition to the next step, this step will wait for the duration configured in `Delay (in seconds)`.  

### Application Example

- In a Behavior Sequence, this delay behavior allows you to insert pauses between its child behaviors.

### Configuration

- #### Delay (in seconds)

    configure the behavior’s delay duration in seconds.

    ##### Example

    Delay (in seconds) = 1.3

### Location of this behavior (for developers):

This behavior is part of the <a href="https://github.com/Innoactive/Basic-Conditions-And-Behaviors" target="_blank">Basic-Conditions-And-Behaviors</a> component. The file is located <a href="https://github.com/Innoactive/Basic-Conditions-And-Behaviors/blob/develop/Runtime/Behaviors/DelayBehavior.cs" target="_blank">here</a>


------

## Disable Object

### Description

This behavior makes the selected `Object` invisible and non-interactive until it specifically is set back to *"enabled"* in a future step.
Put into Unity terms, it deactivates the selected Game Object.

If you would like to make an object non-interactive while being visible, see the [Suspending Interactions](https://developers.innoactive.de/documentation/creator/latest/articles/innoactive-creator/suspending-interactions.html) article.

### Application Example

- The Trainee has finished working with certain tools. In order to clean up the scene, you might want to disable such tools.


### Configuration

- #### Object

    the `Training Scene Object` to be disabled.

### Location of this behavior (for developers):

This behavior is part of the <a href="https://github.com/Innoactive/Basic-Conditions-And-Behaviors" target="_blank">Basic-Conditions-And-Behaviors</a> component. The file is located <a href="https://github.com/Innoactive/Basic-Conditions-And-Behaviors/blob/develop/Runtime/Behaviors/DisableGameObjectBehavior.cs" target="_blank">here</a>


------

## Enable Object

### Description

This behavior makes the selected `Object` visible and interactive until it is specifically set back to *"disabled"* in a future step.
Put into Unity terms, it activates the selected Game Object.

### Application Example

- In some scenarios you want trainees to interact with objects that are not visible at the start of the scene. By enabling them they can be “added” to the training scene when needed.

- Use this behavior as an easy way to add additional visual hints.


### Configuration

- #### Object

    the `Training Scene Object` to be enabled.

### Location of this behavior (for developers):
    
This behavior is part of the <a href="https://github.com/Innoactive/Basic-Conditions-And-Behaviors" target="_blank">Basic-Conditions-And-Behaviors</a> component. The file is located <a href="https://github.com/Innoactive/Basic-Conditions-And-Behaviors/blob/develop/Runtime/Behaviors/EnableGameObjectBehavior.cs" target="_blank">here</a>

------

## Hightlight Object

### Description

This behavior visually highlights the selected object until the end of a step.

Select the highlighted `Object` in the Unity Hierarchy and open the Unity Inspector. Search for the *Interactable Highlighter Script*.

[![Interactable Highlighter Script](../images/default-behaviors/interactable-highlighter-script.png "")](../images/default-behaviors/interactable-highlighter-script.png)

You can define the Color and Material for *On Touch Highlight*, *On Grab Highlight*, and *On Use Highlight*. The object will show the highlight color configured in the Highlight behavior by default, as soon as the object is touched it will change to the color configured in *On Touch Highlight*. The same happens when the object is grabbed or used. It will display the configured color in ‘On Grab Highlight’ or ‘On Use Highlight’. 

### Application Example

- Give trainees feedback when interacting with objects in the scene. 

- Give trainees feedback when entering an object using their controllers (*On Touch Highlight*) or when grabbing (*On Grab Highlight*) or using (*On Use Highlight*) an object. 

### Configuration

- #### Color

    Color in which the target object will be highlighted. Colors are defined in the RGBA or HSV color channel. By configuring the alpha (A) value, highlights can be translucent.

- #### Object

    the `Training Scene Object` which should be highlighted.

### Location of this behavior (for developers):
This behavior is part of the <a href="https://github.com/Innoactive/Basic-Conditions-And-Behaviors" target="_blank">Basic-Conditions-And-Behaviors</a> component. The file is located <a href="https://github.com/Innoactive/Basic-Conditions-And-Behaviors/blob/develop/Runtime/Behaviors/HighlightObjectBehavior.cs" target="_blank">here</a>

------

## Lock Object (deprecated)
Deprecated: please see [Suspending Interactions](https://developers.innoactive.de/documentation/creator/latest/articles/innoactive-creator/suspending-interactions.html).

### Description

The `Lock Object` behavior locks the target object so that you can no longer interact with this object in VR (like touching or grabbing).

### Configuration

- #### Object

    This field contains the `Training Scene Object` to be locked.

- #### Lock only during this step

    If toggled on, the object locks only while the current step is being executed. It is unlocked again when moving to the next step.

------

## Move Object

### Description

This behavior animates the `Object` to move and rotate (no scaling) to the position and rotation of the `Final Position Provider` in the time in seconds specified in `Duration (in seconds)`.
 
Note: If `Object` was affected by gravity before, it will continue to be effected after this behavior. 

### Application Example

- When trainees pick up the wrong tool, move the tool back to their initial place as soon as trainees release the object.

### Configuration

- #### Object

    the `Training Scene Object` to be moved and rotated (no scaling).

- #### Final position provider

    the `Training Scene Object` that is being used as position provider object which should be placed at the exact position and rotation where you want to move and rotate your `object` to.

- #### Animation duration (in seconds)

    time in seconds the animation takes to move and rotate `Object` to the `Final position provider`.

    ##### Example
    
    Duration (in seconds) = 1.3


### Location of this behavior (for developers):

This behavior is part of the <a href="https://github.com/Innoactive/Basic-Conditions-And-Behaviors" target="_blank">Basic-Conditions-And-Behaviors</a> component. The file is located <a href="https://github.com/Innoactive/Basic-Conditions-And-Behaviors/blob/develop/Runtime/Behaviors/MoveObjectBehavior.cs" target="_blank">here</a>

------

## Unlock Object (deprecated)

Deprecated: please see [Suspending Interactions](https://developers.innoactive.de/documentation/creator/v2.8.0/articles/innoactive-creator/suspending-interactions.html).

### Description

The `Unlock Object` behavior unlocks a previously locked object so that it can be interacted with in VR.

### Configuration

- #### Object

    This field contains the `Training Scene Object` to be unlocked.

- #### Unlock only during this step

    If toggled on, the object unlocks only while the current step is being executed. It is locked again when moving to the next step.
