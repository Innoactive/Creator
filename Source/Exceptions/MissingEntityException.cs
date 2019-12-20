using System;

namespace Innoactive.Hub.Training.Exceptions
{
    public class MissingEntityException : TrainingException
    {
        public MissingEntityException(string message) : base(message)
        {
        }
    }
}
