# Managing Training Courses

## Training Runner

You have to handle training courses in your application more explicitly in the Innoactive Creator 1.0: in addition to the `Activate()` method, the external code has to invoke the `Update()` and `Configure()` methods, too. You can use the new `TrainingRunner` static class to handle it automatically: it executes one training course at time, binds to the changes in the runtime configuration, and exposes few convenience methods to control the current course. To use it, you have to pass the training course to it by calling the `Initialize(ICourse)` method. It will create a game object that binds to the `RuntimeConfigurator.ModeChanged` event and calls `ICourse.Update()` every frame. Then you can start the execution of the course by calling `Run()`. If you need to skip one or more chapters you can use the `SkipChapters(int)` method. To skip a step, use the `SkipStep(ITransition)` method, where the method argument is the transition to follow. The `Current` property returns the current course.

## Loading a Training Course

We made it easier for training designers to manage the training courses: the training editor manages training courses files automatically. A designer can pick a course to run or edit in the inspector view of the `[TRAINING_CONFIGURATION]` scene object. As a template developer, you can set the directory to save training courses to by overriding the `IEditorConfiguration.DefaultCourseStreamingAssetsPath`. The target directory has to be located in the `Streaming Assets` folder of the Unity project. Use relative paths: if you want to store training courses in the `[PATH_TO_PROJECT]/Assets/StreamingAssets/Example`, you have to set the property to `Example`. The default configuration uses the `Training` subdirectory.

Then you can call the `RuntimeConfigurator.Configuration.LoadCourse()` to get an instance of a training course. The default implementation loads the training course that was selected in the `[TRAINING_CONFIGURATION]` game object, but you can override this method to do it in a different way.

> A training designer can always use `Innoactive > Setup Current Scene as a Training Scene` option in the Unity toolbar to add the `[TRAINING_CONFIGURATION]` to the scene.

When you program a custom training course loader, you need only three lines of code to load the selected course, deserialize, and launch it:

```csharp
ICourse trainingCourse = RuntimeConfigurator.Configuration.LoadCourse();
TrainingRunner.Initialize(trainingCourse);
TrainingRunner.Run();
```