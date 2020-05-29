# Set up a Template Project

Before you could start working on your custom template, you have to set up a new Unity project for it. This chapter guides you on how to do it. Follow the instructions:

1. [Check your OS](#check-your-os)
1. [Download Unity](#download-unity)
1. [Create a new template project](#create-a-new-template-project)
1. [Import the Innoactive Creator](#import-the-innoactive-creator)
1. [Import dependencies](#import-dependencies)
1. [Check your setup](#check-your-setup)

## Check Your OS

The Innoactive Creator supports only Windows 10. We use Microsoft Text-to-Speech, and you need to install a language package for every language you use. You can do it in the `Windows Settings > Time & Language > Language`.

## Download Unity

Now install Unity 2019.3.

You can get it with the [Unity Hub](https://docs.unity3d.com/Manual/GettingStartedUnityHub.html), a standalone application that manages your Unity projects and editors' installations. Follow the [guide](https://docs.unity3d.com/Manual/GettingStartedInstallingHub.html) to install both the Unity Hub and the Unity Editor 2019.3.

Alternatively, you can download Unity 2019.3 from the [Releases page](https://unity.com/releases/2019-3).

Either way, you will have to login into your Unity account or create a new one. You will need an appropriate Unity [license](https://store.unity.com/).

## Create a New Template Project

To create a Unity project for your template, launch the Unity Hub, then go to the `Projects` tab and click the `New` button.

![Unity Hub Projects](../images/unity-setup/unity-hub-projects-panel.png "An empty `Projects` section of the Unity Hub")

A new window will appear. Choose a name for your project and the folder where Unity should create it. Choose `3D` as a project template and click `Create`. 

Note that Unity project templates simply define the initial settings of the project and have nothing in common with Innoactive Creator templates.

![Unity Project Settings](../images/unity-setup/setup-unity.project.png "Setting up a new project in the Unity Hub.")

The Unity Hub will add the new project to the `Projects` section. You can always open your projects from there.

![Unity Hub Project List](../images/unity-setup/unity-hub-list-of-projects.png "A populated `Projects` section of the Unity Hub.")

Now, open your project in the Unity Editor and proceed to the next section.

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
git submodule add git@github.com:Innoactive/Creator.git Innoactive/Creator
git submodule add git@github.com:Innoactive/TextToSpeech-Component.git Innoactive/TextToSpeech-Component
git submodule add git@github.com:Innoactive/Basic-Conditions-And-Behaviors.git Innoactive/Basic-Conditions-And-Behaviors-Component
git submodule add git@github.com:Innoactive/Basic-Interaction-Component.git Innoactive/Basic-Interaction
git submodule add git@github.com:Innoactive/XR-Interaction-Component.git Innoactive/XR-Interaction-Component
```

## Import Dependencies

The Innoactive Creator uses Unity XR for VR interactions. Depending on which VR headsets you use, you need to import appopriate SDKs. See instructions in our [Unity XR FAQ](../transition-to-v2.0/03-unity-xr-faq.md).

Some VR headsets require additional software. For example, if you use SteamVR SDK, you would need to install SteamVR itself, too. Refer to the headset's manufacturer instructions.

[To the next chapter!](04-general-concepts.md)