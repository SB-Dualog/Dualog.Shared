using System.Collections.Generic;
using System.Linq;
using Dualog.eCatch.Shared.Enums;
using Dualog.eCatch.Shared.Models;

namespace Dualog.eCatch.Shared.Extensions
{
    public static class ReadOnlyListExtensions
    {
        public static string ToNAF(this IEnumerable<FishFAOAndWeight> source)
        {
            return string.Join(" ", source);
        }

        public static string ToNAF(this IEnumerable<AnimalAndCount> source)
        {
            return string.Join(" ", source);
        }

        public static string ToDetailedWeightAndFishNameSummary(this IReadOnlyList<FishFAOAndWeight> source, EcatchLangauge lang)
        {
            var totalWeight = source.Sum(x => x.Weight);
            var result = $"{totalWeight.WithThousandSeparator()} kg";
            if (totalWeight > 0)
            {
                result += $"({string.Join(", ", source.Select(x => x.ToReadableFormat(lang)))})";
            }

            return result;
        }
    }
}
