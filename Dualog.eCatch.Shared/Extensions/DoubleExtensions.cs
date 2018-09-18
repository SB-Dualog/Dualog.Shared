using System.Globalization;
using Dualog.eCatch.Shared.Enums;

namespace Dualog.eCatch.Shared.Extensions
{
    public static class DoubleExtensions
    {
        public static string ToWgs84Format(this double coord, CoordinateType coordinateType)
        {
            var wgs = coordinateType == CoordinateType.Latitude ? coord.ToString("00.000", CultureInfo.InvariantCulture) : coord.ToString("000.000", CultureInfo.InvariantCulture);
            return coord > 0 ? $"+{wgs}" : wgs;
        }

        public static string ToWgs84ShortFormat(this double coord, CoordinateType coordinateType)
        {
            var wgs = coordinateType == CoordinateType.Latitude ? coord.ToString("00.0", CultureInfo.InvariantCulture) : coord.ToString("000.0", CultureInfo.InvariantCulture);
            return coord > 0 ? $"+{wgs}" : wgs;
        }

        public static string ToReadableCoordinate(this double coord, CoordinateType coordinateType)
        {
            var coordString = coordinateType == CoordinateType.Latitude ? $"{coord:##.###}" : $"{coord:###.###}";
            if (coordinateType == CoordinateType.Latitude)
            {
                return coord > 0 ? $"{coordString}N" : $"{coordString}S".Replace("-", "");
            }
            return coord > 0 ? $"{coordString}E" : $"{coordString}W".Replace("-", "");
        }
        
    }
}
