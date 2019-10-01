namespace Dualog.eCatch.Shared.Api
{
    public class DeviceInformationResponse
    {
        public string CurrentAppVersion { get; set; }
        public bool ClientVersionIsOutDated { get; set; }
        public bool AppApiFeature { get; set; }
    }
}
