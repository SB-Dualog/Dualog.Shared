using System;

namespace Dualog.eCatch.Shared.Utilities
{
	public static class LocationUtils
	{
		public static double DmCoordinateToDecimal(string coordinate)
		{
			char heading = coordinate[coordinate.Length - 1];
			double deg;
			double min;
			double factor;

			if (heading == 'N' || heading == 'S')
			{
				deg = Convert.ToDouble(coordinate.Substring(0, 2));
				min = Convert.ToDouble(coordinate.Substring(2, 2));
			}
			else				
			{
				deg = Convert.ToDouble(coordinate.Substring(0, 3));
				min = Convert.ToDouble(coordinate.Substring(3, 2));
			}				

			factor = (heading == 'N' || heading == 'E') ? 1.0 : -1.0;

			return deg + (min * 60 / 3600) * factor;
		}

	    public static double EstimatedCoordinateToDecimal(string coordinate)
	    {
            char heading = coordinate[0];
            double deg;
            double min;
            double factor;

            if (heading == 'N' || heading == 'S')
            {
                deg = Convert.ToDouble(coordinate.Substring(1, 2));
                min = Convert.ToDouble(coordinate.Substring(3, 2));
            }
            else
            {
                deg = Convert.ToDouble(coordinate.Substring(1, 3));
                min = Convert.ToDouble(coordinate.Substring(4, 2));
            }

            factor = (heading == 'N' || heading == 'E') ? 1.0 : -1.0;

            return deg + (min * 60 / 3600) * factor;
        }


		public static double DistanceInMetres(double lat1, double lon1, double lat2, double lon2)
		{
			if (lat1 == lat2 && lon1 == lon2)
				return 0.0;

			var theta = lon1 - lon2;

			var distance = Math.Sin(DegToRad(lat1)) * Math.Sin(DegToRad(lat2)) +
							Math.Cos(DegToRad(lat1)) * Math.Cos(DegToRad(lat2)) *
							Math.Cos(DegToRad(theta));

			distance = Math.Acos(distance);
			if (double.IsNaN(distance))
				return 0.0;

			distance = RadToDeg(distance) * 60.0 * 1.1515 * 1609.344;

			return distance;
		}

		public static double DistanceInNauticalMiles(double lat1, double lon1, double lat2, double lon2) 
		{
			return DistanceInMetres(lat1, lon1, lat2, lon2) * 0.000539956803;
		}

		private static double DegToRad(double deg) 
		{
			return (deg * Math.PI / 180.0);
		}

		private static double RadToDeg(double rad) 
		{
			return (rad / Math.PI * 180.0);
		}
	}
}