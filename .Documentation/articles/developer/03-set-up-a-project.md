# Set up a Template Project

Before you could start working on your custom template, you have to set up a new Unity project for it. This chapter guides you on how to do it. Follow the instructions:

1. [Initial Setup](#initial-setup)
1. [Import the Innoactive Creator](#import-the-innoactive-creator)
1. [Import dependencies](#import-dependencies)

## Initial Setup

Follow the [Setup Guides](../setup-guides/index.md) to prepare a new Unity project. Open the new project in the Unity Editor and proceed to the next section.

## Import the Innoactive Creator

You can find the `Innoactive Creator` core and its components at our [Developer Resources](http://developers.innoactive.de/creator/releases/). You need the following packages:

1. Creator Core
1. Text-To-Speech Component
1. Basic Conditions and Behaviors Component
1. Basic Interaction Component
1. Unity XR Interaction component

Alternatively, you can set up a Git repository for your project and submodule these packages. Go to the `Assets` folder and open Git BASH there. Run the following command to initialize the repository:

```
git init
```

To submodule the packages, run the following commands:

```
git submodule add git@github.com:Innoactive/Creator.git Innoactive/Creator/Core
git submodule add git@github.com:Innoactive/TextToSpeech-Component.git Innoactive/Creator/Components/TextToSpeech-Component
git submodule add git@github.com:Innoactive/Basic-Conditions-And-Behaviors.git Innoactive/Creator/Components/Basic-Conditions-And-Behaviors-Component
git submodule add git@github.com:Innoactive/Basic-Interaction-Component.git Innoactive/Creator/Components/Basic-Interaction
git submodule add git@github.com:Innoactive/XR-Interaction-Component.git Innoactive/Creator/Components/XR-Interaction-Component
```

## Import Dependencies

The Innoactive Creator uses Unity XR for VR interactions. Depending on which VR headsets you use, you need to import appopriate SDKs. See instructions in our [Unity XR FAQ](../transition-to-v2.0/03-unity-xr-faq.md).

Some VR headsets require additional software. For example, if you use SteamVR SDK, you would need to install SteamVR itself, too. Refer to the headset's manufacturer instructions.

[To the next chapter!](04-general-concepts.md)
