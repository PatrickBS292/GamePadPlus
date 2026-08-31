using Microsoft.Win32;

namespace GamePadPlus.Services
{
    public class LibraryLocationService
    {
        public string? ChooseLibraryLocation()
        {
            OpenFolderDialog dialog = new OpenFolderDialog();

            dialog.Title = "Choose where you want to store your GamePad+ library";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                return dialog.FolderName;
            }

            return null;
        }
    }
}