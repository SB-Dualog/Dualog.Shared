namespace Dualog.eCatch.Shared.Models
{
    public class SimpleCoordinate
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public SimpleCoordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
