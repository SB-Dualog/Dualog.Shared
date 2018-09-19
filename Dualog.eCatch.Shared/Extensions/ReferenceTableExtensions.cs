using System.Collections.Generic;
using Dualog.eCatch.Shared.Enums;
using Dualog.eCatch.Shared.Models;
using Dualog.eCatch.Shared.Utilities;

namespace Dualog.eCatch.Shared.Extensions
{
    public static class ReferenceTableExtensions
    {
        #region Reference tables with Norwegian and English texts
        private static readonly Dictionary<EcatchLangauge, Dictionary<string, string>> _fishNames 
            = new Dictionary<EcatchLangauge, Dictionary<string, string>>();
        private static readonly Dictionary<EcatchLangauge, Dictionary<string, string>> _toolNames 
            = new Dictionary<EcatchLangauge, Dictionary<string, string>>();
        private static readonly Dictionary<EcatchLangauge, Dictionary<string, string>> _zoneNames 
            = new Dictionary<EcatchLangauge, Dictionary<string, string>>();
        private static readonly Dictionary<EcatchLangauge, Dictionary<string, string>> _fishingActivityNames 
            = new Dictionary<EcatchLangauge, Dictionary<string, string>>();
        private static readonly Dictionary<EcatchLangauge, Dictionary<string, string>> _errorCodes 
            = new Dictionary<EcatchLangauge, Dictionary<string, string>>();

        public static string ToFishName(this string code, EcatchLangauge lang)
        {
            if (!_fishNames.ContainsKey(lang))
            {
                _fishNames.Add(lang, Services.KeyValueReferenceTableLoader.Load("FishSpecies.txt", lang));
            }

            return _fishNames[lang].ContainsKey(code) ? _fishNames[lang][code] : code;
        }

        public static string ToToolName(this string code, EcatchLangauge lang)
        {
            if (!_toolNames.ContainsKey(lang))
            {
                _toolNames.Add(lang, Services.KeyValueReferenceTableLoader.Load("tools.txt", lang));
            }

            return _toolNames[lang].ContainsKey(code) ? _toolNames[lang][code] : code;
        }

        public static string ToZoneName(this string code, EcatchLangauge lang)
        {
            if (!_zoneNames.ContainsKey(lang))
            {
                _zoneNames.Add(lang, Services.KeyValueReferenceTableLoader.Load("Zones.txt", lang));
            }

            return _zoneNames[lang].ContainsKey(code) ? _zoneNames[lang][code] : code;
        }

        public static string ToFishingActivityName(this string code, EcatchLangauge lang)
        {
            if (!_fishingActivityNames.ContainsKey(lang))
            {
                _fishingActivityNames.Add(lang, Services.KeyValueReferenceTableLoader.Load("FishingActivities.txt", lang));
            }
            return _fishingActivityNames[lang].ContainsKey(code) ? _fishingActivityNames[lang][code] : code;
        }

        public static string ToDetailedErrorCode(this string code, EcatchLangauge lang)
        {
            if (!_errorCodes.ContainsKey(lang))
            {
                _errorCodes.Add(lang, Services.KeyValueReferenceTableLoader.Load("ErrorCodes.txt", lang));
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
