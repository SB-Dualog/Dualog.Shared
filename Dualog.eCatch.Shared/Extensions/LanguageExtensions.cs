using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Dualog.eCatch.Shared.Enums;
using Dualog.eCatch.Shared.Language;

namespace Dualog.eCatch.Shared.Extensions
{
    public static class LanguageExtensions
    {

        public static string Translate(this string text, EcatchLangauge lang, bool isPlural = false)
        {
            var localizedString = Translations.ResourceManager.GetString(text, lang.ToUiCulture());
            return ReplacePlurals(localizedString, isPlural) ?? text;
        }

        public static CultureInfo ToUiCulture(this EcatchLangauge lang)
        {
            switch (lang)
            {
                case EcatchLangauge.English:
                    return new CultureInfo("en-US");
                case EcatchLangauge.Norwegian:
                    return new CultureInfo("nb-NO");
                default:
                    throw new ArgumentException($"{lang} is not supported");
            }
        }

        /// <summary>
        /// We use reference table .txt files with lists of fish species etc, and they are tab separated with "code" "norwegianName" "englishName". They are inconsistent so we need to switch on what index is used for what file.
        /// </summary>
        /// <param name="ecatchLangauge"></param>
        /// <param name="referenceTableName"></param>
        /// <returns></returns>
        public static int ToReferenceTableIndex(this EcatchLangauge ecatchLangauge, string referenceTableName)
        {
            if (!referenceTableName.Contains(".txt"))
            {
                referenceTableName += ".txt";
            }

            switch (referenceTableName.ToUpperInvariant())
            {
                case "FISHSPECIES.TXT":
                case "ERRORCODES.TXT":
                case "TOOLS.TXT":
                    return ecatchLangauge == EcatchLangauge.Norwegian ? 1 : 2;

                case "FISHINGACTIVITIES.TXT":
                case "ZONES.TXT":
                    return ecatchLangauge == EcatchLangauge.Norwegian ? 2 : 1;

                case "HARBOURS.TXT":
                case "MAMMALSANDBIRDS.TXT":
                    return 1; //These two files only have one langauge, norwegian.

                default:
                    throw new Exception($"Could not find a reference table name with the name {referenceTableName}. It might not have support for different languages or it might not have been implemented in this method.");
            }
        }

        public static EcatchLangauge ToEcatchLanguage(this CultureInfo currentInfo)
        {
            if (currentInfo.Name == "nb-NO") return EcatchLangauge.Norwegian;
            if (currentInfo.Name == "en-US" || currentInfo.Name == "en-GB") return EcatchLangauge.English;

            throw new ArgumentException($"{currentInfo.DisplayName} is not supported for localization of Dualog eCatch sources");
        }

        private static string ReplacePlurals(string localizedString, bool isPlural)
        {
            if (string.IsNullOrEmpty(localizedString))
                return null;
            if (!localizedString.Contains("|"))
                return localizedString;
            const string escapedpipe = "<escapedPipe>";
            var escapedString = localizedString.Replace("||", escapedpipe); //Save escaped pipes and put them back in when done.
            var words = escapedString.Split();
            var pluralMap = MapPlurals(words, isPlural);
            var finalString = pluralMap
                .Aggregate(escapedString, (current, pair) => current.Replace(pair.Key, pair.Value))
                .Replace(escapedpipe, "|");
            return finalString;
        }

        private static Dictionary<string, string> MapPlurals(IEnumerable<string> words, bool isPlural)
        {
            var pluralMap = words.Where(w => w.Contains("|")).ToDictionary(w => w, w => {
                var parts = w.Split('|');
                if (parts.Length == 1)
                {
                    return parts[0];
                }
                var singular = parts[0];
                var plural = parts[1];
                return isPlural ? plural : singular;
            });
            return pluralMap;
        }
    }
}
