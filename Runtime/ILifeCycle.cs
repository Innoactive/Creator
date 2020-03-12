using System;

namespace Innoactive.Hub.Training
{
    public interface ILifeCycle
    {
        event EventHandler<ActivationStateChangedEventArgs> StageChanged;

        IEntity Owner { get; }

        Stage Stage { get; }

        void Activate();

        void Deactivate();

        void MarkToFastForward();

        void MarkToFastForwardStage(Stage stage);

        void Update();
    }
}
