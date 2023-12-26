using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace GKMediaCreater.Converter
{
    public class PathToNameFileConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (File.Exists(value?.ToString()))
            {
                var nameWithoutExtension = Path.GetFileNameWithoutExtension(value?.ToString());
                var extension = Path.GetExtension(value?.ToString());
                if (nameWithoutExtension.Length > 25)
                {
                    nameWithoutExtension = nameWithoutExtension.Substring(0, 25) + "...";
                }
                return nameWithoutExtension + extension;
            }
            else
            {
                return "File không tồn tại ...";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
