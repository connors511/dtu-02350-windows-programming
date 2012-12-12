using System;
using System.Windows.Data;
using System.Globalization;
using System.Text.RegularExpressions;

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

    public class ArithmeticConverter : IValueConverter
    {
        private const string ArithmeticParseExpression = "([+\\-*/]{1,1})\\s{0,}(\\-?[\\d\\.]+)";
        private Regex arithmeticRegex = new Regex(ArithmeticParseExpression);

        #region IValueConverter Members

        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            if (value is double && parameter != null)
            {
                string param = parameter.ToString();

                if (param.Length > 0)
                {
                    Match match = arithmeticRegex.Match(param);
                    if (match != null && match.Groups.Count == 3)
                    {
                        string operation = match.Groups[1].Value.Trim();
                        string numericValue = match.Groups[2].Value;

                        double number = 0;
                        if (double.TryParse(numericValue, out number)) // this should always succeed or our regex is broken
                        {
                            double valueAsDouble = (double)value;
                            double returnValue = 0;

                            switch (operation)
                            {
                                case "+":
                                    returnValue = valueAsDouble + number;
                                    break;

                                case "-":
                                    returnValue = valueAsDouble - number;
                                    break;

                                case "*":
                                    returnValue = valueAsDouble * number;
                                    break;

                                case "/":
                                    returnValue = valueAsDouble / number;
                                    break;
                            }

                            return returnValue;
                        }
                    }
                }
            }

            return null;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
