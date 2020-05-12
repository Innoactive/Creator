# Update Behaviors and Conditions

This chapter will help you to rewrite behaviors and conditions designed for the Innoactive Creator v1.X to v2.0.

We changed our public API to make it easier to understand. The logic behind it stays the same, and all changes you have to introduce are trivial. From our own experience, it takes only five to fifteen minutes to refactor a behavior our condition.

## Checklist

While updating our own code, we have figured out a fast and easy way to do so. For every behavior and condition you have to update, take these steps in order:

1. [Update namespaces](#namespaces)
1. [Refactor stage processes' classes](#stage-process)
1. [Refactor the configurator](#configurator)
1. [Refactor the autocompleter](#autocompleter)
1. [Refactor menu items](#menu-items)
1. [Refactor the entity](#entity)

When we mention the `EntityData` class throughout this chapter, we refer to the data type which your entity uses.

## Namespaces

Replace references to `Innoactive.Hub.Training.Editors` to `Innoactive.CreatorEditor`. 

Replace references to `Innoactive.Hub.Training` to `Innoactive.Creator`. 

## Stage Process

The first change that we have introduced is renaming the stage process to just a process.

The second change is that the data object is passed to the stage process's constructor, and not directly to the method. You can access the data with the `Data` property of the abstract `Process<TData>` class.

### Replace base class/interface

If you have used `IStageProcess<TData>` before, inherit from `Process<TData>` instead. 

```csharp
// Before:
private class ActiveProcess : IStageProcess<EntityData> { /* Implementation */ }

// After:
private class ActiveProcess : Process<EntityData> { /* Implementation */ }
```

If you have used `BaseStageProcessOverCompletable<TData>`, use `BaseActiveProcessOverCompletable<TData>` instead. 

```csharp
// Before:
private class ActiveProcess : BaseStageProcessOverCompletable<EntityData> { /* Implementation */ }

// After:
private class ActiveProcess : BaseActiveProcessOverCompletable<EntityData> { /* Implementation */ }
```

### Add public constuctor

```csharp

// After:
public ActiveProcess(EntityData data) : base(data) { }
```

### Change methods signatures

If you have used the `IStageProcess<TData>` interface:

The Start method:

```csharp
// Before:
public void Start(EntityData data) { /* Implementation */ }

// After:
public override void Start() { /* Implementation */ }
```

The Update method:

```csharp
// Before:
public IEnumerator Update(EntityData data) { /* Implementation */ }

// After:
public override IEnumerator Update() { /* Implementation */ }
```

The End method:

```csharp
// Before:
public void End(EntityData data) { /* Implementation */ }

// After:
public override void End() { /* Implementation */ }
```

The FastForward method:

```csharp
// Before:
public void FastForward(EntityData data) { /* Implementation */ }

// After:
public override void FastForward() { /* Implementation */ }
```

If you have used `BaseStageProcessOverCompletable<TData>`, change the CheckIfCompleted() method signature:

```csharp
// Before:
protected override bool CheckIfCompleted(EntityData data) { /* Implementation */ }

// After:
protected override bool CheckIfCompleted() { /* Implementation */ }
```

### Reference the process's property, not the method argument

```csharp
// Before:
public void Start(EntityData data) 
{ 
    data.ExampleValue.DoSomething();    
}

// After:
public override void Start() 
{
    Data.ExampleValue.DoSomething();    
}
```

## Configurator

We changed configurators the same way we have changed stage processes: we pass the data instance through constructor now.

### Inherit from abstract generic class, not from the interface

```csharp
// Before:
private class EntityConfigurator : IConfigurator<EntityData> { /* Implementation */ }

// After:
private class EntityConfigurator : Configurator<EntityData> { /* Implementation */ }
```

### Change the method's signature

```csharp
// Before:
public void Configure(EntityData data, IMode mode, Stage stage) { /* Implementation */ }

// After:
public override void Configure(IMode mode, Stage stage) { /* Implementation */ }
```

### Add public constructor

```csharp
// After:
public EntityConfigurator(EntityData data) : base(data) { }
```

### Reference the configurator's property, not the method argument

```csharp
// Before:
public void Configure(EntityData data, IMode mode, Stage stage) 
{
    data.Value.Configure(mode);
}

// After:
public override void Configure(IMode mode, Stage stage) 
{
    Data.Value.Configure(mode);
}
```

## Autocompleter

Just like for stage processes and autocompleters, we pass the data instance in the constructor.

Also, you don not have to set the `ICompletableData.IsCompleted` flag to true manually anymore.

### Inherit from the abstract class

Inherit from `Autocompleter<EntityData>`, not `BaseAutocompleter<EntityData>` or `IAutocompleter<EntityData>`.

```csharp
// Before:
private class EntityAutocompleter : BaseAutocompleter<EntityData>

// After:
private class EntityAutocompleter : Autocompleter<EntityData>
```

### Add public constructor


```csharp
// After:
public EntityAutocompleter(EntityData data) : base(data) { }
```

### Change method's signature

```csharp
// Before:
public void Complete(EntityData data) { /* Implementation */ }

// After:
public override void Complete() { /* Implementation */ }
```

### Use the autocompleter's property, not the method argument

```csharp
// Before:
public void Complete(EntityData data) 
{
    data.Value.Set();
    // ...
}

// After:
public override void Complete() 
{
    Data.Value.Set();
}
```

### Do not call base.Complete() and do not set Data.IsCompleted to true

```csharp
// Before:
public void Complete(EntityData data) 
{
    /* Implementation */ 
    data.IsCompleted = true;
}

// After:
public override void Complete() 
{
    /* Implementation */ 
}
```

## Menu Items

In the old versions, we hid the menu item class inside the `Menu` class. It turned out that it was hard to find it both to humans and IDEs.

Also, we have changed the `DisplayedMenu` property's return type to the plain and simple `string`.

### Base class

For menu items for behaviors, use `Innoactive.CreatorEditor.UI.StepInspector.Menu.MenuItem<IBehavior>` as the base class.

For menu items for conditions, use `Innoactive.CreatorEditor.UI.StepInspector.Menu.MenuItem<ICondition>` as the base class.

### Property signature

Change `DisplayedName`'s return type to `string`.

### Property implementation

Just return a string instead of constructing a `GUIContent` object.

## Entity

All what has left to refactor is the entity itself.

We replaced properties with methods which should return a new instance of an entity's component each time.

Also, we have removed the `Process` property and replaced it with methods for each of the three stages.

### Reference processes

As the `Process` property is gone, assign stage processes  with `GetActivatingProcess()`, `GetActiveProcess()`, and `GetDeactivatingProcess()` methods:

```csharp
// After:
public override IProcess GetActiveProcess()
{
    return new ActiveProcess(Data);
}
```

By default, these methods return empty processes.

### Reference configurators

Replace the `Configurator` property with `GetConfigurator()` method. You do not have to use an instance of `BaseConfigurator` anymore.

```csharp
// After:
protected override IConfigurator GetConfigurator()
{
    return new EntityConfigurator(Data);
}
```

### Reference autocompleters

Replace the `Autocompleter` property with `GetAutocompleter()` method.

```csharp
protected override IAutocompleter GetAutocompleter()
{
    return new EntityAutocompleter(Data);
}
```