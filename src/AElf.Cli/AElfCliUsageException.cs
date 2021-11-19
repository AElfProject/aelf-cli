using System;

namespace AElf.Cli
{
    public class AElfCliUsageException : Exception
    {
        public AElfCliUsageException(string message)
            : base(message)
        {

        }

        public AElfCliUsageException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}