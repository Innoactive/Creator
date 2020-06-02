# General Concepts

As you have learned in the chapter [Using the Innoactive Creator](02-using-innoactive-creator.md), each VR training application guides its user through a training course. 

Every training course is split in chapters. The application executes chapters in order, one chapter at time.

Each chapter has a number of steps, which connect to each other with transitions. Together they form the chapter's workflow, which may include branches and loops.

Steps have behaviors, and transitions have conditions.

Conditions define what a trainee must do to proceed to the next step: for example, touch or grab an object. Behaviors define what an application must do for the trainee: for example, play a sound or highlight something.

As a template developer, you only need to program new behaviors and conditions and let the Innoactive Creator to handle the rest.

Training courses and their elements inherit from the same class of `Entity`. We will take a closer look at it in this chapter: it will be easier for you to implement your own behaviors and conditions if you know when and how the Innoactive Creator will invoke your code.

## Entity

Every element of a training course is an entity. 

An entity is a composition of a few smaller, isolated components. The most notable of them are the entity's life cycle, process, and data.

During the execution, every entity passes through a number of stages. An entity's life cycle makes sure that its entity changes the stages in a valid way. All entities use the same life cycle class.

Each stage has a stage process. When an entity enters a new stage, it starts executing its process. A process cannot affect the entity or its life cycle: it can only read and modify the entity's data.

The data object can store values of various origin: you could retrieve values from the current configuration or scene, or you could make a field that a training designer would then edit.

You only need to define the process and data: we have included the life cycle implementation in the base `Entity` class.

## Life Cycle

This section reveals the internal mechanics of the Innoactive Creator. Most likely, you will never work with them directly.

Still, this is a system that will manage your behaviors and conditions. An understanding of it will serve as a basis both for the rest of this tutorial and for your daily work with our product.

### Stages

There are four stages of an entity's life cycle:

1. Inactive;
1. Activating;
1. Active;
1. Deactivating.

Entities start in the Inactive stage, and pass through all stages in order, one stage at time. 

After an entity completes the Deactivating stage, it will become Inactive again.

Stages cannot be omitted.

[![Life cycle stages](../images/developer/life-cycle-stages.png)](../images/developer/life-cycle-stages.png "An diagram that describes the same as the text above.")

An entity may go through its life cycle multiple times: for example, a training designer could define a loop in a chapter's workflow: a step in that loop would then run several times.

All entities use the same life cycle class with the same universal rules. We could group these rules in three sets: the rules about stage processes, the rules about switching between stages, and the rules about fast-forwarding.

The rest of this section explains these rules.

### Switching between Stages

In essense, the Activating and Deactivating stages rely on the status of their processes, and Inactive and Active stages wait for you to call the respective public methods. 

[![Life cycle stages with triggers](../images/developer/life-cycle-stages-with-triggers.png)](../images/developer/life-cycle-stages-with-triggers.png "An illustration for the text above.")

This is a full set of rules that entities use to switch between stages:

1. If an entity is Inactive and you invoke the `Activate()` method then it will start Activating.
1. If an entity is Activating, it will proceed to the Active stage as soon as the process is over.
1. If an entity is Activating and you call `Deactivate()`, it will act normally until it enters Active stage.
1. If an entity is Active, it will idle after the process is over.
1. If an entity is Active and you call `Deactivate()` method then it will proceed to the Deactivating stage even if it had no time to finish the current process.
1. If an entity became Active and you have called `Deactivate()` during the Activating stage, it will proceed to the Deactivating stage immediately.
1. If an entity is Deactivating, it will proceed to the Inactive stage as soon as the process is over.

### Fast-Forwarding

The last feature of the life cycle is fast-forwarding. With the Innoactive Creator you can, for example, skip a step or a whole chapter. The catch is that nothing is truly skipped: instead, an entity fakes the natural way of its execution. For now, we will only describe when you can fast-forward an entity; the [Process section](#process) explains how to do it.

If you mark the current stage to fast-forward, it will immediately complete the process. If you mark any other stage to fast-forward, it will do the same as soon as the entity enters that stage.

When an entity leaves a stage, it removes the fast-forwarding mark from that stage.

You can mark individual stages with `MarkToFastForwardStage(Stage)`. If you invoke the `MarkToFastForward()` method, the life cycle will mark all stages that an entity has to pass to become Inactive.

For example, if an entity is in Active stage, the `MarkToFastForward()` method will mark Active and Deactivating stages.

Fast-forwarding affects only processes; the state-switching rules remain unchanged. Even if you fast-forward an Active stage process, the life cycle will still wait until `Deactivate()` is called.

This system ensures that an application will always process every stage of every entity in a proper succession.

### Summary

The diagram below summarizes this section.

[![Life cycle of an entity](../images/developer/life-cycle.png)](../images/developer/life-cycle.png "A diagram that describes an entity's life cycle and its expected behavior.")

## Stage Processes

When an entity enters a new stage it starts an associated stage process. Stage processes carry out various tasks and may take longer than one frame to complete. Together, they define the logic of the entity. For example, if you have a behavior that moves an object around the scene, a stage process will change the position of that object. If you have a condition, a stage process will check for its requirements every frame.

Often, one or two stage processes of an entity are empty: they do not contain any logic and complete immediately. The stage process for Inactive stage is always empty.

Each stage process implements four methods: `Start()`, `Update()`, `Fast-Forward()`, and `End()`. Every time an entity enters a new stage, its life cycle starts to invoke the stage process's methods, as we will describe below.

#### Start()

The life cycle calls the `Start()` method as soon as it enters a new stage.

The invocation of this method is guaranteed.

You could use this method for initialization.

#### Update()

Starting from the next frame, the life cycle iterates over the `Update()` method. `Update()` is an [IEnumerator](https://docs.microsoft.com/en-us/dotnet/api/system.collections.ienumerator)
that works in a way similar to [Unity coroutines](https://docs.unity3d.com/Manual/Coroutines.html): it is being called once per frame and continues its execution from the last `yield return` statement.

Use `return yield null;` to wait for the next frame.

Use `yield break;` to stop iterating.

Note that this method is not compatible with Unity coroutines, for example, the `WaitForSeconds` coroutine.

#### FastForward()

Sometimes a stage process has to complete immediately. In such cases the life cycle stops iterating over the `Update()` method, and calls `FastForward()`.

`FastForward()` should fake the normal execution of `Update()`. For example, if we have a behavior that moves an object to some point, in this method we have to assign the final position to the object, as it would happen if we would iterate over `Update()` completely.

#### End()

The life cycle calls the `End()` method either when it has iterated `Update()` completely, or if it has called `FastForward()`.

The invocation of this method is guaranteed.

You could use this method for deinitialization.

#### Empty Methods

Sometimes, you would leave some methods of a stage process empty. For example, you would need to implement only the `Start()` method if a stage process must happen immediately. If a stage process has nothing to initialize or deinitialize, you would leave `Start()` and `End()` empty. Empty `FastForward()` methods are very common.

#### Summary

The diagram below summarizes this subsection.

[![Stage Process Execution](../images/developer/stage-process.png)](../images/developer/stage-process.png "A diagram that shows the execution order of the methods of a stage process.")

## Data

An entity stores its state in its data object. It includes data that a training designer would save as a part of a course, and the values that are set during the runtime and discarded afterwards.

The process knows only about the data, but not about the entity or the entity's life cycle. It helps to keep things simple.

[To the next chapter!](05-behaviors.md)