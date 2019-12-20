# Unity Setup

## What is Unity

Unity is a popular cross-platform game engine and the preferred platform for creating immersive experiences. The Innoactive Creator is build on top of Unity, which allows fast content creation for template developers. It also abstracts all Unity logic, so training designers can concentrate on creating training courses.

## How to set up Unity

### Unity Hub

There are several Unity versions and ways to download and install them. We recommend to download the `Unity Hub` from [unity3d.com](https://unity3d.com/get-unity/download).

The Unity Hub is a standalone application that streamlines the process of finding, downloading, and managing your Unity Projects and installations. Once the Unity Hub is installed and launched for first time, it might ask you to activate a unity license, which will require you to login using a Unity account. If you don't have one, you can create a new one for free.

>Learn more about the [Unity Hub](https://docs.unity3d.com/Manual/GettingStartedUnityHub.html).

### Download Unity using the Unity Hub

![Unity Hub Installs](../images/unity-setup/unity-hub-installs-panel.png "Unity Hub - Installs")

As we just installed the Unity Hub, the `Projects` and `Installs` sections should be empty.

The `Installs` section shows the Unity versions currently installed and their status while they're being downloaded. After installation you can modify platform modules, show the installation directory or uninstall Unity version.

> We suggest you to use `Unity 2018.4 (LTS)`.

In order to install Unity go to `Installs`, then `Add`, and select `Unity 2018.4 (LTS)`. Continue with the install wizard leaving the configuration at its defaults.

![Add Unity Version](../images/unity-setup/choose-unity-version.png "Add Unity Version")

> If you already have a version of Unity installed that is not shown in the `Installs` section, you can link it to Unity Hub by going to `Installs`, then `Locate` and selecting the `Unity.exe` of your Unity version.

### Create a project using the Unity Hub

![Unity Hub Projects](../images/unity-setup/unity-hub-projects-panel.png "Unity Hub - Projects")

Once we have at least one Unity version installed, we can load a project or create a new one.

For creating a new project, go to `Projects`, then `New`, a new window will pop up showing the basic configuration for new projects. Select 3D from the list of templates, use `New Unity Project` as project name and set a location where to save your project. When you are done with your configuration, select `Create` and Unity will open a new project based on your configuration.

Choose a very short path to the project location (less than 28 symbols): otherwise, Unity might fail to load the Innoactive Creator project. If you need a longer project path, see [the appendix](#appendix).

> Selecting the caret symbol next to `New` will display a dropdown menu with the Unity versions installed. This allows to select a Unity version for the new project.

![Unity Project Settings](../images/unity-setup/setup-unity.project.png "New Unity project configuration")

> If you already have a Unity project that you want to load, you can go in Unity Hub to `Projects`, then `Add`, and locate the project. Unity will add the project to the `Projects` section.

Every Unity project opened using Unity Hub will be added to a list that populates the `Projects` section. If the project that you want to use is already on this list, you can just select it and it will open automatically.

![Unity Hub Project List](../images/unity-setup/unity-hub-list-of-projects.png "Projects list")

---

## API compatibility level

The `Innoactive Creator` requires `.Net API compatibility level` to be set to `.NET 4.X`. Unity by default uses a different configuration. Importing the `Innoactive Creator` without properly setting `.NET 4.X` in Unity will return the following error:

![.Net API compatibility level error](../images/unity-setup/net-api-level-error.png "Incompatible .Net API level")

>We recommend to set `.NET 4.X` as `Api Compatibility Level` before importing the `Innoactive Creator` to avoid any error.

### How to set .Net API compatibility level to .NET 4.X in Unity

The `Player Settings` panel allows you to set various options for the final application built by Unity.

In order to open the `Player Settings` panel, select `Edit` > `Project Settings` > `Player Settings`.

The `Player Settings` panel contains up to 6 different sections (variations depend on the platform target). Select `Other Settings` and then look for `Api Compatibility Level*`. Change it to `.NET 4.X`.

![Player Settings Panel](../images/unity-setup/player-settings-other-api-level.png "Api Compatibility Level - .Net 4.x")

>Learn more about [`Player Settings`](https://docs.unity3d.com/Manual/class-PlayerSettings.html)

## Appendix

### Workaround for long project paths

If it is not possible to have a total project path with not more than 28 symbols, assign a drive letter to its root folder. Do the following:

1. Press Win+R.
2. Type `cmd.exe` in and press enter.
3. Type in the following command, where `z` is a free drive letter:

`subst z: C:\path\to\your\project`

Use backslashes (` \ `) and not the forward slashes (` / `) as a directory separator. 
