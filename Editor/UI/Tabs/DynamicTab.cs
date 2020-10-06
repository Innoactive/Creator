using System;
using UnityEngine;

namespace Innoactive.CreatorEditor.Tabs
{
    /// <summary>
    /// This <inheritdoc cref="ITab"/> implementation defines <seealso cref="GetValue"/> and <seealso cref="SetValue"/> implementation with delegates through the constructor.
    /// </summary>
    public class DynamicTab : ITab
    {
        /// <inheritdoc/>
        public virtual GUIContent Label { get; }

        /// <inheritdoc/>
        public object GetValue()
        {
            return getter();
        }

        /// <inheritdoc/>
        public void SetValue(object value)
        {
            setter(value);
        }

        /// <inheritdoc/>
        public void OnSelected() { }

        /// <inheritdoc/>
        public void OnUnselect() { }

        private readonly Func<object> getter;
        private readonly Action<object> setter;

        /// <param name="label">A label to display.</param>
        /// <param name="getter"><seealso cref="GetValue"/> implementation.</param>
        /// <param name="setter"><seealso cref="SetValue"/> implementation.</param>
        public DynamicTab(GUIContent label, Func<object> getter, Action<object> setter)
        {
            Label = label;
            this.getter = getter;
            this.setter = setter;
        }
    }
}
