using System.Collections.Generic;
using Dualog.Shared.Models;

namespace Dualog.Shared.Extensions
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
    }
}
