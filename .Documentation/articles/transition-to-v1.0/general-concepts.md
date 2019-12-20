# Introduction

At the end of September we released the first stable version of the Innoactive Creator. While working on it, we have refactored the module's 
core: the code that defines and executes training courses. The reason behind it is that now we have a clearer vision of our product than 
one year ago: initially, the core logic was supporting only simple linear sequences; over time, it evolved into a sophisticated system with 
branching and cycles that can reconfigure or skip individual parts of the training course in numerous ways and combinations. Every part of 
a training course: a chapter, step, transition, behavior, condition (and the course itself) inherit from the same `Entity` class through 
deep inheritance trees. We ended up with a monolithic, tangled up code: we had to keep the whole system in mind when working on individual 
parts of it.

In version 1.0, there is still the `Entity` base class, but we have split it into multiple small and isolated ones, flattened its 
inheritance tree, and addressed a few smaller annoyances. The final result is easier to explain, maintain, and extend.

# General Concepts

## Entity

As mentioned before, everything is an entity. Each entity consists of its life cycle, data, process, and configurator. The life cycle 
handles transition between different stages, each of which has its own part of the entity's process. The process has no access to its 
entity; only to its data. The same is true for the configurator, where you can adjust the data according to the current training mode. The 
data itself doesn't have any business logic and stores the state of an entity.

For example, when the life cycle of highlight behavior enters the `Activating` stage, it calls a process that paints a scene object in some 
color. Both the reference to a scene object and the current color's value are stored in the behavior's data. When a trainer changes the 
current training mode, it calls the behavior's configurator, and it changes target color to a new one.

All entities have their life cycles governed by the same set of rules: that's why you can't override this property or extend the 
`LifeCycle` class. You implement a new entity (a behavior or a condition) by defining its data, process, and configurator.

## Life Cycle

You don't have to work with life cycles directly unless you implement a behavior that manages other behaviors; still, it is useful to know 
the system that manages the code you work on.

The life cycle determines the current `Stage` and controls the transition between them. There are four stages: *Inactive*, *Activating*, 
*Active*, and *Deactivating*. Stages can be changed only in that order, going back to *Inactive* after *Deactivating*. 

A life cycle can change to the next stage only when the current process is completed. If the current stage is *Activating* or 
*Deactivating*, the life cycle proceeds to the next stage automatically. Otherwise, it waits for `Activate()` (for *Inactive*) and 
`Deactivate()` (for *Active*) methods to be called.

Processes could be instantaneously finished by calling `MarkToFastForward()` and `MarkToFastForward(Stage)` methods. The first one 
fast-forwards all processes until the entity becomes *Inactive*; the second one skips the process of a given stage. Every Innoactive Creator 
feature that bypasses a normal workflow uses these two methods: when you "skip" an entity, it is not actually skipped, but completed in a 
single frame. The same is true for optional behaviors: mode they are still activated and deactivated even when disabled by the current 
training. When you choose a chapter, the course executes all the previous ones. Note that these methods only affect the processes: the life 
cycle in the `Active` state still waits for `Deactivate()` to be called.

Note that all of entity's logic is supposed to be implemented through its processes, and a process has no access to its entity: it means 
that entity can't modify its own life cycle. This is an intentional limitation: allowing so was enabling error-prone solutions.

## Process

The processes have replaced `PerformActivation()` and `PerformDeactivation()` methods from previous versions of the Innoactive Creator: in a 
process you define what the entity has to do: for example, the process of a behavior that moves an object modifies the position of its 
transform.

## Stage Process

The entity's process consists of four independent stage processes: one per stage, where the process for the `Inactive` stage is predefined 
to be empty. Each stage process implements four methods: `Start()`, `End()`, `Update()`, and `Fast-Forward()`. All of them take the 
instance of the data as an input parameter.

The life cycle calls the `Start()` method immediately after entering a new stage. Starting from the next frame, it starts iterating over
the `Update()` method. The `Update()` method is an [IEnumerator](https://docs.microsoft.com/en-us/dotnet/api/system.collections.ienumerator)
that works similar to [Unity Coroutines](https://docs.unity3d.com/Manual/Coroutines.html) in that it is being called once per frame and 
continues its execution from the last `yield return` statement. After the `Update()`'s iteration has completed, the life cycle calls the 
`End()` can proceed to the next stage.

As described in previous chapter, there are cases when a stage process has to be completed immediately. In such cases the life cycle stops 
iterating over the `Update()` method, and calls the `FastForward()` and `End()` methods of the processes immediately.

The `Start()` and `End()` methods are guaranteed to be called, which makes them a good place to write initialization and deinitialization 
logic. The `Update()` method is good for actions that have to be done every frame; by calling `return yield null;` you extend the stage by 
one frame. Use `FastForward()` to handle the case when `Update()` won't complete its iteration: for example, set the object's position to 
its target destination there. In many cases the `FastForward()` method would be left empty.

## Configurator

As a template developer, you can define a list of training modes. A trainer could then switch between them during the runtime to tweak how 
the training course currently behaves (for example, to make it easier or harder for the trainee). The configurator has the same function as 
the old `Configure()` method of the entity: it is called when the current training mode has changed. When you implement it, you have access 
to the current mode, stage, and data.

## Data

An entity stores its state in its data and exposes to its process and configurator. This includes serialized data that is stored when a 
training designer saves a training course and the values that are set during runtime and discarded afterwards. The configurator and process 
of an entity can only affect its data, but not the entity itself.

## Completable Entity

Some entities (for example conditions), can be completed during their `Active` stage. The completion is not required: for example, a step 
is deactivated when one of its transitions has all its conditions completed. It means that when a step has two transitions, one set of 
conditions is completed during their lifecycle and the other is not. When you implement the fast-forwarding of such an entity, do not 
automatically complete it there. Instead, define this logic in the entity's autocompleter.

## Autocompleter

An autocompleter has a single `Complete()` method. It takes only data that implements `ICompletableData` interface. You have to set its 
`IsCompleted` property to `true` and adjust the rest of data to resemble what you would get when the entity complete normally: for example, 
if a condition is completed when the target is inside a collider, you might update the target's position in this method.