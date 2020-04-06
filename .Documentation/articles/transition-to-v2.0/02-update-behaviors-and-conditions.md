# Update Behaviors and Conditions

This chapter will help you to rewrite behaviors and conditions designed for the Innoactive Creator v1.X to v2.0.

We changed APIs, but kept the business logic behind them. There are plenty of changes you have to make, but all of them are trivial.

Estimated time of refactoring is 5-15 minutes per entity.

## Checklist

Use this order while refactoring your behaviors and conditions. This is the fastest way to do it.

1. Update used namespaces
1. Refactor stage processes' classes
1. Refactor the configurator
1. Refactor the autocompleter
1. Refactor menu items
1. Refactor the behavior/condition

## Update used namespaces

Replace references to `Innoactive.Hub.Training.Editors` to `Innoactive.CreatorEditor`. Replace references to `Innoactive.Hub.Training` to `Innoactive.Creator`. You can skip this step if your IDE manages references automatically.

## Stage Process

Throughout this chapter, the `EntityData` refers to the data type which your entity uses.

### Replace base class/interface

If you have used `IStageProcess<TData>` before, inherit from `Process<TData>` instead:

Before:

```csharp
private class ActiveProcess : IStageProcess<EntityData> { /* Implementation */ }
```

After:

```csharp
private class ActiveProcess : Process<EntityData> { /* Implementation */ }
```

If you have used `BaseStageProcessOverCompletable<TData>`, use `BaseActiveProcessOverCompletable<TData>` instead.

Before:

```csharp
private class ActiveProcess : BaseStageProcessOverCompletable<EntityData> { /* Implementation */ }
```

After:

```csharp
private class ActiveProcess : BaseActiveProcessOverCompletable<EntityData> { /* Implementation */ }
```

### Add public constuctor

After:

```csharp
public ActiveProcess(EntityData data) : base(data) { }
```

### Change methods signatures

If you have used the `IStageProcess<TData>` interface:

The Start method before:

```csharp
public void Start(EntityData data) { /* Implementation */ }
```

After:

```csharp
public override void Start() { /* Implementation */ }
```

The Update method before:

```csharp
public IEnumerator Update(EntityData data) { /* Implementation */ }
// ->
```

After:

```csharp
public override IEnumerator Update() { /* Implementation */ }
```

The End method before:

```csharp
public void End(EntityData data) { /* Implementation */ }
```

After:

```csharp
public override void End() { /* Implementation */ }
```

The FastForward method before:

```csharp
public void FastForward(EntityData data) { /* Implementation */ }
```

After:

```csharp
public override void FastForward() { /* Implementation */ }
```

If you have used `BaseStageProcessOverCompletable<TData>`:


The CheckIfCompleted() method before:

```csharp
protected override bool CheckIfCompleted(EntityData data) { /* Implementation */ }
```

After:

```csharp
protected override bool CheckIfCompleted() { /* Implementation */ }
```

### Reference the process's property, not the method argument

Before:

```csharp
public void Start(EntityData data) 
{ 
    data.ExampleValue.DoSomething();    
}
```

After:

```csharp
public override void Start() 
{
    Data.ExampleValue.DoSomething();    
}
```

## Configurators

### Inherit from abstract generic class, not from the interface

Before:

```csharp
private class EntityConfigurator : IConfigurator<EntityData> { /* Implementation */ }
```

After:

```csharp
private class EntityConfigurator : Configurator<EntityData> { /* Implementation */ }
```

### Change the method's signature

Before:

```csharp
public void Configure(EntityData data, IMode mode, Stage stage) 
{
    /* Implementation */ 
}
```

After:

```csharp
public override void Configure(IMode mode, Stage stage) 
{
    /* Implementation */ 
}
```

### Add public constructor

After:

```csharp
public EntityConfigurator(EntityData data) : base(data) { }
```

### Reference the configurator's property, not the method argument

Before:

```csharp
public void Configure(EntityData data, IMode mode, Stage stage) 
{
    data.Value.Configure(mode);
}
```

After:

```csharp
public override void Configure(IMode mode, Stage stage) 
{
    Data.Value.Configure(mode);
}
```

## Autocompleter

### Inherit from the abstract class

Inherit from `Autocompleter<EntityData>`, not `BaseAutocompleter<EntityData>` or `IAutocompleter<EntityData>`

Before:

```csharp
private class EntityAutocompleter : BaseAutocompleter<EntityData>
```

After:

```csharp
private class EntityAutocompleter : Autocompleter<EntityData>
```

### Add public constructor

After:

```csharp
public EntityAutocompleter(EntityData data) : base(data) { }
```

### Change method's signature

Before:

```csharp
public void Complete(EntityData data) 
{
    /* Implementation */ 
}
```

After:

```csharp
public override void Complete() 
{
    /* Implementation */ 
}
```

### Use the autocompleter's property, not the method argument

Before:

```csharp
public void Complete(EntityData data) 
{
    data.Value.Set();
    // ...
}
```

After:

```csharp
public override void Complete() 
{
    Data.Value.Set();
}
```

### Do not call base.Complete() and do not set Data.IsCompleted to true

Before:

```csharp
public void Complete(EntityData data) 
{
    /* Implementation */ 
    data.IsCompleted = true;
}
```

After:

```csharp
public override void Complete() 
{
    /* Implementation */ 
}
```

## Menu items

### Base class

Use `Innoactive.CreatorEditor.UI.StepInspector.Menu.MenuItem<IBehavior>` or `Innoactive.CreatorEditor.UI.StepInspector.Menu.MenuItem<ICondition>` as the base class.

### Property signature

Replace `DisplayedName`'s return type with string.

### Property implementation

Just return string.

## Refactor entity

### Reference processes

No more the `Process` property. Stage processes assigned separately with `GetActivatingProcess()`, `GetActiveProcess()`, `GetDeactivatingProcess()` methods:

```csharp
public override IProcess GetActiveProcess()
{
    return new ActiveProcess(Data);
}
```

By default, these methods return empty processes.

### Reference configurators

The `Configurator` property is replaced with `GetConfigurator()` method. The usage of `BaseConfigurator` is not required anymore.

### Reference autocompleters

The `Autocompleter` property is replaced with `GetAutocompleter()` method.