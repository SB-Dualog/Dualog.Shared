namespace Dualog.eCatch.Shared.Api
{
    public class ActivateLinkDeviceRequest
    {
        // Login
        public string LinkAuthToken { get; set; }

        // Device activation 
        public string PublicKey { get; set; }
        public string InstallTime { get; set; }

        // Ship data
        public string ShipName { get; set; }
        public string RadioCallSignal { get; set; }
        public string RegistrationNumber { get; set; }
        public string VesselEmail { get; set; }

        // Device data
        public string DeviceModel { get; set; }
        public string DeviceSerial { get; set; }
        public string DeviceVersion { get; set; }
        public string AppVersion { get; set; }
    }
}
