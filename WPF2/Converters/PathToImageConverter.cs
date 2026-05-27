using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows;
using System.IO;

namespace WPF2.Converters
{
    public class PathToImageConverter : IValueConverter
    {
        private const string BasePath = "stankiDB/stankiDBpictures/";
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string fileName = value as string;
            if (fileName == null || string.IsNullOrEmpty(fileName))
                return null;
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, BasePath, fileName);
            if (!File.Exists(fullPath))
            {
                MessageBox.Show($"File not found:{fullPath}");
                System.Diagnostics.Debug.WriteLine($"[Image] Файл не найден: {fullPath}");
                return null;
            }
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(fullPath, UriKind.Absolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            bitmap.Freeze();
            return bitmap;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
