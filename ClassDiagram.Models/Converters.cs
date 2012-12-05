using System;
using System.Windows.Data;
using System.Globalization;

namespace ClassDiagram.Models
{
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

    public class TypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            eType v = (eType)value;

            switch (v)
            {
                case eType.AbstractClass:
                    return "Abstract Class";
                case eType.Class:
                    return "Class";
                case eType.Enum:
                    return "Enum";
                case eType.Struct:
                    return "Struct";
                case eType.Inherit:
                    return "Inherit";
                case eType.Association:
                    return "Association";
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public sealed class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var flag = false;
            if (value is bool)
            {
                flag = (bool)value;
            }
            else if (value is bool?)
            {
                var nullable = (bool?)value;
                flag = nullable.GetValueOrDefault();
            }
            if (parameter != null)
            {
                if (bool.Parse((string)parameter))
                {
                    flag = !flag;
                }
            }
            if (flag)
            {
                return System.Windows.Visibility.Collapsed;
            }
            else
            {
                return System.Windows.Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var back = ((value is System.Windows.Visibility) && (((System.Windows.Visibility)value) == System.Windows.Visibility.Collapsed));
            if (parameter != null)
            {
                if ((bool)parameter)
                {
                    back = !back;
                }
            }
            return back;
        }
    }
}
