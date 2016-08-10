using System;
using System.Windows;
using RayTracer.WPF;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;

namespace RayTracer
{
    public abstract class SceneImage : NotifyObject
    {
        public SceneImage(int width, int height)
        {
            _Width = width;
            _Height = height;
            PropertyChanged += SceneImage_PropertyChanged;
            InitializeImageData();
        }

        protected SceneImage()
        {

        }

        public abstract void SetPixel(int x, int y, Double3 pixel);
        public abstract Double3 GetPixel(double u, double v);
        public abstract ImageSource UpdateImage(Int32Rect dirtyRect);
        protected abstract void InitializeImageData();

        void SceneImage_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Width":
                case "Height":
                    InitializeImageData();
                    break;
                default:
                    throw new NotImplementedException(e.PropertyName + " is not implemented in KImage");
            }
        }

        private int _Width;
        public int Width
        {
            get { return _Width; }
            set
            {
                if (_Width != value)
                {
                    _Width = value;
                    OnPropertyChanged("Width");
                }
            }
        }

        private int _Height;
        public int Height
        {
            get { return _Height; }
            set
            {
                if (_Height != value)
                {
                    _Height = value;
                    OnPropertyChanged("Height");
                }
            }
        }
    }
}
