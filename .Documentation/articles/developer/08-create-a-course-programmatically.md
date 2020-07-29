# Create a Course Programmatically

## Entity Factory

The `EntityFactory` is a helper class that allows you to dynamically create basic elements of a course.

It supports the creation of steps (`IStep`), transitions (`ITransition`), chapters (`IChapter`), and courses (`ICourse`).

Additionally, the `EntityFactory` provides a built-in post-processing stack for the recently built elements.

## Usage

It takes only a line of code to create a course element, and one more line to assign it to the course:

```csharp
using UnityEngine;
using Innoactive.Creator.Core;

// Unity components' classes should have the same names as their files.
public class TrainingCourseBuilder : MonoBehaviour
{
    public ICourse CreateDefaultCourse()
    {
        // Create a new ICourse.
        ICourse course = EntityFactory.CreateCourse("New Course");
        
        // Create a new IChapter.
        IChapter chapter = EntityFactory.CreateChapter("Chapter 1");
        
        // Create a new IStep.
        IStep step = EntityFactory.CreateStep("New Step");
        
        // Create a new ITransition.
        ITransition transition = EntityFactory.CreateTransition();
        
        // Add the transition to the step.
        step.Data.Transitions.Data.Transitions.Add(transition);
        
        // Add the step to the chapter.
        chapter.Data.Steps.Add(step);
        
        // Add the chapter to the course.
        course.Data.Chapters.Add(chapter);

        return course;
    }
}
```

By default, all steps and courses created with the `EntityFactory` already contain a default transition and a chapter. we explicitly show how to create and add them to a course in the example above for reference purposes.

## Entity PostProcessing

You might want to execute some actions over freshly created entities. You can do it by extending the the `EntityPostProcessing <T>` class, where T corresponds to the type of the element you want to post-process.

Every time the `EntityFactory` creates a course element, it executes the post-processing stack of actions.

The following example hooks a class that automatically logs analytics data for every new course created:

```csharp
using Innoactive.Creator.Core;
using CompanyProvider.AnalyticsService;

public class CourseAnalyticsPostProcessing : EntityPostProcessing<ICourse>
{
    public override void Execute(ICourse course)
    {
        AnalyticsTracker tracker = new AnalyticsService.CreateTracker();
        tracker.LogEvent(new AnalyticsEvent(){Category = "Training Courses", Action = "Course Creation", Label = course.Data.Name});
    }
}
```

The following example hooks a class that automatically fills the description field for all new steps:

```csharp
using Innoactive.Creator.Core;

public class StepsDescriptionPostProcessing : EntityPostProcessing<IStep>
{
    public override void Execute(IStep entity)
    {
        entity.Data.Description = "Step created dynamically.";
    }
}
```

[To the next chapter!](09-conditions.md)