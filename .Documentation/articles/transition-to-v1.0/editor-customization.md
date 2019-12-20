# Editor Customization

## Menu Items

When a training designer clicks at an "Add Behavior" or "Add Condition" button, it displays a list of available options. In version 0.9 you had to manage this list manually; now we had reverted it to the older implementation that looks up for menu items through reflection: when you declare a class that inherits from `Menu.Item<IBehavior>` or `Menu.Item<ICondition>` the list will be automatically extended.

The `DisplayedName` property can be used to group the menu items: the items with the names `Group/Behavior` and `Group/AnotherBehavior` create the submenu that contains the `Behavior` and `AnotherBehavior` options.

Then you have to return an actual behavior or condition in the `GetMenuItem()` method. You can perform additional setup here: for example, you can create two menu items for the same behavior that use different constructor parameters.

```csharp
public class MoveObjectMenuItem : Menu.Item<IBehavior>
{
    public override GUIContent DisplayedName
    {
        get
        {
            return new GUIContent("Move Object");
        }
    }

    public override IBehavior GetNewItem()
    {
        return new MoveObjectBehavior();
    }
}
```

## Allowed Menu Items Settings

In some cases you would want to hide some of the menu items in the project: for example, if you have created a custom highlight behavior and you don't want training designers to use the default one. To be able to do so, you have to override the `AllowedMenuItemsSettingsAssetPath` property when you inherit from the `DefaultEditorConfiguration`. It will create a text asset at the specified path that can be edited through `Innoactive > Creator > Developer > Allowed Menu Items Settings`:
                         
![Open Allowed Menu Items Settings](../images/menu-items/open-allowed-menu-items-settings.png "Open Allowed Menu Items Settings")

There you can exclude some menu items from being displayed:

![Edit Allowed Menu Items Settings](../images/menu-items/edit-allowed-menu-items-settings.png "Edit Allowed Menu Items Settings")

The settings will persist even if you remove the menu item from the project. Note that they use the full type name of a menu item to store the entry: it means that if you rename it or change its namespace then the settings will lose the track of it.

## Virtual Members

You can achieve any level of control by overriding any of the `DefaultEditorConfiguration` properties: you can create `AllowedMenuItemsSettings` programmatically instead of loading it from a text asset, or you can set up the `BehaviorsMenuContent` or `ConditionsMenuContent` manually (just like in the version 0.9).