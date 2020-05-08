using System;
using System.Collections.Generic;

namespace Webshot
{
    internal class Utils
    {
    }

    public static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> fn)
        {
            foreach (var item in items)
            {
                fn(item);
            }
        }
    }
}