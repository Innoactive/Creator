# The Transition Guide from 1.X to 2.0

This guide is for developers to rewrite behaviors and conditions designed for the Innoactive Creator 1.X to 2.0.

We changed APIs, but kept logic. There are plenty of changes you have to make, but they all are trivial.

Estimated time of refactoring per entity is 5-15 minutes.

## Checklist

Use this order while refactoring your behaviors and conditions. This is the fastest way to do it.

1. Refactor stage processes' classes
2. Refactor the configurator
3. Refactor the autocompleter
4. Refactor menu items
5. Refactor the behavior/condition

## Stage Process

`EntityData` refers to the data type which your process uses.

### Replace base class/interface

If you have used `IStageProcess<TData>` before, inherit from `Process<TData>` instead:

```csharp
private class ActiveProcess : IStageProcess<EntityData> { /* Implementation */ }

// ->

private class ActiveProcess : Process<EntityData> { /* Implementation */ }
```

If you have used `BaseStageProcessOverCompletable<TData>`, use `BaseActiveProcessOverCompletable<TData>` instead.

```csharp
private class ActiveProcess : BaseStageProcessOverCompletable<EntityData> { /* Implementation */ }

// ->

private class ActiveProcess : BaseActiveProcessOverCompletable<EntityData> { /* Implementation */ }
```

### Add public constuctor

```csharp
public ActiveProcess(EntityData data) : base(data) { }
```

### Change methods signatures

If you have used the `IStageProcess<TData>` interface:

```csharp
public void Start(EntityData data) { /* Implementation */ }
// ->
public override void Start() { /* Implementation */ }
```

```csharp
public IEnumerator Update(EntityData data) { /* Implementation */ }
// ->
public override IEnumerator Update() { /* Implementation */ }
```

```csharp
public void End(EntityData data) { /* Implementation */ }
// ->
public override void End() { /* Implementation */ }
```

```csharp
public void FastForward(EntityData data) { /* Implementation */ }
// ->
public override void FastForward() { /* Implementation */ }
```

If you have used `BaseStageProcessOverCompletable<TData>`:

```csharp
protected override bool CheckIfCompleted(EntityData data) { /* Implementation */ }
// ->
protected override bool CheckIfCompleted() { /* Implementation */ }
```

### Reference the process's property, not the method argument

```csharp
public void Start(EntityData data) 
{ 
    data.ExampleValue.DoSomething();    
}
// ->
public override void Start() 
{
    Data.ExampleValue.DoSomething();    
}
```

## Configurators

### Inherit from abstract generic class, not from the interface

```csharp
private class EntityConfigurator : IConfigurator<EntityData> { /* Implementation */ }

// ->

private class EntityConfigurator : Configurator<EntityData> { /* Implementation */ }
```

### Change the method's signature

```csharp
public void Configure(EntityData data, IMode mode, Stage stage) 
{
    /* Implementation */ 
}
```

```csharp
public override void Configure(IMode mode, Stage stage) 
{
    /* Implementation */ 
}
```

### Add public constructor

### Reference the configurator's property, not the method argument

## Autocompleter

### Inherit

Inherit from `Autocompleter<EntityData>`, not `BaseAutocompleter<EntityData>` or `IAutocompleter<EntityData>`

```csharp
private class EntityAutocompleter : BaseAutocompleter<EntityData>
```

to

```csharp
private class EntityAutocompleter : Autocompleter<EntityData>
```

### Add constructor

### Change method's signature

### Use the autocompleter's property, not the method argument

### Do not call base.Complete() and do not set Data.IsCompleted to true

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

The `Configurator` property is replaced with `GetConfigurator`. The usage of `BaseConfigurator` is not required anymore.

### Reference autocompleters

The `Autocompleter` property is replaced with `GetAutocompleter()`.