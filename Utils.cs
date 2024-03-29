﻿using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace Webshot
{
    public static class Utils
    {
        public static string SanitizeFilename(string filename)
        {
            var sanitized = Regex.Replace(filename, "[^-a-zA-Z0-9]+", "_");
            sanitized = Regex.Replace(sanitized, "__+", "_");
            return sanitized;
        }

        public static string CreateTimestampDirectory(
            string parentDir,
            DateTime? creationTimestamp = null)
        {
            Directory.CreateDirectory(parentDir);
            string timestamp = (creationTimestamp ?? DateTime.Now).Timestamp();
            var origPath = Path.Combine(parentDir, timestamp);
            string uniquePath = MakeDirectoryNameUnique(origPath);
            Directory.CreateDirectory(uniquePath);
            return uniquePath;
        }

        public static string MakeDirectoryNameUnique(string dir)
        {
            string uniquePath = dir;
            var i = 1;
            while (Directory.Exists(uniquePath))
            {
                uniquePath = Path.Combine(dir, $"-[{i++}]");
            }
            return uniquePath;
        }

        public static int InRange(int value, int min, int max) =>
            Math.Min(max, Math.Max(min, value));

        public static void ChangeSettings(Action<Properties.Settings> settingsFn)
        {
            settingsFn(Properties.Settings.Default);
            Properties.Settings.Default.Save();
        }

        public static int CombineHashCodes(int h1, int h2) => (((h1 << 5) + h1) ^ h2);

        public static Action Debounce(Action action, int delay)
        {
            System.Threading.Timer timer = null;
            void callback(object o)
            {
                action();
                timer?.Dispose();
                timer = null;
            }

            System.Threading.Timer CreateTimer()
            {
                return new System.Threading.Timer(
                    callback,
                    null,
                    delay,
                    System.Threading.Timeout.Infinite);
            }

            return () =>
            {
                timer?.Dispose();
                timer = CreateTimer();
            };
        }
    }

    public sealed class Debouncer : IDisposable
    {
        private Timer _timer;
        private readonly Action _action;
        private readonly int _callLimit;

        /// <summary>
        /// Creates a <see cref="Debouncer"/> instance.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="delay">Performs <paramref name="action"/> after <paramref name="delay"/> milliseconds of no calls to <see cref="Call"/>.</param>
        /// <param name="callLimit">Dispose the object after invoking <paramref name="action"/> this many times (0 for unlimited).</param>
        public Debouncer(Action action, int delay, int callLimit = 0)
        {
            _action = action;
            _callLimit = callLimit;
            _timer = new Timer()
            {
                Enabled = false,
                Interval = delay
            };
            _timer.Tick += TimerTick;
        }

        /// <summary>
        /// Disposes the debouncer and its resources.
        /// </summary>
        public void Dispose()
        {
            _timer.Dispose();
            _timer = null;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            CallAction();
        }

        /// <summary>
        /// Schedule the action's invokation.
        /// </summary>
        public void Call()
        {
            _timer?.Start();
        }

        public void Cancel()
        {
            _timer?.Stop();
        }

        /// <summary>
        /// Immediately call the action if it's scheduled for the future.
        /// </summary>
        public void Flush()
        {
            if (_timer?.Enabled == true)
            {
                CallAction();
            }
        }

        private int _callCount = 0;

        private void CallAction()
        {
            _timer?.Stop();
            _action();
            if (++_callCount == _callLimit)
            {
                Dispose();
            }
        }
    }

    public static class Extensions
    {
        /// <summary>
        /// Returns or creates a new collection as the value for a dictionary's key.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TVal"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TVal GetOrInitValue<TKey, TVal>(this Dictionary<TKey, TVal> dict, TKey key) where TVal : new()
        {
            return dict.GetOrInitValue(key, () => new TVal());
        }

        /// <summary>
        /// Returns or creates a new collection as the value for a dictionary's key.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TVal"></typeparam>
        ///
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="initializer">A function that creates the object</param>
        /// <returns></returns>
        public static TVal GetOrInitValue<TKey, TVal>(this Dictionary<TKey, TVal> dict, TKey key, Func<TVal> initializer)
        {
            if (!dict.TryGetValue(key, out var existingCol))
            {
                existingCol = initializer();
                dict[key] = existingCol;
            }
            return existingCol;
        }

        public static void ForEach<T>(this IEnumerable<T> items, Action<T> fn)
        {
            items.ForEach((x, _) => fn(x));
        }

        public static void ForEach<T>(this IEnumerable<T> items, Action<T, int> fn)
        {
            int i = 0;
            foreach (var item in items)
            {
                fn(item, i++);
            }
        }

        public static bool Unanimous<T>(this IEnumerable<T> items)
        {
            if (!items.Any()) return true;
            var comparer = EqualityComparer<T>.Default;
            var first = items.First();
            bool MatchesFirst(T item) => comparer.Equals(first, item);
            return items.Skip(1).All(MatchesFirst);
        }

        /// <summary>
        /// Returns a new Uri with minor differences (e.g. fragment) removed or null.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static Uri Standardize(this Uri uri)
        {
            // Removes fragment/anchor
            // Adds slashes to path if option is enabled and the url isn't a filename with extension
            string lastSegment = uri.Segments.Last();

            // Add a trailing slash to non-filename path components
            // e.g. http://example.com/test -> http://example.com/test/
            bool appendTrailingSlash = true;
            bool addSlash = appendTrailingSlash
                && !lastSegment.Contains(".")
                && !lastSegment.EndsWith("/");

            string trailingSlash = addSlash ? "/" : "";
            return new Uri($"{uri.Scheme}{Uri.SchemeDelimiter}{uri.Authority}{uri.AbsolutePath}{trailingSlash}{uri.Query}");
        }

        /// <summary>
        /// Standardizes URI if possible or returns the original uri on failure.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static Uri TryStandardize(this Uri uri)
        {
            try
            {
                return uri.Scheme.StartsWith(Uri.UriSchemeHttp)
                    ? uri.Standardize()
                    : uri;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error parsing URI {uri}: {ex.Message}");
                return uri;
            }
        }

        public static string Timestamp(this DateTime dt) =>
            dt.ToString("yyyy-dd-M_HH-mm-ss");

        public static T JsonClone<T>(this T obj)
        {
            var serialized = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<T>(serialized);
        }
    }

    /// <summary>
    /// Encryption methods derived from: https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.protecteddata?redirectedfrom=MSDN&view=dotnet-plat-ext-3.1
    /// </summary>
    public static class Encryption
    {
        private static byte[] s_additionalEntropy = { 7, 2, 56, 72, 2, 2, 23, 3, 3, 4, 1, 67 };

        /// <summary>
        /// Encrypts plain-text string to a base64-encoded, encrypted string.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Protect(string data)
        {
            var encoding = new System.Text.UTF8Encoding();
            byte[] input = encoding.GetBytes(data);
            var encrypted = Protect(input);
            return Convert.ToBase64String(encrypted); ;
        }

        /// <summary>
        /// Decrypts a base64-encoded, encrypted string.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Unprotect(string data)
        {
            byte[] input = Convert.FromBase64String(data);
            var decrypted = Unprotect(input);
            var encoding = new System.Text.UTF8Encoding();
            return encoding.GetString(decrypted);
        }

        public static byte[] Protect(byte[] data)
        {
            try
            {
                // Encrypt the data using DataProtectionScope.CurrentUser. The result can be decrypted
                // only by the same current user.
                return ProtectedData.Protect(data, s_additionalEntropy, DataProtectionScope.CurrentUser);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("Data was not encrypted. An error occurred.");
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        public static byte[] Unprotect(byte[] data)
        {
            try
            {
                //Decrypt the data using DataProtectionScope.CurrentUser.
                return ProtectedData.Unprotect(data, s_additionalEntropy, DataProtectionScope.CurrentUser);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("Data was not decrypted. An error occurred.");
                Console.WriteLine(e.ToString());
                return null;
            }
        }
    }
}