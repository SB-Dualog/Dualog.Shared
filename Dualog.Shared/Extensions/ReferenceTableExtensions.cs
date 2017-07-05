using System.Collections.Generic;
using Dualog.Shared.Models;
using Dualog.Shared.Utilities;

namespace Dualog.Shared.Extensions
{
    public static class ReferenceTableExtensions
    {
        private static Dictionary<string, string> _fishNames;
        private static Dictionary<string, string> _toolNames;
        private static Dictionary<string, string> _zoneNames;
        private static Dictionary<string, DmCoordinate> _harbours;
        private static Dictionary<string, string> _harbourNames;
        private static Dictionary<string, string> _animalNames; 
        private static Dictionary<string, string> _fishingActivityNames;
        private static Dictionary<string, string> _errorCodes;

        public static string ToFishName(this string code, int langaugeIndex = 1)
        {
            if (_fishNames == null)
            {
                _fishNames = Services.KeyValueReferenceTableLoader.Load("FishSpecies.txt", langaugeIndex);
            }

            return _fishNames.ContainsKey(code) ? _fishNames[code] : code;
        }

        /// <summary>
        /// Call this method to clear all language translations after changing language
        /// </summary>
        public static void ClearLanguageCache()
        {
            _fishNames = null;
            _toolNames = null;
            _fishingActivityNames = null;
            _errorCodes = null;
        }

        public static string ToToolName(this string code, int languageIndex = 1)
        {
            if (_toolNames == null)
            {
                _toolNames = Services.KeyValueReferenceTableLoader.Load("tools.txt", languageIndex);
            }

            return _toolNames.ContainsKey(code) ? _toolNames[code] : code;
        }

        public static string ToZoneName(this string code)
        {
            if (_zoneNames == null)
            {
                _zoneNames = Services.KeyValueReferenceTableLoader.Load("Zones.txt");
            }

            return _zoneNames.ContainsKey(code) ? _zoneNames[code] : code;
        }

        public static string ToHarbourName(this string code)
        {
            if (_harbourNames == null)
            {
                _harbourNames = Services.KeyValueReferenceTableLoader.Load("Harbours.txt");
            }

            return _harbourNames.ContainsKey(code) ? _harbourNames[code] : code;
        }

        public static string ToAnimalName(this string code)
        {
            if (_animalNames == null)
            {
                _animalNames = Services.KeyValueReferenceTableLoader.Load("MammalsAndBirds.txt");
            }
            return _animalNames.ContainsKey(code) ? _animalNames[code] : code;
        }

        public static SimpleCoordinate HarbourToCoordinate(this string code)
        {
            if (_harbours == null)
            {
                _harbours = Services.HarbourCoordinateLoader.Load();
            }

            if(!_harbours.ContainsKey(code)) throw new KeyNotFoundException("Could not find harbour with key {0}".FormatWith(code));

            var dmCoordinate = _harbours[code];
            var lat = LocationUtils.DmCoordinateToDecimal(dmCoordinate.Latitude);
            var lon = LocationUtils.DmCoordinateToDecimal(dmCoordinate.Longitude);
            return new SimpleCoordinate(lat, lon);
        }

        public static string ToFishingActivityName(this string code, int languageIndex = 2)
        {
            if (_fishingActivityNames == null)
            {
                _fishingActivityNames = Services.KeyValueReferenceTableLoader.LoadFishingActivities("FishingActivities.txt", languageIndex);
            }
            return _fishingActivityNames.ContainsKey(code) ? _fishingActivityNames[code] : code;
        }

        public static string ToDetailedErrorCode(this string code, int languageIndex)
        {
            if (_errorCodes == null)
            {
                _errorCodes = Services.KeyValueReferenceTableLoader.Load("ErrorCodes.txt", languageIndex);
            }
            return _errorCodes.ContainsKey(code) ? _errorCodes[code] : code;
        }
    }
}
