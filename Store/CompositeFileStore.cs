using Newtonsoft.Json;
using System;
using System.IO;

namespace Webshot
{
    public class FileStore<T> : IObjectStore<T> where T : class
    {
        public string FilePath { get; set; }

        public FileStore(string filePath)
        {
            FilePath = filePath;
        }

        public T Load()
        {
            return Load(typeof(T)) as T;
        }

        private static JsonSerializerSettings JsonSettings =>
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                };

        public object Load(Type t)
        {
            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException("File not found.", FilePath);
            }
            var serializedContents = File.ReadAllText(FilePath);

            return JsonConvert.DeserializeObject(
                serializedContents,
                t,
                JsonSettings);
        }

        public void Save(T obj)
        {
            var dir = Path.GetDirectoryName(FilePath);
            Directory.CreateDirectory(dir);

            var serializedContents = JsonConvert.SerializeObject(obj, JsonSettings);
            File.WriteAllText(FilePath, serializedContents);
        }
    }
}