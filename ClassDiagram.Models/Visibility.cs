using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace ClassDiagram.Models
{
    public enum Visibility
    {
        Public,
        Private,
        Protected,
    }

    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility v = (Visibility)value;

            switch (v)
            {
                case Visibility.Public:
                    return "+";
                case Visibility.Protected:
                    return "#";
                case Visibility.Private:
                    return "-";
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
