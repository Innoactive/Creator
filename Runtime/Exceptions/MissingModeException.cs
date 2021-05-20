using System;

namespace VPG.Core.Exceptions
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
