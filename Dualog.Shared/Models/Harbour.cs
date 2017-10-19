using Dualog.Shared.Contracts;
using Dualog.Shared.Utilities;

namespace Dualog.Shared.Models
{
	public class Harbour : IHasId
	{		
		public string CountryCode { get; }
		public string UnloCode { get; }
		public string Name { get; }
		public double Latitude { get; }
		public double Longitude { get; }

	    public string Id => UnloCode;
        public bool HasCoordinates { get; }
			
		public Harbour(string countryCode, string unloCode, string name, string latitude, string longitude)
		{
			CountryCode = countryCode;
			UnloCode = countryCode + unloCode;
			Name = name;
			Latitude = string.IsNullOrEmpty(latitude) ? 0 : LocationUtils.DmCoordinateToDecimal(latitude);
			Longitude = string.IsNullOrEmpty(longitude) ? 0 : LocationUtils.DmCoordinateToDecimal(longitude);
		    HasCoordinates = !string.IsNullOrEmpty(latitude) && !string.IsNullOrEmpty(longitude);
		}

	    public override string ToString() => Name;
		public override int GetHashCode() => new { CountryCode, UnloCode, Name, Latitude, Longitude }.GetHashCode();

		public override bool Equals(object obj)
		{
			var rhs = obj as Harbour;
			if (rhs == null) return false;
			return rhs.GetHashCode() == GetHashCode();
		}
	}
}