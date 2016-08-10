using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class Ring : Primitive
    {
        public Double3 Normal { get; private set; }
        public Double3 Center { get; private set; }
        public double InnerRadius { get; private set; }
        public double OuterRadius { get; private set; }

        private double Offset;

        public Ring(Material material, Double3 normal, Double3 center, double innerRadius, double outerRadius)
            : base(material, "Ring")
        {
            Normal = normal;
            Center = center;
            InnerRadius = innerRadius;
            OuterRadius = outerRadius;
        }

        public override Double3 GetNormal(Double3 hitPosition)
        {
            return Normal;
        }

        public override void Preprocess()
        {
            Normal.Normalize();
            Offset = Normal.Dot(Center);
        }

        public override double Intersect(Ray ray, HitData data)
        {
            double product = Normal.Dot(ray.Direction);
            if (Math.Abs(product) > Constants.EPSILON)
            {
                double t = (Offset - Normal.Dot(ray.Origin)) / product;

                Double3 p = ray.Origin + ray.Direction * t;
                double dist = (p - Center).Length();
                if (dist <= OuterRadius && dist >= InnerRadius)
                    data.Hit(ray, t, this);
            }
            return data.MinDistance;
        }

        public override void SetBounds()
        {
            Double3 u = Center.Cross(Normal);
            u.Normalize();
            Bounds[0] = Center - u * OuterRadius;
            Bounds[0] = Center + u * OuterRadius; 
        }

        public override void GetTextureCoordinates(out double u, out double v, Double3 hitPosition)
        {
            throw new NotImplementedException();
        }
    }
}
