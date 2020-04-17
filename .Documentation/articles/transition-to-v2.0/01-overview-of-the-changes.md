# Overview of the Changes

This chapter lists most important changes and additions to the Innoactive Creator. We will provide full documentation with the actual release of v2.0.

## Unity Version

We start to support Unity 2019 and stop supporting Unity 2017 or 2018. 

Use Unity 2019.3 with this release. As soon as Unity Technologies releases Unity 2019.4 LTS, we will support only that version. It helps us to focus development and quality assurance of our product.

## Serializer

You can replace the default training serializer with your own, as long as it supports `DataContract` and `DataMember` attributes of `System.Xml.Serialization`.

The default implementation still uses NewtonsoftJson.

## Components

We have extracted built-in behaviors, conditions, and properties of the Innoactive Creator into separate components. It makes the Innoactive Creator more flexible and speeds up our releasing process. 

For now, we ship two template packages that include everything you need. We will provide detailed instructions on how to configure components in the actual release.

## Dependencies

We made the Innoactive Creator independent from our legacy code and from a number of external projects.

## Abstracted VR

The previous change allowed us to remove hard dependency to VRTK and integrate our product with Unity XR. We will release VRTK component with v2.0 but we will not support it anymore. The new recommended VR framework is Unity XR. Check our [FAQ](03-unity-xr-faq.md).

You can write your own component to support any VR framework you want.

## Standalone Devices Support

The Innoactive Creator supports standalone devices now. 

*If you use the default course serializer, go to `Unity > Edit > Project Settings... > Player > Other Settings`. Set API compitability settings to .NET 4.x.*

## Scene Setup

Instead of overriding `SetupTrainingScene()` method of the `DefaultRuntimeConfiguration`, you can inherit from the `Innoactive.Creator.Utils.SceneSetup`. This way, we can implement a separate setup for each component or template that requires it.

## Runtime Configuration

We have moved logging and training mode handling to separate classes.

## Assemblies

Unity supports separate assemblies now and we made use of it to make some of our code `internal`. We had to keep them public and keep that part of our API unchanged. It was extra work that was yielding no real value for you.

## Namespaces

We changed the namespaces from `Innoactive.Hub.Training` to `Innoactive.Creator` and from `Innoactive.Hub.Training.Editors` to `Innoactive.CreatorEditor`. This way, we removed remnants of the old name of our product.

## Entities

We have refactored the public API of entities (behaviors and conditions). While it still reflects the established concepts, it makes easier to understand and implement them. We provide a [separate chapter](02-update-behaviors-and-conditions.md) on how to update your behaviors and conditions to this version.