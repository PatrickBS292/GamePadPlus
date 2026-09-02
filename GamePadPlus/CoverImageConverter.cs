using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace GamePadPlus.Services
{
    public class CoverImageConverter : IValueConverter
    {
        private readonly LibraryStorageService storageService =
            new LibraryStorageService();

        public object Convert(
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

            string filePath = Path.Combine(
                storageService.GetCoversFolder(),
                fileName
            );

            if (!File.Exists(filePath))
            {
                return null;
            }

            BitmapImage image = new BitmapImage();

            image.BeginInit();
            image.UriSource = new Uri(filePath);
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            image.EndInit();

            return image;
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