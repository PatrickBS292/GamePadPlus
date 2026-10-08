using System.IO;
using System.Text.Json;

namespace GamePadPlus.Services
{
    internal static class JsonFile
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            WriteIndented = true
        };

        public static void Write<T>(string filePath, T value)
        {
            string json = JsonSerializer.Serialize(value, Options);

            File.WriteAllText(filePath, json);
        }

        public static T? Read<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return default;
            }

            string json = File.ReadAllText(filePath);

            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
