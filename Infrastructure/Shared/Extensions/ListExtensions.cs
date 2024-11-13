using System.Collections.Generic;

namespace Infrastructure.Shared.Extensions
{
    public static class ListExtensions
    {
        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            int length = list.Count;
            int lastIndex = length - 1;
            for (int i = 0; i < lastIndex; ++i)
            {
                int randomItem = UnityEngine.Random.Range(i, length);
                (list[i], list[randomItem]) = (list[randomItem], list[i]);
            }

            return list;
        }

        public static T Random<T>(this IList<T> list)
        {
            int randomIndex = UnityEngine.Random.Range(0, list.Count);
            return list[randomIndex];
        }
        
        public static T[] Random<T>(this IList<T> list, int count)
        {
            var randomElements = new List<T>();
            var copy = new List<T>(list);
            for (int i = 0; i < count; i++)
            {
                var randomElement = copy.Random();
                copy.Remove(randomElement);
                randomElements.Add(randomElement);
            }

            return randomElements.ToArray();
        }
    }
}