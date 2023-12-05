using System;
using System.Collections.Generic;

namespace Commons.Extensions
{
    public static class ListExtensions
    {
        public static List<T> GetRandom<T>(this List<T> list, int count = 1)
        {
            var random = new Random();

            if (count > list.Count) return list;

            var randomElements = new List<T>();
            while (randomElements.Count < count)
            {
                var randomIndex = random.Next(0, list.Count);
                var randomEntity = list[randomIndex];
                randomElements.Add(randomEntity);
                list.RemoveAt(randomIndex);
            }

            return randomElements;
        }
    }
}