using System;
using System.Windows.Media;
using System.Windows.Data;
using System.Globalization;
using System.Diagnostics;

namespace RayTracer.WPF
{
    public class Double3ToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.Assert(targetType == typeof(Brush));

            var float3 = Double3.Parse(value.ToString());
            return new SolidColorBrush(float3.ToMediaColor());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
