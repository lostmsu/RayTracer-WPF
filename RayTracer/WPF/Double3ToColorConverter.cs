using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Diagnostics;
using System.Windows.Media;
using System.Globalization;

namespace RayTracer.WPF
{
    public class Double3ToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var float3 = Double3.Parse(value.ToString());
            return float3.ToMediaColor();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Double3((Color)value);
        }
    }
}
