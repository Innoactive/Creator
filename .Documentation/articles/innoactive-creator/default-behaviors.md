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

The `Play Audio File` behavior plays audio clips from any `Resources` folder in your project's asset folder.

### Configuration

- #### Localization key

    If you use localization files for your project, you can write the localization key for the corresponding sound file that you want to play into this text field. For more information on how to use localization files and Text to speech, look into [Text to speech engine (TTS)](../miscellaneous/setup-text-to-speech.md).
    If you do not use localization files, leave this text field empty and use the `Default resource path` field.

    ##### Example

    You have the following localization file `EN.json`:
    
    ```json
    {
        "humming_sound": "Sounds/Teleporter/humming",
        "teleport_sound": "Sounds/teleport",
        "teleport_complete": "Congratulations! You successfully used the teleporter!"
    }
    ```

    If you want to hear the teleport sound, you have to enter the `teleport_sound` key into the `Localization key` text field.

- #### Default resource path

    If you want to play an audio clip from a file with this behavior, the file path must be the relative path **after** the `Resources` folder. The extension of the file must be omitted.

    ##### Example
     
    File to be played: `Assets/.../Resources/Sounds/teleport.ogg`  
    Path entered in the field: `Sounds/teleport` 

- #### Execution stages

    By default, steps execute behaviors in the beginning, in their activation stage. This can be changed with the `Execution stages` dropdown menu:

    - `Before Step Execution`: The step invokes the behavior during its activation.
    - `After Step Execution`: Once a transition to another step has been selected and the current step starts deactivating, the behavior is invoked.
    - `Before and After Step Execution`: Execution at activation and deactivation of a step.

- #### Is blocking

    By default, a behavior is blocking the transition to another step while the behavior is executing. If you want to skip a behavior's execution when a transition to another step is happening (e. g. when all conditions of one transition are met), you can uncheck this option. 
    In this case the unchecked `Is blocking` option means that the audio clip will be interrupted or not even started when a transition to another step is happening.

------

## Audio/Play TextToSpeech Audio

This behavior is part of the TextToSpeech Component. The Innoactive Base Template provides it by default.

### Description

The `Play TextToSpeech Audio` behavior plays speech audio clips generated from text. The behavior can convert localized text into speech by fetching values from `.json` files using the provided localization keys. If either the key in `Localization Key` does not exist or the text field is empty, the text entered into the `Default text` field will be used.

### Configuration

- #### Localization key

    If you use localization files for your project, you can write the localization key for the corresponding text that you want to hear into this text field. For more information on how to use localization files and Text to speech, look into [Text to speech engine (TTS)](../miscellaneous/setup-text-to-speech.md).
    If you do not use localization files, leave this text field empty and use the `Default text` field.

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

    If you want to hear the spoken text *"Behold! The mighty flying cube!"*, you have to enter the `move_cube` key into the `Localization key` text field.

- #### Default text

    The text entered into this text field is used, if either the `Localization key` text field is empty or the provided localization key is invalid (e. g. the key does not exist or the localization file is not loaded).

- #### Execution stages

    By default, steps execute behaviors in the beginning, in their activation stage. This can be changed with the `Execution stages` dropdown menu:

    - `Before Step Execution`: The step invokes the behavior during its activation.
    - `After Step Execution`: Once a transition to another step has been selected and the current step starts deactivating, the behavior is invoked.
    - `Before and After Step Execution`: Execution at activation and deactivation of a step.

- #### Is blocking

    By default, a behavior is blocking the transition to another step while the behavior is executing. If you want to skip a behavior's execution when a transition to another step is happening (e. g. when all conditions of one transition are met), you can uncheck this option. 
    In this case the unchecked `Is blocking` option means that the audio clip will be interupted or not even started when a transition to another step is happening.

------

## Behavior Sequence

### Description

The `Behavior Sequence` contains a list of child behaviors which will be activated one after the other. This means that the next child behavior in the list will not be activated until the previous child behavior has finished its life cycle.

### Configuration

- #### Repeat

    If this option is checked, the behavior sequence will start from the top of the child behaviors list over and over again as soon as the life cycle of the last child behavior in the list is finished.

- #### Child behaviors

    This is the list of all queued behaviors. By clicking on the `Add Behavior` button below it, you can add any behavior to it.

- #### Is blocking

    By default, the behavior sequence is blocking the transition to another step while it is executing all its child behaviors in the list. After completion, it does not block the transition to another step anymore even if you enabled the `Repeat` option. 
    If you want to skip or interrupt the execution of the behavior sequence when a transition to another step is happening (e. g. when all conditions of one transition are met), you can uncheck this option. 

------

## Delay

### Description

The activated `Delay` behavior completes after the specified amount of time. This behavior is especially useful for the `Behavior Sequence`, if you want to have time breaks in between its child behaviors. But you can also just delay the completion of a step with it.

### Configuration

- #### Delay in seconds

    In this field you can set the behavior's delay duration in seconds.

------

## Disable Object

### Description

The `Disable Object` behavior **deactivates** the target's *GameObject* until it will be enabled again.

### Configuration

- #### Object to disable

    This field contains the `Training Scene Object` to be disabled.

------

## Enable Object

### Description

The `Enable Object` behavior **activates** the target's *GameObject* until it will be disabled again.

### Configuration

- #### Object to enable

    This field contains the `Training Scene Object` to be enabled.

------

## Hightlight Object

### Description

The `Highlight Object` behavior will highlight the target object in the specified color until the end of the step.

### Configuration

- #### Hightlight color

    This field contains the color in which the target object will be colored.

- #### Object to highlight

    This field contains the `Training Scene Object` which should be highlighted.

------

## Lock Object

### Description

The `Lock Object` behavior locks the target object so that you can no longer interact with this object in VR (like touching or grabbing).

### Configuration

- #### Object to lock

    This field contains the `Training Scene Object` to be locked.

- #### Lock only during this step

    If toggled on, the object locks only while the current step is being executed. It is unlocked again when moving to the next step.

------

## Move Object

### Description

The `Move Object` behavior moves and rotates the target game object to the position and rotation of the position provider game object within a specified time. Note that if the game object is affected by gravity before it moves, it will also be affected by gravity afterwards.

### Configuration

- #### Object to move

    This field contains the `Training Scene Object` to be moved which should be moved and rotated.

- #### Final position provider

    This field contains the `Training Scene Object` that is being used as position provider object which should be placed at the exact position and rotation where you want to move and rotate your target object to.

- #### Duration in seconds

    In this field you can set the number of seconds it should take to move and rotate the target object.

------

## Unlock Object

### Description

The `Unlock Object` behavior unlocks a previously locked object so that it can be interacted with in VR.

### Configuration

- #### Object to unlock

    This field contains the `Training Scene Object` to be unlocked.

- #### Unlock only during this step

    If toggled on, the object unlocks only while the current step is being executed. It is locked again when moving to the next step.
