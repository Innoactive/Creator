using System;

namespace Innoactive.Creator.Core.Exceptions
{
    public class MissingEntityException : TrainingException
    {
        public MissingEntityException(string message) : base(message)
        {
        }
    }
}
