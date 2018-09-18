namespace Dualog.eCatch.Shared.Api
{
    public class RegisterDeviceRequest
    {
        public string PublicKey { get; set; }
        public string InstallTime { get; set; }
        public string RadioCallSignal { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceSerial { get; set; }
    }
}
