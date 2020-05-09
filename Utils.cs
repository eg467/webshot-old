using System;
using System.Collections.Generic;

namespace Webshot
{
    internal static class Utils
    {
        public static void ChangeSettings(Action<Properties.Settings> settingsFn)
        {
            settingsFn(Properties.Settings.Default);
            Properties.Settings.Default.Save();
        }
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