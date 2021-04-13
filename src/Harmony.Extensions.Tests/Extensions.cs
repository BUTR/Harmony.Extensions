using System;
using System.Collections.Generic;
using System.Text;

namespace Harmony.Extensions.Tests
{
    public static class Extensions
    {
        internal static Dictionary<K, V> Merge<K, V>(this IEnumerable<KeyValuePair<K, V>> firstDict, params IEnumerable<KeyValuePair<K, V>>[] otherDicts)
        {
            var dictionary = new Dictionary<K, V>();

            foreach (var keyValuePair in firstDict)
                dictionary[keyValuePair.Key] = keyValuePair.Value;

            foreach (var otherDict in otherDicts)
            {
                foreach (var keyValuePair in otherDict)
                    dictionary[keyValuePair.Key] = keyValuePair.Value;
            }

            return dictionary;
        }
    }
}