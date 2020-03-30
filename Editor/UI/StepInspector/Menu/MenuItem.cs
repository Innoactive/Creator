namespace Innoactive.CreatorEditor.UI.StepInspector.Menu
{
    /// <summary>
    /// The Step Inspector populates "Add Behavior" and "Add Condition" buttons' dropdown menus with implementations of this class.
    /// Use either <seealso cref="IBehavior"/> or <see cref="ICondition"/> as the generic parameter.
    /// The Step Inspector will display it with <see cref="get_DisplayedName"/>.
    /// If clicked, it will use the result of <see cref="GetNewItem"/>.
    /// </summary>
    /// <typeparam name="T">A type of an object to create.</typeparam>
    public abstract class MenuItem<T> : MenuOption<T>
    {
        /// <summary>
        /// Returns a new instance of an object (behavior or condition).
        /// </summary>
        public abstract T GetNewItem();

        /// <summary>
        /// A name displayed in the Step Inspector.
        /// </summary>
        public abstract string DisplayedName { get; }
    }
}
