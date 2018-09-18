using System.Collections.Generic;
using Dualog.eCatch.Shared.Enums;
using Dualog.eCatch.Shared.Models;
using Dualog.eCatch.Shared.Utilities;

namespace Dualog.eCatch.Shared.Extensions
{
    public static class ReferenceTableExtensions
    {
        #region Reference tables with Norwegian and English texts
        private static Dictionary<EcatchLangauge, Dictionary<string, string>> _fishNames;
        private static Dictionary<EcatchLangauge, Dictionary<string, string>> _toolNames;
        private static Dictionary<EcatchLangauge, Dictionary<string, string>> _zoneNames;
        private static Dictionary<EcatchLangauge, Dictionary<string, string>> _fishingActivityNames;
        private static Dictionary<EcatchLangauge, Dictionary<string, string>> _errorCodes;

        public static string ToFishName(this string code, EcatchLangauge lang)
        {
            if (_fishNames[lang] == null)
            {
                _fishNames[lang] = Services.KeyValueReferenceTableLoader.Load("FishSpecies.txt", lang);
            }

            return _fishNames[lang].ContainsKey(code) ? _fishNames[lang][code] : code;
        }

        public static string ToToolName(this string code, EcatchLangauge lang)
        {
            if (_toolNames[lang] == null)
            {
                _toolNames[lang] = Services.KeyValueReferenceTableLoader.Load("tools.txt", lang);
            }

            return _toolNames[lang].ContainsKey(code) ? _toolNames[lang][code] : code;
        }

        public static string ToZoneName(this string code, EcatchLangauge lang)
        {
            if (_zoneNames[lang] == null)
            {
                _zoneNames[lang] = Services.KeyValueReferenceTableLoader.Load("Zones.txt", lang);
            }

            return _zoneNames[lang].ContainsKey(code) ? _zoneNames[lang][code] : code;
        }

        public static string ToFishingActivityName(this string code, EcatchLangauge lang)
        {
            if (_fishingActivityNames[lang] == null)
            {
                _fishingActivityNames[lang] = Services.KeyValueReferenceTableLoader.Load("FishingActivities.txt", lang);
            }
            return _fishingActivityNames[lang].ContainsKey(code) ? _fishingActivityNames[lang][code] : code;
        }

        public static string ToDetailedErrorCode(this string code, EcatchLangauge lang)
        {
            if (_errorCodes[lang] == null)
            {
                _errorCodes[lang] = Services.KeyValueReferenceTableLoader.Load("ErrorCodes.txt", lang);
            }
            return _errorCodes[lang].ContainsKey(code) ? _errorCodes[lang][code] : code;
        }
        #endregion

        #region Reference tables with only Norwegian texts
        private static Dictionary<string, DmCoordinate> _harbours;
        private static Dictionary<string, string> _harbourNames;
        private static Dictionary<string, string> _animalNames;

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

            if (!_harbours.ContainsKey(code)) throw new KeyNotFoundException("Could not find harbour with key {0}".FormatWith(code));

            var dmCoordinate = _harbours[code];
            var lat = LocationUtils.DmCoordinateToDecimal(dmCoordinate.Latitude);
            var lon = LocationUtils.DmCoordinateToDecimal(dmCoordinate.Longitude);
            return new SimpleCoordinate(lat, lon);
        }
        #endregion
    }
}
