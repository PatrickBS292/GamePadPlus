using System;
using System.IO;
using System.Text.Json;

namespace GamePadPlus.Services
{
    public class AppSettingsService
    {
        private const string ApplicationFolderName = "GamePadPlus";
        private const string SettingsFileName = "settings.json";

        public string GetSettingsFolder()
        {
            string localAppDataFolder = Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData
            );

            return Path.Combine(localAppDataFolder, ApplicationFolderName);
        }

        public string GetSettingsFilePath()
        {
            return Path.Combine(
                GetSettingsFolder(),
                SettingsFileName
            );
        }

        public void SaveLibraryLocation(string libraryLocation)
        {
            Directory.CreateDirectory(GetSettingsFolder());

            AppSettings settings = new AppSettings
            {
                LibraryLocation = libraryLocation
            };

            string json = JsonSerializer.Serialize(
                settings,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            File.WriteAllText(
                GetSettingsFilePath(),
                json
            );
        }

        public string? LoadLibraryLocation()
        {
            string filePath = GetSettingsFilePath();

            if (!File.Exists(filePath))
            {
                return null;
            }

            string json = File.ReadAllText(filePath);

            AppSettings? settings =
                JsonSerializer.Deserialize<AppSettings>(json);

            return settings?.LibraryLocation;
        }
    }

    public class AppSettings
    {
        public string? LibraryLocation { get; set; }
    }
}