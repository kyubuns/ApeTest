using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ApeTest.Utils
{
    public static class LinqExtension
    {
        public static IOrderedEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Random.Range(0f, 1f));
        }

        public static T RandomPick<T>(this IEnumerable<T> source)
        {
            return source.Shuffle().FirstOrDefault();
        }
    }
}
