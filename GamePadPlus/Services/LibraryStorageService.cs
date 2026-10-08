using System.IO;
using GamePadPlus.Models;

namespace GamePadPlus.Services
{
    public class LibraryStorageService
    {
        private const string LibraryFileName = "library.json";
        private const string CoversFolderName = "Covers";

        private readonly AppSettingsService settingsService =
            new AppSettingsService();

        public string GetDataFolder()
        {
            string? selectedLocation =
                settingsService.LoadLibraryLocation();

            if (string.IsNullOrWhiteSpace(selectedLocation))
            {
                return string.Empty;
            }

            return Path.Combine(
                selectedLocation,
                AppPaths.ApplicationFolderName
            );
        }

        public string GetLibraryFilePath()
        {
            return Path.Combine(
                GetDataFolder(),
                LibraryFileName
            );
        }

        public string GetCoversFolder()
        {
            return Path.Combine(
                GetDataFolder(),
                CoversFolderName
            );
        }

        public string GetCoverImagePath(string imageFileName)
        {
            return Path.Combine(
                GetCoversFolder(),
                imageFileName
            );
        }

        public void EnsureStorageExists()
        {
            Directory.CreateDirectory(GetDataFolder());
            Directory.CreateDirectory(GetCoversFolder());
        }

        public void SaveLibrary(IEnumerable<Game> games)
        {
            EnsureStorageExists();

            JsonFile.Write(GetLibraryFilePath(), games);
        }

        public List<Game> LoadLibrary()
        {
            string dataFolder = GetDataFolder();

            if (string.IsNullOrWhiteSpace(dataFolder))
            {
                return new List<Game>();
            }

            EnsureStorageExists();

            return JsonFile.Read<List<Game>>(GetLibraryFilePath())
                ?? new List<Game>();
        }
    }
}
