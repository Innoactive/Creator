# Menu Items

If you click on an "Add Behavior" or "Add Condition" button in the Step Inspector, it will display a list of available options. If you select one of them, it will add a new behavior or condition to the step.

We have created a fully functional behavior in the previous chapter, but we still miss it in the list. "Add Behavior" and "Add Condition" buttons do not display behaviors or conditions directly. Instead, they display menu items. A menu item defines the label to display and the way how it creates a new entity for a step. Training designers would be able to use our behavior only after we create a menu item for it.

## Implementation for Behaviors

A menu item is an instance of a class that inherits from `MenuItem` inside the `Innoactive.CreatorEditor.UI.StepInspector.Menu` namespace. A menu item class is an Editor script, so you must create a file for it under the [Editor](https://docs.unity3d.com/Manual/SpecialFolders.html) subfolder of the `Assets` folder. 

The `MenuItem` class is generic. Use `IBehavior` as the generic parameter. Once the class compiles, the Step Inspector will find it on its own.

The base class declares one property and one method that you have to implement. 

The Step Inspector uses the `DisplayedName` property as a label. This property returns a `string`. If you use forward slashes ("`/`") in it, the Step Inspector will split it into submenus.

When a user selects one of the items, the Step Inspector calls the `GetNewItem()` method and adds the result to the list of entities.

Call the new file `ScalingBehaviorMenuItem.cs` and copy the following:

```csharp
using Innoactive.Creator.Core.Behaviors;
using Innoactive.Creator.BasicTemplate.Behaviors;
using Innoactive.CreatorEditor.UI.StepInspector.Menu;

public class ScalingBehaviorMenuItem : MenuItem<IBehavior>
{
    public override string DisplayedName 
    {
        get 
        { 
            return "Example Behaviors/Scale Object"; 
        } 
    }

    public override IBehavior GetNewItem()
    {
        return new ScalingBehavior();
    }
}
```

If you have named your behavior in a different way or placed it under a namespace, adjust the code accordingly.

Open the Unity Editor, let the changes compile, and then click "Add Behavior" button in the Step Inspector. You will see the menu item in the list.

You can create multiple menu items for a single behavior. If you modify the behavior's data in the `GetNewItem()` method, you will effectively create different presets of it.

## Implementation for Conditions

If you want to create a menu item for a condition, you need to change the generic parameter of the class and the return type of the `GetNewItem()` method to `ICondition` type. Other than that, menu items for conditions work in the exact same way.

## Allowed Menu Items Settings

Sometimes you would want to hide some of the menu items in the project: for example, if you have created a custom highlight behavior and you want training designers to use it instead of the default one. To do so, override the `AllowedMenuItemsSettingsAssetPath` property of the `DefaultEditorConfiguration` class. The value is a relative path from the project root folder to a file. The Innoactive Creator will create a JSON file at that path.

```csharp
public class CustomEditorConfiguration : DefaultEditorConfiguration
{
    public override string AllowedMenuItemsSettingsAssetPath
    {
        get { return "Assets/My Wanted Path/allowed-menu-items-config.json"; }
    }
}
```

This JSON file stores display settings for all menu items in the project. You can modify it through `Innoactive > Developer > Allowed Menu Items Settings`.
                         
![Open Allowed Menu Items Settings](../images/menu-items/open-allowed-menu-items-settings.png "Open Allowed Menu Items Settings")

Toggle off a menu item to hide it from training designers. This setting will persist even if you remove the menu item from the project and add it later again. This window locates menu items by their full type name: if you rename the class or move it to another namespace, this setting will lose track of it.

![Edit Allowed Menu Items Settings](../images/menu-items/edit-allowed-menu-items-settings.png "Edit Allowed Menu Items Settings")

### Create Menu Content Manually

If you need a higher level of control, you can override `BehaviorsMenuContent` or `ConditionsMenuContent` of the `DefaultEditorConfiguration`. There you can construct a list of available menu items manually.

[To the next chapter!](07-run-a-course.md)
