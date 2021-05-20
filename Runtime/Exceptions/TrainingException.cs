using System;

namespace VPG.Core.Exceptions
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
