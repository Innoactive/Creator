# Run a Course

## Training Runner

The Innoactive Creator's API includes the `TrainingRunner`, a helper class that handles execution of courses. If you use this class, it will take you three lines of code to load and start a course.

It is up to you to write a script that will call that code. You could create a graphic interface for trainers, or bind a button on a keyboard or VR controller to start the training process.

## Usage

The most basic way is to start the course as soon as a user launches the application.

We can create a [Unity script](https://docs.unity3d.com/Manual/CreatingAndUsingScripts.html) that will trigger as soon as the scene loads.

Create a  `TrainingCourseLoader.cs` file in the `Assets` folder. Replace its contents, if any, with the following:

```csharp
using System.Collections;
using Innoactive.Hub.Training;
using Innoactive.Hub.Training.Configuration;
using UnityEngine;

// Unity components' classes should have the same names as their files.
public class TrainingCourseLoader : MonoBehaviour
{
    private void Start()
    {
        // Load the selected training course.
        ICourse trainingCourse = RuntimeConfigurator.Configuration.LoadCourse();

        // Pass the course to the training runner.
        TrainingRunner.Initialize(trainingCourse);

        // Start the course.
        TrainingRunner.Run();
    }
}
```

`RuntimeConfigurator.Configuration.LoadCourse()` creates a course from the file that the training designer has selected in the runtime configuration, in the `[TRAINING_CONFIGURATION]` game object.

We have described the `[TRAINING_CONFIGURATION]` game object in the [designer's getting started guide](../getting-started/designer.md). You can refresh you memory [here](../innoactive-creator/training-configuration.md).

`TrainingRunner.Initialize(ICourse)` loads a given course into the training runner. The training runner can handle only one course at time.

`TrainingRunner.Run()` starts to execute the course.

Create a new game object on your scene and attach this script to it. Now it will launch the course as soon as you run the application.

Test your behavior with the training course you have created in the previous chapter.

## Other API Members

You could use the `Current` property to get the loaded course, and `IsRunning` to see if it is currently executing or not. 

Use the `SkipChapters(int)` method to skip next chapters. `SkipChapter(1)` will skip only the current chapter. 

You can skip current step with the `SkipStep(ITransition)` method, but you need to specify the transition to use.

[To the next chapter!](08-conditions.md)