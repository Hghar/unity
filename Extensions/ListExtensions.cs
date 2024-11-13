using System;
using System.Collections.Generic;

namespace Extensions
{
    public static class ListExtensions
    {
        private static readonly Random _random = new Random();

        public static T GetRandom<T>(this List<T> list)
        {
            int randomIndex = _random.Next(list.Count);
            return list[randomIndex];
        }
    }
}