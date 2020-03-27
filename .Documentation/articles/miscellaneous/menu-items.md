# Menu Items

When a training designer clicks the "Add Behavior" or "Add Condition" button a list of available options is displayed. The `Innoactive Creator` searches for menu items through reflection: the list is automatically extended with each new class that inherits from `Menu.Item<IBehavior>` or `Menu.Item<ICondition>`.

Example menu item:
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

The `DisplayedName` property can be used to group the menu items: the items with the names `Group/Behavior` and `Group/Another Behavior` create the submenu called `Group` that contains the `Behavior` and `Another Behavior` options.

![Grouped menu items](../images/menu-items/group-menu-items.png "Grouped Menu Items")

Then you have to return an actual behavior or condition in the `GetMenuItem()` method. You can perform additional setup here: for example, you can create two menu items for the same behavior that use different constructor parameters.

## Allowed Menu Items Settings

In some cases you would want to hide some of the menu items in the project: for example, if you have created a custom highlight behavior and you don't want training designers to use the default one. To be able to do so, you have to override the `AllowedMenuItemsSettingsAssetPath` property when you inherit from the `DefaultEditorConfiguration`. It has to be a relative path from `Assets`.

Example:
```csharp
public class OurOwnEditorConfiguration : DefaultEditorConfiguration
{
    public override string AllowedMenuItemsSettingsAssetPath
    {
        get { return "Assets/My Wanted Path/allowed-menu-items-config.json"; }
    }
}
```

It will create a text asset at the specified path that can be edited through `Innoactive > Creator > Developer > Allowed Menu Items Settings`:
                         
![Open Allowed Menu Items Settings](../images/menu-items/open-allowed-menu-items-settings.png "Open Allowed Menu Items Settings")

There you can exclude some menu items from being displayed:

![Edit Allowed Menu Items Settings](../images/menu-items/edit-allowed-menu-items-settings.png "Edit Allowed Menu Items Settings")

The settings will persist even if you remove the menu item from the project. Note that they use the full type name of a menu item to store the entry: it means that if you rename it or change its namespace the settings will lose track of it.

## Manually Created Menu Content

You can achieve any level of control by overriding any of the `DefaultEditorConfiguration` properties: you can create `AllowedMenuItemsSettings` programmatically instead of loading it from a text asset, or you can even set up the `BehaviorsMenuContent` or `ConditionsMenuContent` manually.

