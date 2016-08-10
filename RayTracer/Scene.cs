using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading;

namespace RayTracer
{
    public class Scene
    {
        public SceneImage Image { get; set; }
        public Group Group { get; set; }
        public Camera Camera { get; set; }
        public Background Background { get; set; }
        public Light[] Lights { get; set; }
        public bool IsDirty { get; set; }
        public int Samples { get; set; }

        // more images, maybe for faster rasterization
        

        private Double3 _Ambient;
        public Double3 Ambient
        {
            get { return _Ambient ?? (_Ambient = new Double3()); } // this is when Double3 is a class, instead of a struct 
            set { _Ambient = value; }
        }

        private int _MaxDepth;
        public int MaxDepth
        {
            get { return _MaxDepth; }
            set
            {
                if (value != _MaxDepth)
                {
                    _MaxDepth = value;
                    IsDirty = true;
                }
            }
        }

        private double _MinAttenuation;
        public double MinAttenuation
        {
            get { return _MinAttenuation; }
            set
            {
                if (value != _MinAttenuation)
                {
                    _MinAttenuation = value;
                    IsDirty = true;
                }
            }
        }

        public Scene(SceneImage image, Camera camera, Group group, List<Light> lights, Background background, int maxDepth, double minAttenuation, int samples)
        {
            Image = image;
            Camera = camera;
            Group = group;
            if (lights != null)
                Lights = lights.ToArray();
            Background = background;
            MaxDepth = maxDepth;
            MinAttenuation = minAttenuation;
            Samples = samples;
            
            Image.PropertyChanged += Image_PropertyChanged;

            IsDirty = true;
        }

        void Image_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IsDirty = true;
        }

        public void Preprocess()
        {
            Camera.Preprocess(this);
            Group.Preprocess(this);            

            IsDirty = false;
        }

        public bool TraceShadowRay(HitData hitData)
        {
            Group.Intersect(hitData);            
            return hitData.Primitive != null;
        }

        public virtual void RenderScene(int minX, int minY, int maxX, int maxY)
        {
            if (IsDirty || Camera.IsDirty)
                Preprocess();

            Double3 pixelColor = new Double3();

            for (int y = minY; y < maxY; y++)
            {
                for (int x = minX; x < maxX; x++)
                {
                    pixelColor.X = pixelColor.Y = pixelColor.Z = 0.0;
                    double minHit = Double.PositiveInfinity;

                    for (int i = 0; i < Samples; i++)
                    {
                        Ray ray = Camera.MakeRay(x, y);

                        HitData data = new HitData(ray, Double.PositiveInfinity, Double3.One);
                        Double3 color = Background.GetColor();

                        if (Group.Intersect(data) < Double.PositiveInfinity)
                        {
                            minHit = data.MinDistance;
                            data.Primitive.Material.Shade(out color, data, this);
                        }
                        pixelColor.Add(color);
                    }

                    Image.SetPixel(x, y, pixelColor / Samples);
                }
            }
        }

        public void TraceRay(int x, int y)
        {
            Ray ray = Camera.MakeRay(x, y);

            Double3 pixelColor = TraceRay(ray);

            Image.SetPixel(x, y, pixelColor);
        }

        public Double3 TraceRay(Ray ray)
        {
            HitData data = new HitData(ray, Single.PositiveInfinity, new Double3(1.0f));
            Double3 pixelColor = Background.GetColor();

            if (Group.Intersect(data) < Single.PositiveInfinity)
                data.Primitive.Material.Shade(out pixelColor, data, this);

            return pixelColor;
        }
    }
}
