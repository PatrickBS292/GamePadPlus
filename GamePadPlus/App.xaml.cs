using System.Windows;
using GamePadPlus.Services;

namespace GamePadPlus
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            AppSettingsService settingsService =
                new AppSettingsService();

            string? libraryLocation =
                settingsService.LoadLibraryLocation();

            if (string.IsNullOrWhiteSpace(libraryLocation))
            {
                LibraryLocationService locationService =
                    new LibraryLocationService();

                string? selectedLocation =
                    locationService.ChooseLibraryLocation();

                if (string.IsNullOrWhiteSpace(selectedLocation))
                {
                    Shutdown();
                    return;
                }
                settingsService.SaveLibraryLocation(selectedLocation);
            }
        }
    }
}