using System.IO;

namespace GamePadPlus.Services
{
    public class AppSettingsService
    {
        private const string SettingsFileName = "GamePadsettings.json";

        public string GetSettingsFolder() {
            string desktopFolder = Environment.GetFolderPath(
                Environment.SpecialFolder.Desktop
            );

            return Path.Combine(desktopFolder, AppPaths.ApplicationFolderName);
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

            JsonFile.Write(GetSettingsFilePath(), settings);
        }

        public string? LoadLibraryLocation()
        {
            AppSettings? settings = JsonFile.Read<AppSettings>(GetSettingsFilePath());

            return settings?.LibraryLocation;
        }
    }

    public class AppSettings
    {
        public string? LibraryLocation { get; set; }
    }
}
