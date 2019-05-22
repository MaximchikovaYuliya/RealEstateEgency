﻿using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace REA.Views
{
    public class VisibilityToNullableBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility)
            {
                return ((Visibility)value) == Visibility.Visible;
            }
            else
            {
                return Binding.DoNothing;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool?)
            {
                return ((bool?)value) == true ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (value is bool)
            {
                return ((bool?)value) == true ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                return Binding.DoNothing;
            }
        }
    }
}
