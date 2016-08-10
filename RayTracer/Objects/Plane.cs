using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class Plane : Primitive
    {
        public Double3 Point { get; private set; }
        public Double3 Normal { get; private set; }
        public double Offset { get; private set; }

        public Plane(Material material, Double3 point, Double3 normal)
            : base(material, "Plane")
        {
            Point = point;
            Normal = normal;
        }

        public override Double3 GetNormal(Double3 hitPosition)
        {
            return Normal;
        }

        public override void Preprocess(Scene scene)
        {
            Normal.Normalize();
            Offset = Normal.Dot(Point);
        }

        public override double Intersect(HitData data)
        {
            Ray ray = data.Ray;
            double denom = Normal.Dot(ray.Direction);

            if (denom < Constants.EPSILON)
            {
                double NDotO = Normal.Dot(ray.Origin);
                double t = (Offset - NDotO) / denom;
                data.Hit(t, this);
            }
	
	        return data.MinDistance;
        }

        public override void SetBounds()
        {
            // We could make this more realistic, but oh maybe later if it's worth it.
            Bounds[0] = new Double3(double.MaxValue);
            Bounds[1] = new Double3(double.MinValue);
        }
    }
}
