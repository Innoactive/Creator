using System;

namespace VPG.Creator.Core.Exceptions
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
