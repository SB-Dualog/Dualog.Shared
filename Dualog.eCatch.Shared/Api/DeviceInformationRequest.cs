using System;

namespace Dualog.eCatch.Shared.Api
{
    public class DeviceInformationRequest
    {
        public string AppVersion { get; set; }
        public string DeviceVersion { get; set; }
        public DateTime LastUsed { get; set; }
        public string AuthCode { get; set; }
        public string RadioCallSignal { get; set; }
    }
}
