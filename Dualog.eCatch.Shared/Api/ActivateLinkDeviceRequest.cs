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

        public ActivateLinkDeviceRequest(
            string linkAuthToken, 
            string publicKey, string installTime, 
            string shipName, string radioCallSignal, string registrationNumber, string vesselEmail, 
            string deviceModel, string deviceSerial, string deviceVersion, string appVersion)
        {
            LinkAuthToken = linkAuthToken;
            PublicKey = publicKey;
            InstallTime = installTime;
            ShipName = shipName;
            RadioCallSignal = radioCallSignal;
            RegistrationNumber = registrationNumber;
            VesselEmail = vesselEmail;
            DeviceModel = deviceModel;
            DeviceSerial = deviceSerial;
            DeviceVersion = deviceVersion;
            AppVersion = appVersion;
        }
    }
}
