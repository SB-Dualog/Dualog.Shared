using System;

namespace Dualog.eCatch.Shared.Exceptions
{
    public class MessageRefusedException : Exception
    {
        public MessageRefusedException(string details) : base(details)
        {
        }
    }
}
