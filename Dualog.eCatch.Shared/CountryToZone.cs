namespace Dualog.eCatch.Shared
{
    public static class CountryToZone
    {
        public static string FindZone(string country)
        {
            if (country == "DK" || country == "IE" || country == "SE") return Constants.Zones.EU;
            if (country == "RU") return Constants.Zones.Russia;
            if (country == "IS") return Constants.Zones.Island;
            if (country == "FO") return Constants.Zones.FaroeIslands;
            if (country == "SJ") return Constants.Zones.Svalbard;
            if (country == "GB") return Constants.Zones.GBR;
            if (country == "GL") return Constants.Zones.Greenland;
            return Constants.Zones.Norway;
        }
    }
}
