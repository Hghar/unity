using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Shared.Extensions
{
    public static class EnumerableExtensions
    {
        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            int randomIndex = UnityEngine.Random.Range(0, enumerable.Count());
            return enumerable.ElementAt(randomIndex);
        }
    }
}