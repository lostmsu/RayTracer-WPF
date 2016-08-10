using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class Disk : Primitive
    {
        public Double3 Normal { get; private set; }
        public Double3 Center { get; private set; }
        public double Radius { get; private set; }
        
        private double Offset;

        public Disk(Material material, Double3 normal, Double3 center, double radius)
            : base(material, "Disk")
        {
            Normal = normal;
            Center = center;
            Radius = radius;
        }

        public override Double3 GetNormal(Double3 hitPosition)
        {
            return Normal;
        }

        public override void Preprocess(Scene scene)
        {
            Normal.Normalize();
            Offset = Normal.Dot(Center);
        }

        public override double Intersect(HitData data)
        {
            Ray ray = data.Ray;
            double product = Normal.Dot(ray.Direction);
            if (Math.Abs(product) > Constants.EPSILON)
            {
                double t = (Offset - Normal.Dot(ray.Origin)) / product;

                Double3 p = ray.Origin + ray.Direction * t;
                Double3 dist = p - Center;
                if (dist.Length() <= Radius)                
                    data.Hit(t, this);                
            }
            return data.MinDistance;
        }

        public override void SetBounds()
        {
            Double3 u = Double3.XAxis.Cross(Normal);
            u.Normalize();
            
            Double3 d1 = Center - u * Radius;
            Double3 d2 = Center + u * Radius;

            Bounds[0] = Double3.Minimum(d1, d2);
            Bounds[1] = Double3.Maximum(d1, d2);

            u = Double3.YAxis.Cross(Normal);
            u.Normalize();

            d1 = Center - u * Radius;
            d2 = Center + u * Radius;

            Bounds[0] = Double3.Minimum(Bounds[0], Double3.Minimum(d1, d2));
            Bounds[1] = Double3.Maximum(Bounds[1], Double3.Maximum(d1, d2));

            u = Double3.ZAxis.Cross(Normal);
            u.Normalize();

            d1 = Center - u * Radius;
            d2 = Center + u * Radius;

            Bounds[0] = Double3.Minimum(Bounds[0], Double3.Minimum(d1, d2));
            Bounds[1] = Double3.Maximum(Bounds[1], Double3.Maximum(d1, d2));
        }
    }
}
