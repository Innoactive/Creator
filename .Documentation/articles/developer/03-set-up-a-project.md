# Set up a Template Project

Before you could start working on your custom template, you have to set up a new Unity project for it. This chapter guides you on how to do it. Follow the instructions:

1. [Check your OS](#check-your-os)
1. [Download Unity](#download-unity)
1. [Create a new template project](#create-a-new-template-project)
1. [Import dependencies](#import-dependencies)
1. [Import the Innoactive Creator](#import-the-innoactive-creator)
1. [Check your setup](#check-your-setup)

## Check Your OS

The Innoactive Creator supports only Windows 10. We use Microsoft Text-to-Speech, and you need to install a language package for every language you use. You can do it in the `Windows Settings > Time & Language > Language`.

## Download Unity

Now install Unity 2018.4.

You can get it with the [Unity Hub](https://docs.unity3d.com/Manual/GettingStartedUnityHub.html), a standalone application that manages your Unity projects and editors's installations. Follow the [guide](https://docs.unity3d.com/Manual/GettingStartedInstallingHub.html) to install both the Unity Hub and the Unity Editor 2018.4.

Alternatively, you can download Unity 2018.4 from the [LTS Releases page](https://unity3d.com/unity/qa/lts-releases?version=2018.4).

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

## Import Dependencies

The Innoactive Creator uses VRTK 3.3.0 for VR interactions. Depending on which VR headsets you use, you need to import appopriate SDKs. Use download links from the VRTK [documentation](https://vrtoolkit.readme.io/v3.3.0/docs/summary).

Once you have got a required package, import it into your project. To do so, select `Assets > Import Package > Custom Package...` in the Unity Editor's menu bar.

![Import Unity Package](../images/import-unity-package.png "How to import a custom package.")

The file explorer window will appear. Locate the package you want to import. Once you select it, an `Import Unity Package` dialog will appear. Select `All` and then `Import`.

![Import Unity Package dialog](../images/developer/steamvr-import.png "Import Unity Package dialog.")

Some VR headsets require additional software. For example, if you use SteamVR SDK, you would need to install SteamVR itself, too. Refer to the headset's manufacturer instructions.

## Import the Innoactive Creator

You can find the `Innoactive Creator` unity package at our [Developer's Portal](http://developers.innoactive.de/components/#training-module). Find the latest release in the `Innoactive Creator` section, click `Download`, and select the `innoactive-creator-vX.Y.Z.unitypackage` file, where the `vX.Y.Z` is the latest version. Import it in the same way as we have described in the previous [section](#import-dependencies).

![Innoactive Creators](../images/training-modules.png "Innoactive Creator section.")

## Check Your Setup

Follow this checklist to make sure that everything is set as required:

1. You use Windows 10;
1. You use Unity of version 2018.4;
1. You have your VR headset's SDK inside the `Assets` folder of the project. Its version is compatible with the VRTK v3.3.0.
1. You have the `Assets\Extensions\Innoactive\Creator` folder inside your project.

[To the next chapter!](04-general-concepts.md)