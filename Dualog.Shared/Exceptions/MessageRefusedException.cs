using System;

namespace Dualog.Shared.Exceptions
{
    public class MessageRefusedException : Exception
    {
        public MessageRefusedException(string details) : base(details)
        {
        }
    }
}
