using System;
using System.Globalization;
using Dualog.eCatch.Shared.Enums;

namespace Dualog.eCatch.Shared.Models
{
    public class GeoAngle
    {
        public bool IsNegative { get; set; }
        public int Degrees { get; set; }
        public int Minutes { get; set; }
        public double Seconds { get; set; }
        public int Milliseconds { get; set; }
        public double DecimalMinutes { get; set; }

        public static double ToDouble(int degrees, double minutes, bool negative)
        {
            var res = degrees + minutes/60;
            return negative ? res * -1 : res;
        }

        /* PREVIOUS SOLUTION FOR CALCULATING FROM DECIMAL DEGREES TO MINUTESSECONDSDEGREES*/
        //public static GeoAngle FromDouble(double angleInDegrees)
        //{
        //    //ensure the value will fall within the primary range [-180.0..+180.0]
        //    while (angleInDegrees < -180.0)
        //        angleInDegrees += 360.0;

        //    while (angleInDegrees > 180.0)
        //        angleInDegrees -= 360.0;

        //    var result = new GeoAngle();

        //    //switch the value to positive
        //    result.IsNegative = angleInDegrees < 0;
        //    angleInDegrees = Math.Abs(angleInDegrees);

        //    //gets the degree
        //    result.Degrees = (int)Math.Floor(angleInDegrees);
        //    var delta = angleInDegrees - result.Degrees;
        //    delta = Math.Round(delta, 3);
        //    if (delta == 1)
        //    {
        //        delta = 0.999;
        //    }

        //    //gets minutes and seconds
        //    var seconds = (int)Math.Floor(3600.0 * delta);
        //    result.Seconds = seconds % 60;
        //    result.Minutes = (int)Math.Floor(seconds / 60.0);
        //    result.DecimalMinutes = seconds / 60.0;
        //    delta = delta * 3600.0 - seconds;

        //    //gets fractions
        //    result.Milliseconds = (int)(1000.0 * delta);

        //    return result;
        //}

        public static GeoAngle FromDouble(double angleInDegrees)
        {
            var decimalDegrees = Math.Abs(angleInDegrees);
            var result = new GeoAngle();

            result.Degrees = (int)Math.Truncate(decimalDegrees);
            result.Minutes = (int)Math.Truncate((decimalDegrees * 60) % 60);
            result.Seconds = Math.Round(decimalDegrees * 3600 % 60, 3);
            result.DecimalMinutes = 0;
            result.Milliseconds = 0;
            return result;
        }


        public override string ToString()
        {
            var degrees = IsNegative ? -Degrees : Degrees;
            return $"{degrees}° {Minutes:00}' {Seconds:00}\"";
        }

        public string ToWgsFormat(CoordinateType coordinateType)
        {
            var degrees = coordinateType == CoordinateType.Latitude ? Degrees.ToString() : Degrees.ToString("D3");
            var minutes = Minutes.ToString("D3");
            var heading = IsNegative ? "-" : "+";
            return $"{heading}{degrees}.{minutes}";
        }

        public string ToString(string format)
        {
            switch (format)
            {
                case "NS":
                    //return $"{Degrees}° {Minutes:00}' {Seconds:00}\".{Milliseconds:000} {(IsNegative ? "SouthShort".Localize() : "NorthShort".Localize())}"; // TODO Localize
                    var returnString = string.Format(CultureInfo.InvariantCulture, $"{Degrees}° {Minutes:00}' {Seconds:00.000}\" {(IsNegative ? "S" : "N")}");
                    return returnString;

                case "WE":
                    //return $"{Degrees}° {Minutes:00}' {Seconds:00}\".{Milliseconds:000} {(IsNegative ? "WestShort".Localize() : "EastShort".Localize())}"; // TODO Localize
                    returnString = string.Format(CultureInfo.InvariantCulture, $"{Degrees}° {Minutes:00}' {Seconds:00.000}\" {(IsNegative ? "W" : "E")}");
                    return returnString;
                    //return $"{Degrees}° {Minutes:00}' {Seconds:00.000}\" {(IsNegative ? "W" : "E")}";
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
