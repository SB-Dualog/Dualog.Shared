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
        public static EcatchLangauge ToEcatchLanguage(this CultureInfo currentInfo)
        {
            if (currentInfo.Name == "NO") return EcatchLangauge.Norwegian;
            if (currentInfo.Name == "US" || currentInfo.Name == "GB") return EcatchLangauge.English;

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
