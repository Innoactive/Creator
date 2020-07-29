# Dependency Manager

The Innoactive Creator contains multiple components that depend on [Unity packages](https://docs.unity3d.com/Manual/PackagesList.html). It would be bothersome to include every package manually, so we have implemented our dependency manager tool. It retrieves all required dependencies from the [Unity Package Manager](https://docs.unity3d.com/Manual/Packages.html) and enables them automatically. This way, our components are ready to use as soon as you include them in your project.

This chapter explains how you can use the dependency manager for your components and templates.

## Usage

The dependency manager collects all classes that inherit from the `Dependency` type and orders them by their `Priority` property value. Then it queues the dependencies and enables them one by one. Each dependency refers to one and only one `Unity package` through its `Package` property. Multiple dependencies can refer to the same package. The dependency manager will then import it only once. Inherit from the `Dependency` class and override the `Package` and `Priority` properties:

```csharp
using Innoactive.CreatorEditor.PackageManager;

/// Adds Unity's Post-Processing package as a dependency.
public class PostProcessingPackageEnabler : Dependency
{
    /// A string representing the package to add.
    public override string Package { get; } = "com.unity.postprocessing";

    /// Priority lets you tweak in which order different dependences should resolve.
    /// The package manager goes from lowest to highest priority.
    public override int Priority { get; } = 10;
}
```

### Package

The `Package` property indicates the [official package name](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@1.8/manual/index.html#viewing-package-details).

### Priority

`Priority` is a numeric value. The package manager resolves dependencies with lower values first.

This concludes the template developer's documentation. Further topics are to be done.