using System;

namespace Innoactive.Hub.Training
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