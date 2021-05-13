namespace VPG.CreatorEditor.UI.StepInspector.Menu
{
    /// <summary>
    /// This class adds a separator in the "Add Behavior"/"Add Condition" dropdown menus.
    /// </summary>
    public sealed class MenuSeparator<T> : MenuOption<T>
    {
        /// <summary>
        /// The submenu where separator will be displayed.
        /// </summary>
        public string PathToSubmenu { get; }

        public MenuSeparator(string pathToSubmenu = "")
        {
            PathToSubmenu = pathToSubmenu;
        }
    }
}
