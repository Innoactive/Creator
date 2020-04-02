# The Transition Guide from v1.X to v2.0

This guide is for developers who have used the Innoactive Creator v1.X and wish to try out the preview release of the Innoactive Creator v2.0. You will learn about the upcoming changes and how to convert your code to the new API.

We have released the preview version to collect your feedback before making the changes final. Please, send it to your contact person at Innoactive. Of course, it means that the public API could change once more when we make the actual release.

We have extended the Innoactive Creator with new features and made it easier to use. Every concept that you have learned still applies to the new version.

We recommend to update your templates and develop new training applications with v2.0 when we will make it available. We recommend to still use v1.3.1 for existing application projects.

# Overview of The Changes

## Unity Version

We are not supporting Unity 2017/2018 anymore. Use Unity 2019.3 for this release. As soon as Unity Technologies releases Unity 2019.4 LTS, we will support only this version. It helps us to focus development and quality assurance of our product.

## Serializer

You can replace the default training serializer with your own, as long as it supports `System.Xml.Serialization` attributes `DataContract` and `DataMember`. 

The default implementation still uses NewtonsoftJson.

## Components

We have extracted some parts of the Innoactive Creator package into separate components. For now, we ship two template packages that include everything you need. We will provide detailed instructions on how to configure components in the actual release.

## Unity XR and VRTK Support

The previous change allowed us to remove hard dependency to VRTK and add Unity XR support. We will release VRTK component with v2.0, but we will not support it anymore. The new recommended VR framework is Unity XR.

You can write your own component to support any VR framework you want.

## Standalone Devices Support

The Innoactive Creator supports standalone devices now.

## Loading Training Courses

Runtime configurations load training courses asyncroniously now. We needed it for the support of standalone devices. You could use it, for example, to load courses over the Internet.

## Scene Setup

Instead of overriding `SetupTrainingScene()` method of the `DefaultRuntimeConfiguration`, you can inherit from the `Innoactive.Creator.Utils.SceneSetup`. This way, we can implement a separate setup for each component or template that requires it.

## Runtime Configuration

We have moved logging and training mode handling to separate classes.

## Assemblies

Unity supports separate assemblies now and we made use of it to make some of our code `internal`. We had to keep them public and keep that part of our API unchanged. It was extra work that had no real value for you.

## Namespaces

We changed the namespaces from `Innoactive.Hub.Training` to `Innoactive.Creator` and from `Innoactive.Hub.Training.Editors` to `Innoactive.CreatorEditor`. This way, we removed remnants of the old name of our product.

## Entities

We have refactored the public API of entities (behaviors and conditions). While it still reflects the established concepts, it makes easier to understand and implement them. We provide a separate chapter on how to update your behaviors and conditions to this version.


# Refactor Entities

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