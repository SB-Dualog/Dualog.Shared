using System;
using System.Globalization;
using Dualog.eCatch.Shared.Enums;

namespace Dualog.eCatch.Shared.Extensions
{
    public static class LanguageExtensions
    {
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
    }
}
