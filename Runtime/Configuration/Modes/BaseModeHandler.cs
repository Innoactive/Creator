using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VPG.Creator.Core.Exceptions;

namespace VPG.Creator.Core.Configuration.Modes
{
    /// <summary>
    /// Simple ModeHandler.
    /// </summary>
    public sealed class BaseModeHandler : IModeHandler
    {
        /// <inheritdoc />
        public event EventHandler<ModeChangedEventArgs> ModeChanged;

        /// <inheritdoc />
        public int CurrentModeIndex { get; private set; }

        /// <inheritdoc />
        public IMode CurrentMode
        {
            get { return AvailableModes[CurrentModeIndex]; }
        }

        /// <inheritdoc />
        public ReadOnlyCollection<IMode> AvailableModes { get; }

        public BaseModeHandler(List<IMode> modes, int defaultMode = 0)
        {
            AvailableModes = new ReadOnlyCollection<IMode>(modes);
            CurrentModeIndex = defaultMode;
        }

        /// <inheritdoc />
        public void SetMode(int index)
        {
            if (AvailableModes.Count == 0)
            {
                throw new MissingModeException("You cannot access the current training mode index because there are no training modes available.");
            }

            if (CurrentModeIndex >= AvailableModes.Count)
            {
                string message = string.Format("The current training mode index is set to {0} but the current number of available training modes is {1}.", CurrentModeIndex, AvailableModes.Count);
                throw new IndexOutOfRangeException(message);
            }

            CurrentModeIndex = index;

            if (ModeChanged != null)
            {
                ModeChanged(this, new ModeChangedEventArgs(CurrentMode));
            }
        }

        /// <inheritdoc />
        public void SetMode(IMode mode)
        {
            if (AvailableModes.Contains(mode))
            {
                SetMode(AvailableModes.IndexOf(mode));
            }
            else
            {
                throw new MissingModeException("Given mode is not part of the available modes!");
            }
        }
    }
}
