using System;
using System.Collections.Generic;
using System.Linq;

namespace Dualog.eCatch.Shared.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool In<T>(this T item, IEnumerable<T> enumerable)
        {
            return enumerable.Contains(item);
        }

        public static bool In<T>(this T item, params T[] enumerable)
        {
            return enumerable.Contains(item);
        }

        public static bool NotIn<T>(this T item, IEnumerable<T> enumerable)
        {
            return !enumerable.Contains(item);
        }

        public static bool NotIn<T>(this T item, params T[] enumerable)
        {
            return !enumerable.Contains(item);
        }

        public static string ToJoinedString<T>(this IEnumerable<T> enumerable, string separator)
        {
            return string.Join(separator, enumerable.Select(e => e.ToString()));
        }

        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
        {
            return !enumerable.Any();
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || enumerable.IsEmpty();
        }

        public static bool IsNotEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable != null && enumerable.Any();
        }

        public static string ToQueryStrings(this IEnumerable<KeyValuePair<string, string>> pairs)
        {
            return pairs.Select(ToQueryString).ToJoinedString("&");
        }

        public static string ToQueryString(this KeyValuePair<string, string> kvp)
        {
            return kvp.Key.ToUrlEncoded() + "=" + kvp.Value.ToUrlEncoded();
        }

        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            foreach (var item in sequence)
            {
                action(item);
            }
        }

        public static int IndexOf<T>(this IEnumerable<T> sequence, T item, int notFoundIndex)
        {
            var index = 0;

            foreach(var o in sequence)
            {
                if (o.Equals(item))
                {
                    return index;
                }
                index++;
            }

            return notFoundIndex;
        }
    }
}