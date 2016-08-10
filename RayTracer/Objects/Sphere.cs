using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class Sphere : Primitive
    {
        public double Radius { get; private set; }
        public Double3 Center { get; private set; }

        public Sphere(Material material, double radius, Double3 center)
            : base(material, "Sphere")
        {
            Radius = radius;
            Center = center;
        }

        public override Double3 GetNormal(Double3 hitPosition)
        {
            Double3 normal = hitPosition - Center;
            normal.Normalize();
            return normal;
        }

        public override void Preprocess(Scene scene)
        {
        }

        public override double Intersect(HitData data)
        {
            Ray ray = data.Ray;
            Double3 e = ray.Origin;
            Double3 d = ray.Direction;
            Double3 c = Center;

            // vector from ray origin to center of sphere.
            Double3 ec = c - e;
            
            // sphere center is behind the eye so return false for now
            if (ec.Dot(d) < 0)            
                return Single.PositiveInfinity;            
            else
            {
                Double3 pec = d * (d.Dot(ec)); // projection of ec vector onto ray
                double dist = (pec - ec).Length();

                // projection point is not inside the sphere.
                // if dist == r then it is on the surface.
                if (dist >= Radius)                
                    return Single.PositiveInfinity;                
                else
                {
                    // calculate the two distances from the eye to a sphere intersections.
                    double temp = (double)Math.Sqrt(Radius * Radius - dist * dist);
                    double pecLength = pec.Length();
             
                    data.Hit(pecLength - temp, this);
                    data.Hit(pecLength + temp, this);
                    
                    return data.MinDistance;
                }
            }
        }


        public override void SetBounds()
        {
            Double3 d1 = Center - Radius;
            Double3 d2 = Center + Radius;

            Bounds[0] = Double3.Minimum(d1, d2);
            Bounds[1] = Double3.Maximum(d1, d2);
        }
    }
}
