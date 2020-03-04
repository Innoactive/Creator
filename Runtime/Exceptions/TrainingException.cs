using System;

namespace Innoactive.Hub.Training.Exceptions
{
    public class TrainingException : Exception
    {
        public TrainingException()
        {
        }

        public TrainingException(string message) : base(message)
        {
        }

        public TrainingException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
