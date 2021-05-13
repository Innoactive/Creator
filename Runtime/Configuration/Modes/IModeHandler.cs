using System;
using System.Collections.ObjectModel;

namespace VPG.Creator.Core.Configuration.Modes
{
    /// <summary>
    /// Interface for the ModeHandler used to configure modes during runtime.
    /// </summary>
    public interface IModeHandler
    {
        /// <summary>
        /// The event that has to be invoked when the current mode changes, for example in <see cref="SetMode"/> method.
        /// </summary>
        event EventHandler<ModeChangedEventArgs> ModeChanged;

        /// <summary>
        /// The ordered collection of all available training modes.
        /// </summary>
        ReadOnlyCollection<IMode> AvailableModes { get; }

        /// <summary>
        /// The index of the current training mode.
        /// </summary>
        int CurrentModeIndex { get; }

        /// <summary>
        /// The current training mode.
        /// </summary>
        IMode CurrentMode { get; }

        /// <summary>
        /// Set the current training mode.
        /// </summary>
        /// <param name="index">The index of the desired training mode.</param>
        void SetMode(int index);

        /// <summary>
        /// Set the current training mode, this training mode has to be one of the available modes.
        /// </summary>
        /// <param name="mode">The desired training mode which should be set.</param>
        void SetMode(IMode mode);
    }
}
