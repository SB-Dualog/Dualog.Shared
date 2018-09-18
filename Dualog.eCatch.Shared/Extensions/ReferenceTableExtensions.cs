using System.Collections.Generic;
using Dualog.eCatch.Shared.Enums;
using Dualog.eCatch.Shared.Models;
using Dualog.eCatch.Shared.Utilities;

namespace Dualog.eCatch.Shared.Extensions
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

        public static string ToFishName(this string code, EcatchLangauge lang)
        {
            if (_fishNames == null)
            {
                _fishNames = Services.KeyValueReferenceTableLoader.Load("FishSpecies.txt", lang);
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
            _zoneNames = null;
        }

        public static string ToToolName(this string code, EcatchLangauge lang)
        {
            if (_toolNames == null)
            {
                _toolNames = Services.KeyValueReferenceTableLoader.Load("tools.txt", lang);
            }

            return _toolNames.ContainsKey(code) ? _toolNames[code] : code;
        }

        public static string ToZoneName(this string code, EcatchLangauge lang)
        {
            if (_zoneNames == null)
            {
                _zoneNames = Services.KeyValueReferenceTableLoader.Load("Zones.txt", lang);
            }

            return _zoneNames.ContainsKey(code) ? _zoneNames[code] : code;
        }

        public static string ToHarbourName(this string code)
        {
            if (_harbourNames == null)
            {
                _harbourNames = Services.KeyValueReferenceTableLoader.Load("Harbours.txt", EcatchLangauge.Norwegian);
            }

            return _harbourNames.ContainsKey(code) ? _harbourNames[code] : code;
        }

        public static string ToAnimalName(this string code)
        {
            if (_animalNames == null)
            {
                _animalNames = Services.KeyValueReferenceTableLoader.Load("MammalsAndBirds.txt", EcatchLangauge.Norwegian);
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

        public static string ToFishingActivityName(this string code, EcatchLangauge lang)
        {
            if (_fishingActivityNames == null)
            {
                _fishingActivityNames = Services.KeyValueReferenceTableLoader.Load("FishingActivities.txt", lang);
            }
            return _fishingActivityNames.ContainsKey(code) ? _fishingActivityNames[code] : code;
        }

        public static string ToDetailedErrorCode(this string code, EcatchLangauge lang)
        {
            if (_errorCodes == null)
            {
                _errorCodes = Services.KeyValueReferenceTableLoader.Load("ErrorCodes.txt", lang);
            }
            return _errorCodes.ContainsKey(code) ? _errorCodes[code] : code;
        }
    }
}
