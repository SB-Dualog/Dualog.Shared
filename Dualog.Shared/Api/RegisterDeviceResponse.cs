namespace Dualog.Shared.Api
{
    public class RegisterDeviceResponse
    {
        public string AuthCode { get; set; }
        public string DeviceName { get; set; }
        public bool AuthCodeIsReceived { get; set; }
    }
}
