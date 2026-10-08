using System.Globalization;
using System.Windows.Data;

namespace GamePadPlus.Services
{
    public class CoverImageConverter : IValueConverter
    {
        private readonly LibraryStorageService storageService =
            new LibraryStorageService();

        public object? Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            if (value is not string fileName ||
                string.IsNullOrWhiteSpace(fileName))
            {
                return null;
            }

            string filePath = storageService.GetCoverImagePath(fileName);

            return ImageLoader.LoadFromFile(filePath);
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
