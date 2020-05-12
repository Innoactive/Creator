# Overview of the Changes

This chapter compares the public Innoactive API v2.0 to v1.3.1, and highlights the most important additions and chagnes between them.

## Unity Version

We start supporting Unity 2019.4 LTS and discontinue our support of Unity 2017.4 LTS and Unity 2018 LTS. This way we can develop our product faster and test it better.

## Serializer

Now you can replace the default training serializer with your own, as long as it supports `DataContract` and `DataMember` attributes of `System.Xml.Serialization`. The default implementation still uses NewtonsoftJson.

## Dependencies

We made the Innoactive Creator independent from our old product and from a number of external projects. The core package weights eleven times less now.

## Components

We have extracted built-in behaviors, conditions, and properties of the Innoactive Creator into separate components. It makes the Innoactive Creator more flexible and speeds up our releasing process. For this version, we ship two template packages that include everything you need: one for VRTK and the other for Unity XR.

## Abstracted VR

With the previous change, we moved VR interactions into separate, interchangeable components. The integration with VRTK is a separate components, as well as the new component for Unity XR. You can now write your own component to support any VR framework you want, too.

The new recommended VR framework is Unity XR. We have updated the VRTK component to the new API, but this is our last release for it, as we drop our support for it from now on. While integrating Unity XR into our tool, we have collected our insights for working with it. You can find these insights in our [FAQ](03-unity-xr-faq.md) chapter.

## Standalone Devices Support

The Innoactive Creator supports standalone devices now. 

*If you use the default course serializer, go to `Unity > Edit > Project Settings... > Player > Other Settings`. Set API compitability settings to .NET 4.x.*

## Scene Setup

Instead of overriding `SetupTrainingScene()` method of the `DefaultRuntimeConfiguration`, you should inherit from the `Innoactive.Creator.Utils.SceneSetup` class. Now any component or template can setup the scene independently from others.

## Runtime Configuration

We have moved logging and training mode handling out of the runtime configuration to separate classes.

## Assemblies

Unity supports separate assemblies now and we made use of it to make some of our code `internal`. As we had to keep them public, we had to keep that part of our API unchanged. That was yielding no real value for you, but was troublesome for us.

## Namespaces

We changed the namespaces from `Innoactive.Hub.Training` to `Innoactive.Creator` and from `Innoactive.Hub.Training.Editors` to `Innoactive.CreatorEditor`. This way, we removed remnants of the old name of our product to match our code to our branding.

## Entities

We have refactored the public API of entities (behaviors and conditions). While it still reflects the established concepts, it makes easier to understand and implement them. We provide a [separate chapter](02-update-behaviors-and-conditions.md) on how to update your behaviors and conditions to this version. Alternatively, you can skim though the updated [developer's tutorial](../developer/index.md).