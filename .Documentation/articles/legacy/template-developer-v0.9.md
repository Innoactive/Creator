# What is a training application

The Innoactive Creator is a part of the Innoactive Hub SDK. It's designed for large enterprises who train people to do manufacturing tasks. With it, you can train employees remotely, safely, and cost-efficiently. Our solution covers the complete lifecycle of virtual training applications, including their creation, maintenance, and distribution to the final user.

A training application has a scene and a training course. A scene is an environment for a trainee to interact with, and a training course is a program that puts objects in motion and guides the trainee. Normally, you need to be a software engineer to define a training course. With the Innoactive Creator SDK, you do it with an intuitive graphical editor.

A training course is made out of chapters. Each chapter starts where the previous ends: if a trainee has to drill a hole in a wall in a first chapter, the hole will be there when you load the second chapter. Chapters consist of individual steps that in turn have any number of behaviors and an exactly one  transition. Each transition can have multiple conditions.

When a training application starts a new step, it executes all its behaviors. Afterwards, it waits until the trainee completes all conditions of the transition. When it happens, the application proceeds to the step at the other end of the transition. If there are no conditions, the step completes at the same time as its behaviors. 
 
Behaviors and conditions communicate with objects in a scene through their training properties: for example, to make use of `Grabbed` condition, you need to attach a `GrabbableProperty` to the target object.

# What is a template

A training designer can setup a scene and a training course on his own, but what if he needs a behavior or condition that is not there by default? As a software developer, you configure and extend the Innoactive Creator SDK to fit your needs, creating a template (this why do we call you a template developer). This document describes how to create one: if you follow it, you will create a template that is very similar to this project.

Note that this project is just an example. We focus on explaining the Innoactive Creator SDK: everything else is simplified to not distract you. Sacrifices had to be made: you can't comfortably extend this project or use it in production.

# Initial setup

## Setup the Hub SDK

Hub SDK is used for the development of Innoactive Hub VR applications. Check the [pre-requisites](http://docs.hub.innoactive.de/v4.0.0/articles/sdk-setup.html#prerequisites) and follow the [instructions](http://docs.hub.innoactive.de/v4.0.0/articles/sdk-setup.html#importing-the-hub-sdk) to set it up.

## Install VR SDKs

Install the SDKs of the VR headsets you're working with. For example, [SteamVR SDK](https://github.com/Innoactive/SteamVR).

## Import the Innoactive Creator

You can find the latest stable version at [Innoactive Hub Developer Portal](http://developers.innoactive.de/components/#training-module).

# The simplest training template

The simplest template possible consists of a single scene that contains a Innoactive Hub SDK and Innoactive Creator configuration, preconfigured VR headset, and an object with a single script that would load a training.

All that a training designer would have to do only two steps:

1) Copy the scene and populate it with training objects he needs.
2) Create a training course, save it in the default training course folder, and select it in the `Runtime Configurator` component on the game object called `[TRAINING_CONFIGURATION]` in the scene.

## Create a scene

Create a new Unity scene in which you want a VR training to be executed.

## Setup the scene as a Training Scene

Select the following option in the Unity editor's toolbar: `Innoactive > Training > Setup Scene as a Training Scene`.

## Create a script

Create a new C# script named TrainingCourseLoader and replace its contents with the following code snippet:

```c#
using System.Collections;
using Innoactive.Hub.Training.Configuration;
using UnityEngine;

namespace Innoactive.Hub.Training.Template
{
    public class TrainingCourseLoader : MonoBehaviour
    {
        private IEnumerator Start()
        {
            // Skip the first two frames to give VRTK time to initialize.
            yield return null;
            yield return null;

            // Load the training course selected from the Runtime Configurator
            // in the '[TRAINING_CONFIGURATION]' game object in the scene.
            ICourse trainingCourse = RuntimeConfigurator.Configuration.LoadCourse();

            // Start the training execution.
            trainingCourse.Activate();
        }
    }
}
```

## Add a training course loader to the scene

1) Add a new empty game object to the scene.
2) Add the `Trainer Course Loader` component to it.

## The complete example

To verify the achieved result, you can compare it to the scene under the following path: `[Path to Innoactive Template]/Scenes/Simple`.

# Advanced topics

Cheers! You've covered the basics.

The following chapters describe available Innoactive Creator features and explain how to use them to create more complex templates. You can tune both training designer tools and a running application logic with Training module configurations, or you can extend it with your own behaviors and conditions.

We recommend you to explore this project while you are reading this tutorial.

# Template configuration

You can either modify training designer tools with editor configuration, or you can define how a built training application would look and behave like. 

## Editor configuration

To change the editor configuration, implement `Innoactive.Hub.Training.Editors.Configuration.IEditorConfiguration` interface. The Innoactive Creator will automatically locate it if you do so, and will use the `DefaultEditorConfiguration` otherwise. If there is more than one custom editor configuration, an error will be logged and a random configuration will be used.

 > This is the main reason why we recommend to build a template from scratch instead of extending this project.

The `DefaultEditorConfiguration` configures the Innoactive Creator in a basic way. Inherit from it to not implement everything from scratch. Take a look at `InnoactiveEditorConfiguration` to see how to implement complex things like `Audio Hint` menu option.

With editor configuration, you can limit or allow training designers to use certain behaviors and conditions. For example, you might add a custom behavior that highlights its target object in a special way, and hide the default `Highlight` behavior.

1. `BehaviorsMenuContent` property defines a dropdown menu which is shown when a training designer clicks on `Add Behavior` button.
2. `ConditionsMenuContent` property defines a dropdown menu which is shown when a training designer clicks on `Add Condition` button.

Finally, you define what happens when someone clicks at `Innoactive > Training > Setup Scene` menu option in the `SetupTrainingScene()` method. By default, it set ups the Hub SDK and creates a training configuration object.

## Runtime configuration

You can use the runtime configuration to adjust the way the training application executes in a runtime.
 
There should be one and only one training runtime configurator scene object in a scene. In the scenes of this example template, the game object is called `[TRAINING_CONFIGURATION]`. This object is a container for the runtime configuration, which actually customizes the template. You can assign the runtime configuration either programmatically, or with the game object inspector. By default, the `Innoactive.Hub.Training.Configuration.DefaultRuntimeConfiguration` is used. Extend it to create your own, and then assign it to the scene's game object. Take a look at `InnoactiveRuntimeConfiguration` to see how to implement other training modes like the `No Audio Hints` mode.
 
The runtime configuration has the following properties and methods:

1. `SelectedCoursePath` returns the absolute path to the training course file.
2. `LoadCourse()` returns the deserialized training course (`ICourse`) located at `SelectedCoursePath`.
3. `SceneObjectRegistry` provides the access to all training objects and properties of the current scene.
4. `Trainee` is a shortcut to a trainee's headset.
5. `InstructionPlayer` is a default audio source for all `PlayAudioBehavior` behaviors.
6. `TextToSpeechConfig` defines a TTS engine, voice, and language to use to generate audio.
7. `SetMode(index)` sets current mode to the one at provided `index` in the collection of available modes.
8. `GetCurrentMode()` returns the current mode.
9. `GetCurentModeIndex()` returns the current mode's index.
10. `AvailableModes` returns a collection of all modes available. Normally, this is a single modes-related class member you want to override.

The next chapters explain the TTS configuration and training modes in detail.

## TTS configuration

The text-to-speech config defines the TTS engine, voice, and language to use. By default, the training configuration loads TTS config from `[YOUR_PROJECT_ROOT_FOLDER]/Config/text-to-speech-config.json` file (if there is any). A different TTS config can be set at runtime, but it will have no effect until you reload the current training.

The `Provider` property contains the name of a TTS provider class. For example, `MicrosoftSapiTextToSpeechProvider` uses the Windows TTS engine (works offline), and `WatsonTextToSpeechProvider` uses the Watson TTS (requires internet connection). 

The acceptable values for the `Voice` and the `Language` properties differ from provider to provider. For online TTS engines, the `Auth` property may be required, as well.

### Using the offline Windows TTS

Windows 10 has a built-in speech synthesizer. It doesn't require an internet connection but a respective Language Pack has to be installed at end user's system (`Windows Settings > Time and Language > Language > Add a language`).

The `Language` field of a config takes either natural name of the language or [its two-letter ISO language code](https://msdn.microsoft.com/en-us/library/cc233982.aspx). Valid values of a `Voice` field are `Male`, `Female`, and `Neutral`.

> Despite the name, some two-letter ISO codes are three letters long.

An example of a proper config:

```c#
new TextToSpeechConfig()
{
    // Define which TTS provider is used.
    Provider = typeof(MicrosoftSapiTextToSpeechProvider).Name,
    
    Voice = "Female",
    
    Language = "EN"
};
```

## Localization

The Training SDK uses the `Localization` class from the Hub SDK. It's a wrapper around a dictionary of strings with convenient API. To define a current localization, either assign `entries` property directly, or load it from a JSON file with the `LoadLocalization(string path)` method. The valid JSON file complies to the following key-value structure:

```json
{
  "translated_text": "Ein übersetzter Text.",
  "example": "Ein Beispiel." 
}
```

> The `Localization` class is not concerned about current language or which localization file should be loaded. You have to manage it in the template.

Whenever you want to localize a `string` value, replace it with a `LocalizedString`. When `Value` property is invoked, it searches for a localization entry by the `Key` and returns the result. If `Key` isn't specified or the entry is missing, it uses the `DefaultText` instead. 

## Instruction player

A `PlayAudioBehavior` uses the value of the `RuntimeConfigurator.Configuration.InstructionPlayer` property as its audio source. By default, the property attaches an audio source to the trainee's headset.

## Training modes

Some conditions and behaviors reference the current training mode for custom parameters. For example, parameters can define in which color the object has to be highlighted. The training mode parameters is a string-to-object dictionary. Additionally, the current mode defines which behaviors and conditions should be entirely skipped.

The default runtime configuration has only one available mode. It allows any condition or behavior, and has no parameters. To define your own training modes, override the `AvailableModes` property in your own new training runtime configuration. To switch between modes, call `SetMode(index)` method. Use the `Mode` class constructor to create a mode.

# Extend the Innoactive Creator

The default behaviors and conditions are sufficient for most of the trainings, but you might have to write your own to handle very specific cases.

> To name one, we do not provide the "Is that sheep shaved?" condition. 

In this chapter we will create a behavior that changes the scale of a target object, and a condition that triggers when a trainee points at a given object with a laser pointer.

## Custom training property

Behaviors and conditions communicate with objects on the scene through their properties. To create a pointing condition, we need to create a property for a pointer object first. Create new C# script named `PointingProperty` and set its contents to the following:

```c#
using System;
using Innoactive.Hub.Training.SceneObjects.Properties;
using UnityEngine;
using VRTK;

namespace Innoactive.Hub.Training.Template
{
    // PointingProperty requires VRTK_Pointer to work.
    [RequireComponent(typeof(VRTK_Pointer))]
    // Training object with that property can point at other training objects.
    // Any property should inherit from SceneObjectProperty class.
    public class PointingProperty : SceneObjectProperty
    {
        // Event that is invoked every time when the object points at something.
        public event Action<ColliderWithTriggerProperty> PointerEnter;

        // Reference to attached VRTK_Pointer.
        private VRTK_Pointer pointer;

        // Fake the pointing at target. Used when you fast-forward PointedCondition.
        public virtual void FastForwardPoint(ColliderWithTriggerProperty target)
        {
            if (target != null && PointerEnter != null)
            {
                PointerEnter(target);
            }
        }

        // Unity callback method
        protected override void OnEnable()
        {
            // Training object property handle their initialization at OnEnable().
            base.OnEnable();

            // Find attached VRTK_Pointer.
            pointer = GetComponent<VRTK_Pointer>();

            // Subscribe to VRTK_Pointer's event which is raised when it hits any collider.
            pointer.DestinationMarkerEnter += PointerOnDestinationMarkerEnter;
        }

        // Unity callback method
        private void OnDisable()
        {
            // Unsubscribe from VRTK_Pointer's event.
            pointer.DestinationMarkerEnter -= PointerOnDestinationMarkerEnter;
        }

        // VRTK_Pointer.DestinationMarkerEnter handler.
        private void PointerOnDestinationMarkerEnter(object sender, DestinationMarkerEventArgs e)
        {
            // If target is ColliderWithTriggerProperty, raise PointerEnter event.
            ColliderWithTriggerProperty target = e.target.GetComponent<ColliderWithTriggerProperty>();
            if (target != null && PointerEnter != null)
            {
                PointerEnter(target);
            }
        }
    }
}
```

This property does the following:

* It ensures that a scene object has all required components attached.
* It encapsulates the VRTK_Pointer event handling.
* It exposes an event so a training condition could use it, and it ensures that the event is fired only when the pointer points at a training object with a collider or the condition was fast-forwarded.

Now it can be attached to a game object on a scene. To save time on making your own pointer tool, copy `Your Hub SDK Directory\SDK\Tools\Presenter\Resources\Presenter` and attach the property to its `Pointer` child game object.

> Note that we expect the `ColliderWithTriggerProperty` to be attached to the training object we point at. VRTK_Pointer expects an object to have a collider with a trigger, and the `ColliderWithTriggerProperty` ensures that its owner object has one.

## Custom condition

Create new C# script named `PointedCondition` and change its contents to the following:

```c#
using System.Runtime.Serialization;
using Innoactive.Hub.Training.Attributes;
using Innoactive.Hub.Training.Conditions;
using Innoactive.Hub.Training.SceneObjects;
using Innoactive.Hub.Training.SceneObjects.Properties;
using Newtonsoft.Json;

namespace Innoactive.Hub.Training.Template
{
    [DataContract(IsReference = true)]
    [DisplayName("Point at Object")]
    // Condition which is completed when Pointer points at Target.
    public class PointedCondition : Condition
    {
        [DataMember]
        // Reference to a pointer property.
        public ScenePropertyReference<PointingProperty> Pointer { get; private set; }

        [DisplayName("Target with a collider")]
        [DataMember]
        // Reference to a target property.
        public ScenePropertyReference<ColliderWithTriggerProperty> Target { get; private set; }

        [JsonConstructor]
        // Make sure that references are initialized.
        public PointedCondition() : this(new ScenePropertyReference<PointingProperty>(), new ScenePropertyReference<ColliderWithTriggerProperty>())
        {
        }

        public PointedCondition(ScenePropertyReference<PointingProperty> pointer, ScenePropertyReference<ColliderWithTriggerProperty> target)
        {
            Pointer = pointer;
            Target = target;
        }

        // This method is called when the step with that condition has completed activation of its behaviors.
        protected override void PerformActivation()
        {
            Pointer.Value.PointerEnter += OnPointerEnter;
            SignalActivationFinished();
        }

        // This method is called at deactivation of the step, after every behavior has completed its deactivation.
        protected override void PerformDeactivation()
        {
            Pointer.Value.PointerEnter -= OnPointerEnter;
            SignalDeactivationFinished();
        }

        // When a condition or behavior is fast-forwarded, the activation has to complete immediately.
        // This method should handle it, but since the activation is instanteneous,
        // It doesn't require any additional actions.
        protected override void FastForwardActivating()
        {
        }

        // When fast-forwarded, a conditions should complete immediately.
        // For that, the pointer fakes that it pointed at the target.
        protected override void FastForwardActive()
        {
            Pointer.Value.FastForwardPoint(Target);
        }

        // When a condition or behavior is fast-forwarded, the deactivation has to complete immediately.
        // This method should handle it, but since the deactivation is instanteneous,
        // It doesn't require any additional actions.
        protected override void FastForwardDeactivating()
        {
        }

        // When PointerProperty points at something,
        private void OnPointerEnter(ColliderWithTriggerProperty pointed)
        {
            // Ignore it if this condition is already fulfilled.
            if (IsCompleted)
            {
                return;
            }

            // Else, if Target references the pointed object, complete the condition.
            if (Target.Value == pointed)
            {
                MarkAsCompleted();
            }
        }
    }
}
```

All conditions should inherit from the `Condition` abstract class. To initialize a condition, implement the `PerformActivation()` method. This condition subscribes to a `PointerEnter` event of a referenced `PointingProperty`. When the Pointer points at the target, the condition will mark itself as complete. To deinitialize, implement the `PerformDeactivation()` method. In both methods, you have to call `SignalActivationFinished()` and `SignalDeactivationFinished()`, respectively.

Every condition should be able to complete immediately if `FastForwardActive()` method is called. In this case, we fake that the target was actually pointed at. To do so, we call the `FastForwardPoint()` method that we implemented in the previous chapter. Fast-forwarding allows us to load chapters, skip steps, and change modes.

## Custom behavior

Create new C# script named `ScalingBehavior` and change its contents to the following:

```c#
using System.Collections;
using System.Runtime.Serialization;
using Innoactive.Hub.Threading;
using Innoactive.Hub.Training.Attributes;
using Innoactive.Hub.Training.Behaviors;
using Innoactive.Hub.Training.SceneObjects;
using Newtonsoft.Json;
using UnityEngine;

namespace Innoactive.Hub.Training.Template
{
    // This behavior linearly changes scale of a Target object over Duration seconds, until it matches TargetScale.
    [DataContract(IsReference = true)]
    [DisplayName("Scale Object")]
    public class ScalingBehavior : Behavior
    {
        // Training object to scale.
        [DataMember]
        public SceneObjectReference Target { get; private set; }

        // Target scale.
        [DataMember]
        [DisplayName("Target Scale")]
        public Vector3 TargetScale { get; private set; }

        // Duration of the animation in seconds.
        [DataMember]
        [DisplayName("Animation Duration")]
        public float Duration { get; private set; }

        // A coroutine responsible for scaling the target.
        private IEnumerator coroutine;
        
        // Handle data initialization in the constructor.
        [JsonConstructor]
        public ScalingBehavior() : this(new SceneObjectReference(), Vector3.one, 0f)
        {
        }

        public ScalingBehavior(SceneObjectReference target, Vector3 targetScale, float duration)
        {
            Target = target;
            TargetScale = targetScale;
            Duration = duration;
        }
        
        // Called on activation of the training entity. Define activation logic here.
        // You have to call `SignalActivationFinished()` after you've done everything you wanted to do during the activation.
        protected override void PerformActivation()
        {
            // Start coroutine which will scale our object.
            coroutine = ScaleTarget();
            CoroutineDispatcher.Instance.StartCoroutine(coroutine);
        }

        // Called on deactivation of the training entity. Define deactivation logic here.
        // You have to call `SignalDeactivationFinished()` after you've done everything you wanted to do during the deactivation.
        protected override void PerformDeactivation()
        {
            SignalDeactivationFinished();
        }

        // This method is called when the activation has to be interrupted and completed immediately.
        protected override void FastForwardActivating()
        {
            // Stop the scaling coroutine,
            CoroutineDispatcher.Instance.StopCoroutine(coroutine);

            // Scale the target manually,
            Target.Value.GameObject.transform.localScale = TargetScale;

            // And signal that activation is finished.
            SignalActivationFinished();
        }
        
        // It requires no additional action.
        protected override void FastForwardActive()
        {
        }

        // Deactivation is instanteneous.
        // It requires no additional action.
        protected override void FastForwardDeactivating()
        {
        }
        
        // Coroutine which scales the target transform over time and then finished the activation.
        private IEnumerator ScaleTarget()
        {
            float startedAt = Time.time;

            Transform scaledTransform = Target.Value.GameObject.transform;

            Vector3 initialScale = scaledTransform.localScale;

            while (Time.time - startedAt < Duration)
            {
                float progress = (Time.time - startedAt) / Duration;

                scaledTransform.localScale = Vector3.Lerp(initialScale, TargetScale, progress);
                yield return null;
            }

            scaledTransform.localScale = TargetScale;

            SignalActivationFinished();
        }
    }
}
```

All behaviors should inherit from the `Behavior` class. Similarly to [conditions](#custom-condition), they should implement `PerformActivation()` and `PerformDeactivation()` methods, as well as means to fast-forward it. 

Note the major difference from the [condition](#custom-condition) example: instead of activating immediately, this behavior starts a coroutine and calls the `SignalActivationFinished()` method only at the end of it. It allows to create behaviors and conditions that take some time to activate or deactivate. 

> Use `CoroutineDispatcher.Instance.StartCoroutine(coroutine)` to start coroutines.

The other difference is that behaviors are simply idling when they are active. The actual work happens either at activation or deactivation.


## Shared considerations for behaviors and conditions

* The Innoactive Creator uses Newtonsoft.Json to serialize and preserve trainings. Note the `DataContract`, `DataMember`, and `JsonConstructor` attributes: they denote which properties of the condition are serialized and thus saved.
* If you make a behavior or condition to implement an `IOptional` interface, you will be able to skip it with training modes.
* If you want this condition to available to training designers, you have to adjust the [editor configuration](#editor-configuration) accordingly. The same is true for behaviors.
* Conditions and behaviors never reference training objects or properties directly: they use instances of `SceneObjectReference` and `ScenePropertyReference<TProperty>` classes instead. It makes trainings independent from scenes. References locate training objects by their unique names.

## Mode parameters

To customize a behavior or condition with mode parameters, declare a property of a `ModeParameter` type. It automatically fetches the training mode parameter by its `key`. If the current mode does not define the parameter, it uses the default value instead. You can subscribe to the `ParameterModified` event to handle the training mode change.

For example, you might create a giant glowing arrow and change its color depending on the current training mode.

See the following code snippet for the reference:

```c#
// Declare the property.
public ModeParameter<bool> IsShowingHighlight { get; private set; }

// Initialise the property (typically in constructor, at Awake() or OnEnable()).
protected void Initialise()
{
    // Create a new mode parameter that binds to a training mode entry with a key `ShowSnapzoneHighlight`. 
    // It expects the value to be a bool, and if it isn't defined, it uses `true` as a default value.
    IsShowingHighlight = new ModeParameter<bool>("ShowSnapzoneHighlight", true);
    
    // Perform necessary changes 
    IsShowingHighlight.ParameterModified += (sender, args) =>
    {
        HighlightObject.SetActive(IsShowingHighlight.Value);
    };
}
```

## Custom drawers

You can customize the way behaviors and conditions are displayed. To make a new drawer, you have to implement the `ITrainingDrawer` interface. If you want to use the drawer as a default drawer for all data members of a type, use `[DefaultTrainingDrawer(Type type)]` attribute. 

For an example, see `BehaviorDrawer` class.

Keep the following in mind:

* Drawers use Unity Editor IMGUI.
* Only one instance per drawer type is created. Do not store any information that is related to a specific object.
* Inherit from an `AbstractDrawer` instead of `ITrainingDrawer`, as it properly implements `ChangeValue` method and one of the `Draw` overloads.
* Pass the value assignment logic via `changeValueCallback` parameter of the `Draw` method.
* Never invoke that callback directly: either pass it to a child drawer, or call a `ChangeValue` with it as a parameter.
* Call `ChangeValue` only when the *current* member has changed, not one of its children.
* Call `ChangeValue` only when the current member *has* changed, because you will clutter Undo stack otherwise.

For example, that's how `ListDrawer` handles a new entry being added to it:

```c# 
// When user clicks "Add new item" button...
if (GUI.Button(rect, "Add"))
{
    // Define function that results in a list with a new element added.
    Func<object> getListWithAddedElement = () =>
    {
        InsertIntoList(ref list, list.Count, ReflectionUtils.GetDefault(entryDeclaredType));
        return list;
    };

    // Define function that results in a previous version of a list (with freshly added element removed back).
    Func<object> getListWithRemovedElement = () =>
    {
        RemoveFromList(ref list, list.Count - 1);
        return list;
    };

    // changeValueCallback contains the logic required to assign a value to the drawn property or field.
    // It takes a value that has to be assigned as its argument.
    // ChangeValue will create a new undoable command.
    // When it is performed, it invokes changeValueCallback with the result of getListWithAddedElement.
    // When it is reverted, it invokes changeValueCallback with the result of GetListWithRemovedElement.
    ChangeValue(getListWithAddedElement, getListWithRemovedElement, changeValueCallback);
}
```

And that's how it draws the elements of the list:

```c#
entry = listBeingDrawn[index];

// Define the action that has to be performed if that entry's value changes.
Action<object> entryValueChangedCallback = newValue =>
{
    // Assign new value to the entry.
    listBeingDrawn[index] = newValue;
    
    // Invoke the assignment logic of a parent drawer.
    changeValueCallback(list);
};

ITrainingDrawer entryDrawer = // Determine a drawer for the entry [out of scope of this example].

Rect entryRect = // Determine a rect for the entry [out of scope of this example].

// Use index as a label of the entry's view.
string label = "#" + index;

// Draw entry.
entryDrawer.Draw(entryRect, entry, entryValueChangedCallback, label);
```

## Custom overlay

To make your own controls define your own `Spectator Cam Prefab Overload` in [`[HUB-PLAYER-SETUP-MANAGER]`](http://docs.hub.innoactive.de/api/Innoactive.Hub.PlayerSetup.PlayerSetupManager.html) scene object. 

For reference, find the the prefab `AdvancedTrainerCamera` located in `IA-Training-Template/Resources/CustomCamera/Prefabs`. It replaces the default spectator camera in the `Advanced` scene. The child of this prefab is a custom overlay with `AdvancedTrainingController` script attached. Using this overlay, a trainer is able to see the current training status, start, reset, and mute the training, pick a chapter and skip a step, choose a language and the training mode to use.

This training controller loads the training course selected in the `Runtime Configurator` component on the game object called `[TRAINING_CONFIGURATION]` in the scene.

The localization files must be named by the two-letter ISO code of the respective language (for example, `en.json` or `de.json`). They have to be located at `[YOUR_PROJECT_ROOT_FOLDER]/Assets/StreamingAssets/Training/[COURSE_NAME]/Localization`. The script automatically loads all available localizations and displays them in the language dropdown menu. If there is no [respective language pack](#using-the-offline-windows-tts), the localization file is ignored. 
