using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class HitData
    {
        public double MinDistance { get; set; }
        public Primitive Primitive { get; set; }
        public Ray Ray { get; set; }
        public int Depth { get; set; }
        public Double3 Attenuation { get; set; }
        
        public double u { get; set; }
        public double v { get; set; }

        public HitData()
        { }

        public HitData(Ray ray, double minDistance, Double3 attenuation)
        {
            Ray = ray;
            MinDistance = minDistance;
            Attenuation = attenuation;
        }

        public HitData(Ray ray, double minDistance, Double3 attenuation, int depth)
            : this(ray, minDistance, attenuation)
        {
            Depth = depth;
        }

        public Double3 HitPosition
        {
            get { return Ray.Origin + (Ray.Direction * MinDistance); }
        }

        public bool Hit(double distance, Primitive primitive)
        {
            if (distance < MinDistance && distance > Constants.EPSILON)
            {
                MinDistance = distance;
                Primitive = primitive;
                return true;
            }
            else
                return false;
        }

        public void GetHitData(out Double3 hitPosition, out Double3 normal)
        {
            hitPosition = Ray.Origin + (Ray.Direction * MinDistance);
            normal = Primitive.GetNormal(hitPosition);            
        }
    }
}
