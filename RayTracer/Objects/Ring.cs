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
                double dist = (p - Center).Length();
                if (dist <= OuterRadius && dist >= InnerRadius)
                    data.Hit(t, this);
            }
            return data.MinDistance;
        }

        public override void SetBounds()
        {
            Double3 u = Double3.XAxis.Cross(Normal);
            u.Normalize();

            Double3 d1 = Center - u * OuterRadius;
            Double3 d2 = Center + u * OuterRadius;

            Bounds[0] = Double3.Minimum(d1, d2);
            Bounds[1] = Double3.Maximum(d1, d2);

            u = Double3.YAxis.Cross(Normal);
            u.Normalize();

            d1 = Center - u * OuterRadius;
            d2 = Center + u * OuterRadius;

            Bounds[0] = Double3.Minimum(Bounds[0], Double3.Minimum(d1, d2));
            Bounds[1] = Double3.Maximum(Bounds[1], Double3.Maximum(d1, d2));

            u = Double3.ZAxis.Cross(Normal);
            u.Normalize();

            d1 = Center - u * OuterRadius;
            d2 = Center + u * OuterRadius;

            Bounds[0] = Double3.Minimum(Bounds[0], Double3.Minimum(d1, d2));
            Bounds[1] = Double3.Maximum(Bounds[1], Double3.Maximum(d1, d2));
        }
    }
}
