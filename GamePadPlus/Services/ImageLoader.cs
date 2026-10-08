using System.IO;
using System.Windows.Media.Imaging;

namespace GamePadPlus.Services
{
    public static class ImageLoader
    {
        public static BitmapImage? LoadFromFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
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
    }
}
