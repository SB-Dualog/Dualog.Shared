using System;

namespace Dualog.eCatch.Shared.Models
{
    public class PositionAndTime : SimpleCoordinate
    {
        public DateTime DateTime { get; set; }

        public PositionAndTime(DateTime dateTime, double latitude, double longitude) : base(latitude, longitude)
        {
            DateTime = dateTime;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
