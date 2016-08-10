using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace WpfControls
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        public ColorPicker()
        {
            SetUpBindings();
            
            InitializeComponent();
           
            // for some reason, I have to create the CheckerboardEffect here or else there will be a exception
            //ColorBorder.Effect = checkerEffect;
        }

        #region Properties

        public string Title { get; set; }
        
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color", typeof(Color), typeof(ColorPicker), new PropertyMetadata(Color_Changed));

        private static readonly DependencyProperty InternalColorProperty = DependencyProperty.Register(
            "InternalColor", typeof(Color), typeof(ColorPicker), new PropertyMetadata(InternalColor_Changed));

        public static readonly DependencyProperty AlphaProperty = DependencyProperty.Register(
            "Alpha", typeof(byte), typeof(ColorPicker));

        public static readonly DependencyProperty RedProperty = DependencyProperty.Register(
            "Red", typeof(byte), typeof(ColorPicker));

        public static readonly DependencyProperty GreenProperty = DependencyProperty.Register(
            "Green", typeof(byte), typeof(ColorPicker));

        public static readonly DependencyProperty BlueProperty = DependencyProperty.Register(
            "Blue", typeof(byte), typeof(ColorPicker));

        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public Color InternalColor
        {
            get { return (Color)GetValue(InternalColorProperty); }
            set { SetValue(InternalColorProperty, value); }
        }

        public byte Alpha
        {
            get { return (byte)GetValue(AlphaProperty); }
            set { SetValue(AlphaProperty, value); }
        }

        public byte Red
        {
            get { return (byte)GetValue(RedProperty); }
            set { SetValue(RedProperty, value); }
        }

        public byte Green
        {
            get { return (byte)GetValue(GreenProperty); }
            set { SetValue(GreenProperty, value); }
        }

        public byte Blue
        {
            get { return (byte)GetValue(BlueProperty); }
            set { SetValue(BlueProperty, value); }
        }

        #endregion Properties

        private static void InternalColor_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ((ColorPicker)sender).SetValue(ColorProperty, (Color)args.NewValue);
        }

        private static void Color_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ((ColorPicker)sender).SetValue(InternalColorProperty, (Color)args.NewValue);
        }

        private void SetUpBindings()
        {
            // note that this order matters because the converter assumes they came in this order ARGB.
            var multiBinding = new MultiBinding();

            multiBinding.Converter = new ByteColorMultiConverter();
            multiBinding.Mode = BindingMode.TwoWay;

            var alphaBinding = new Binding("Alpha");
            alphaBinding.Source = this;
            alphaBinding.Mode = BindingMode.TwoWay;
            multiBinding.Bindings.Add(alphaBinding);

            var redBinding = new Binding("Red");
            redBinding.Source = this;
            redBinding.Mode = BindingMode.TwoWay;
            multiBinding.Bindings.Add(redBinding);

            var greenBinding = new Binding("Green");
            greenBinding.Source = this;
            greenBinding.Mode = BindingMode.TwoWay;
            multiBinding.Bindings.Add(greenBinding);

            var blueBinding = new Binding("Blue");
            blueBinding.Source = this;
            blueBinding.Mode = BindingMode.TwoWay;
            multiBinding.Bindings.Add(blueBinding);

            this.SetBinding(InternalColorProperty, multiBinding);
        }
    }

    public class ByteColorMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length != 4)
                throw new ArgumentException("There should be 4 values");

            var alpha = (byte)values[0];
            var red = (byte)values[1];
            var green = (byte)values[2];
            var blue = (byte)values[3];
            
            return Color.FromArgb(alpha, red, green, blue);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            Color color = (Color)value;

            return new object[] { color.A, color.R, color.G, color.B };
        }
    }

    [ValueConversion(typeof(byte), typeof(double))]
    public class ByteToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (double)((double)value / byte.MaxValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ColorGradientConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string param = parameter as string;
            Color color = (Color)value;
            if (param == null)
                throw new ArgumentException("parameter not a string");
            switch (param)
            {
                case "Alpha":
                    return new LinearGradientBrush(Color.FromArgb(0, color.R, color.G, color.B), Color.FromArgb(byte.MaxValue, color.R, color.G, color.B), 0);
                case "Red":
                    return new LinearGradientBrush(Color.FromArgb(color.A, 0, color.G, color.B), Color.FromArgb(color.A, byte.MaxValue, color.G, color.B), 0);
                case "Green":
                    return new LinearGradientBrush(Color.FromArgb(color.A, color.R, 0, color.B), Color.FromArgb(color.A, color.R, byte.MaxValue, color.B), 0);
                case "Blue":
                    return new LinearGradientBrush(Color.FromArgb(color.A, color.R, color.G, 0), Color.FromArgb(color.A, color.R, color.G, byte.MaxValue), 0);
                default:
                    throw new ArgumentException(param + " is not valid value");
            }    
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
