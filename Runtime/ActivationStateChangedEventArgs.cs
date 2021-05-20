using System;

namespace VPG.Core
{
    /// <summary>
    /// Event that is fired when the current stage changes.
    /// </summary>
    public class ActivationStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// New stage.
        /// </summary>
        public readonly Stage Stage;

        public ActivationStateChangedEventArgs(Stage stage)
        {
            Stage = stage;
        }
    }
}
