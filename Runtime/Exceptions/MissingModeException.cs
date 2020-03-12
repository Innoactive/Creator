using System;

namespace Innoactive.Hub.Training.Exceptions
{
    public class MissingModeException : TrainingException
    {
        public MissingModeException()
        {
        }

        public MissingModeException(string message) : base(message)
        {
        }

        public MissingModeException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
