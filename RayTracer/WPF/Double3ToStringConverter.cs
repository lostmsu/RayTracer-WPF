using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace RayTracer.WPF
{
    public class Double3ToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.Assert(targetType == typeof(String));

            var float3String = value.ToString();
            return float3String;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.Assert(targetType == typeof(Double3));

            var float3 = Double3.Parse((string)value);
            return float3;
        }
    }
}
