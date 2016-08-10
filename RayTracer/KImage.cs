using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Collections.Generic;

namespace RayTracer
{
    public class KImage : SceneImage
    {
        int stride;
        byte[] pixels;

        public KImage(int width, int height)
            : base(width, height)
        {

        }

        #region IImage Members

        protected override void InitializeImageData()
        {
            stride = Width * 3;
            pixels = new byte[Height * stride];
        }

        public override void SetPixel(int x, int y, Double3 pixel)
        {
            int index1 = x * 3 + y * Width * 3;
            int index2 = index1 + 1;
            int index3 = index1 + 2;

            pixel.Clamp(0.0, 1.0);

            pixels[index1] = (byte)(pixel.X * 255);
            pixels[index2] = (byte)(pixel.Y * 255);
            pixels[index3] = (byte)(pixel.Z * 255);
        }

        public override Double3 GetPixel(double u, double v)
        {
            return GetPixel(u, v);
        }

        public Double3 GetPixel(int x, int y)
        {
            int index1 = x * 3 + y * Width * 3;
            int index2 = index1 + 1;
            int index3 = index1 + 2;

            return new Double3(pixels[index1], pixels[index2], pixels[index3]);
        }

        #endregion

        public override ImageSource UpdateImage(System.Windows.Int32Rect dirtyRect)
        {
            // Try creating a new image with a custom palette.
            List<Color> colors = new List<Color>();
            colors.Add(System.Windows.Media.Colors.Red);
            colors.Add(System.Windows.Media.Colors.Blue);
            colors.Add(System.Windows.Media.Colors.Green);
            BitmapPalette myPalette = new BitmapPalette(colors);

            // Creates a new empty image with the pre-defined palette
            BitmapSource bitmapSource = WriteableBitmap.Create(
                Width, Height,
                96, 96,
                PixelFormats.Rgb24,
                myPalette,
                pixels,
                stride);

            // Define parameters used to create the BitmapSource.
            return bitmapSource;
        }
    }
}
