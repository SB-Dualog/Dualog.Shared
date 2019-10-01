using System.Collections.Generic;

namespace Dualog.eCatch.Shared.Api
{
    public class ReturnMessageResponse
    {
        public int syncId { get; set; }
        public IList<string> retMessages { get; set; }
    }
}
