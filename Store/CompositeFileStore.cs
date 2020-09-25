using Newtonsoft.Json;
using System;
using System.IO;

namespace Webshot
{
    public class FileStore<T> : IObjectStore<T> where T : class
    {
        public string FilePath { get; set; }
        public bool FileExists => File.Exists(FilePath);

        public FileStore(string filePath)
        {
            FilePath = filePath;
        }

        public T Load()
        {
            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException("File not found.", FilePath);
            }
            var serializedContents = File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<T>(serializedContents, JsonSettings);
        }

        private static JsonSerializerSettings JsonSettings =>
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                };

        public void Save(T obj)
        {
            var dir = Path.GetDirectoryName(FilePath);

            if (!string.IsNullOrEmpty(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var serializedContents = JsonConvert.SerializeObject(obj, JsonSettings);
            File.WriteAllText(FilePath, serializedContents);
        }
    }
}