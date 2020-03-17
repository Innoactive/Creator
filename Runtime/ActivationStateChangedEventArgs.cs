using System;

namespace Innoactive.Creator.Core
{
    public class ActivationStateChangedEventArgs : EventArgs
    {
        public readonly Stage Stage;

        public ActivationStateChangedEventArgs(Stage stage)
        {
            Stage = stage;
        }
    }
}
