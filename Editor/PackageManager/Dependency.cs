using System;

namespace Innoactive.CreatorEditor.PackageManager
{
    /// <summary>
    ///
    /// </summary>
    public abstract class Dependency
    {
        /// <summary>
        ///
        /// </summary>
        public virtual string Package { get; } = "";

        /// <summary>
        /// Priority lets you tweak in which order different <see cref="SceneSetup"/>s will be performed.
        /// The priority is considered from lowest to highest.
        /// </summary>
        public virtual int Priority { get; } = 0;

        /// <summary>
        ///
        /// </summary>
        public event EventHandler<EventArgs> OnPackageEnabled;

        /// <summary>
        ///
        /// </summary>
        public event EventHandler<EventArgs> OnPackageDisabled;

        /// <summary>
        ///
        /// </summary>
        internal bool IsEnabled
        {
            get => isEnabled;
            set
            {
                if (isEnabled != value)
                {
                    if (value)
                    {
                        EmitOnEnabled();
                    }
                    else
                    {
                        EmitOnDisabled();
                    }
                }

                isEnabled = value;
            }
        }

        private bool isEnabled;

        protected virtual void EmitOnEnabled()
        {
            OnPackageEnabled?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void EmitOnDisabled()
        {
            OnPackageDisabled?.Invoke(this, EventArgs.Empty);
        }
    }
}
