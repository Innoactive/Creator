# Dependency Manager

The Creator works as a distributed network of components. Each component handles one specific functionality. Due to the nature of some components, they sometimes depend on some [Unity Packages](https://docs.unity3d.com/Manual/PackagesList.html) that are not enabled by default. Without them, the component will not work. This is a problem. That is why we introduced a system that allows us to automatically retrieve all required dependencies from the [Unity Package Manager](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@1.8/manual/index.html). By using a `Dependency Manager` we ensure the `Creator` to be as standalone as possible.

## Overview

The `Dependency Manager` guarantees that all required `Unity packages` are enabled in the project. It works automatically at the startup and it only adds missing dependencies.

### Dependency

A `Dependency` can be considered as a `Unity package` that is required for any of the `Creator`\`s components or templates. Without it, the component or template would not work.

### Usage

The `Dependency Manager` collects all `Dependency` type classes in the project and orders them according to their `Priority` property value. Once all dependencies are collected and prioritized, the `Dependency Manager` queues them to be enabled one by one in an asynchronous process. Each dependency enables a single `Unity package` in the `Package Manager`.

To make the `Dependency Manager` automatically add a new dependency or validate, if the dependency is enabled in the project, it is necessary to create a new class inheriting from the `Dependency` class and overriding their properties `Package` and `Priority`.

```csharp
using Innoactive.CreatorEditor.PackageManager;

/// Adds Unity's Post-Processing package as a dependency.
public class PostProcessingPackageEnabler : Dependency
{
    /// A string representing the package to be added.
    public override string Package { get; } = "com.unity.postprocessing";

    /// Priority lets you tweak in which order different dependences will be performed.
    /// The priority is considered from lowest to highest.
    public override int Priority { get; } = 10;
}
```

#### Package

The _Package_ property indicates the [official package name](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@1.8/manual/index.html#viewing-package-details).

#### Priority

The _Priority_ is a numeric value that allows the `Dependency Manager` to determine the installation order for the dependencies.


> Know more about the [`Package Manager`](https://docs.unity3d.com/Manual/Packages.html).
