using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using RayTracer.WPF;
using System.Windows;

namespace RayTracer
{
    public class RenderContext : NotifyObject
    {
        public Scene Scene { get; private set; }
        public int MinX { get; set; }
        public int MinY { get; set; }
        public int MaxX { get; set; }
        public int MaxY { get; set; }

        private string _RenderTime;
        public string RenderTime
        {
            get { return _RenderTime; }
            set
            {
                _RenderTime = value;
                OnPropertyChanged("RenderTime");
            }
        }

        public RenderContext(Scene scene, int minX, int minY, int maxX, int maxY)
        {
            Scene = scene;

            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;
        }

        public void Preprocess()
        {
            if (Scene.IsDirty)
                Scene.Preprocess();
        }

        public void RenderScene()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Scene.RenderScene(MinX, MinY, MaxX, MaxY);

            stopwatch.Stop();
            RenderTime = (stopwatch.ElapsedMilliseconds / 1000.0).ToString();
        }
    }
}
