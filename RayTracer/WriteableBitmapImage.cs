using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace RayTracer
{
    public class WriteableBitmapImage : SceneImage
    {
        Int32[] pixels;

        WriteableBitmap writeableBitmap;

        public WriteableBitmapImage(Int32 width, Int32 height)
            : base(width, height)
        {
        }

        public WriteableBitmapImage(ImageSource imageSource)
            : base ()
        {                     
            BitmapSource bitmapSource = imageSource as BitmapSource;
            if (bitmapSource != null)
            {
                writeableBitmap = new WriteableBitmap(bitmapSource);
                Width = writeableBitmap.PixelWidth;
                Height = writeableBitmap.PixelHeight;
                SetPixelsEqualToBitmap();
            }
            else
                throw new NotImplementedException();        
        }

        private void SetPixelsEqualToBitmap()
        {
            pixels = new Int32[Height * Width];

            unsafe
            {
                // Get a pointer to the back buffer.
                int pBackBuffer = (int)writeableBitmap.BackBuffer;

                for (int i = 0; i < Width * Height; i++)
                {
                    // Find the address of the pixel to draw.
                    pBackBuffer += 4;

                    // Assign the color data to the pixel.
                    int color_data = *((int*)pBackBuffer);
                    pixels[i] = color_data;
                }
            }
        }

        public void SetPixel(Int32 x, Int32 y, Int32 value)
        {
            pixels[x + y * Width] = value;
        }

        #region IImage Members

        protected override void InitializeImageData()
        {
            pixels = new Int32[Height * Width];

            writeableBitmap = new WriteableBitmap(
                Width,
                Height,
                96,
                96,
                PixelFormats.Bgr32,
                null);     
        }

        public override void SetPixel(Int32 x, Int32 y, Double3 pixel)
        {
            // Compute the pixel's color.
            pixel.Clamp(0.0, 1.0);
            Int32 color_data  = (Int32)(pixel.X * 255) << 16;  // R
                  color_data |= (Int32)(pixel.Y * 255) << 8;  // G
                  color_data |= (Int32)(pixel.Z * 255) << 0;  // B

            SetPixel(x, y, color_data);
        }

        public override Double3 GetPixel(double u, double v)
        {
            // right now of course this is nearest neighbor, we could make settings where it could merge values a little.
            int x = (Width + (int)(u * Width)) % Width;
            int y = (Height + (int)(v * Height)) % Height;
            
            return GetPixel(x, y);
        }

        public Double3 GetPixel(int x, int y)
        {
            return Double3.CreateFromInt32(pixels[x + y * Width]);
        }

        public override ImageSource UpdateImage(Int32Rect dirtyRect)
        {
            writeableBitmap.WritePixels(dirtyRect, pixels, writeableBitmap.BackBufferStride, 0);
            return writeableBitmap;            
        }

        #endregion
    }
}
