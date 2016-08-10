using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace RayTracer
{
    public class Triangle : Primitive
    {
        public Double3 Point0 { get; private set; }
        public Double3 Point1 { get; private set; }
        public Double3 Point2 { get; private set; }
        public Point Texture0 { get; private set; }
        public Point Texture1 { get; private set; }
        public Point Texture2 { get; private set; }
        public Double3 Normal { get; private set; }
        private Double3 Edge0;
        private Double3 Edge1;
        private Vector TextureEdge0;
        private Vector TextureEdge1;

        public Triangle(Material material, Double3 p0, Double3 p1, Double3 p2)
            : base(material, "Triangle")
        {
            Point0 = p0;
            Point1 = p1;
            Point2 = p2;

            Texture0 = Texture1 = Texture2 = new Point();

            // calculate texture coordinates?
            //double minX = Math.Min(p0.X, Math.Min(p1.X, p2.X));
            //double minY = Math.Min(p0.Y, Math.Min(p1.Y, p2.Y));
            //double minZ = Math.Min(p0.Z, Math.Min(p1.Z, p2.Z));

            //double maxX = Math.Min(p0.X, Math.Min(p1.X, p2.X));
            //double maxY = Math.Min(p0.Y, Math.Min(p1.Y, p2.Y));
            //double maxZ = Math.Min(p0.Z, Math.Min(p1.Z, p2.Z));
        }

        public Triangle(Material material, Double3 p0, Double3 p1, Double3 p2, Point t0, Point t1, Point t2)
            : base(material, "Triangle")
        {
            Point0 = p0;
            Point1 = p1;
            Point2 = p2;

            Texture0 = t0;
            Texture1 = t1;
            Texture2 = t2;
        }

        public override Double3 GetNormal(Double3 hitPosition)
        {
            return Normal;
        }

        public override void Preprocess(Scene scene)
        {
            Edge0 = Point0 - Point2;
            Edge1 = Point1 - Point2;
            TextureEdge0 = Texture0 - Texture2;
            TextureEdge1 = Texture1 - Texture2;
            Normal = Edge0.Cross(Edge1);
            Normal.Normalize();
        }

        
        public override double Intersect(HitData data)
        {
            Ray ray = data.Ray;
            Double3 u = ray.Direction.Cross(Edge1);
            double a = Edge0.Dot(u);

            if (Math.Abs(a) < Constants.EPSILON)
                return double.PositiveInfinity; // no hit

            double b = 1.0 / a;
            Double3 v = ray.Origin - Point2;
            double c = b * (v.Dot(u));

            if (c < 0.0 || c > 1.0)
                return double.PositiveInfinity; // no hit

            Double3 w = v.Cross(Edge0);
            double d = b * (ray.Direction.Dot(w));
            if (d < 0.0 || c + d > 1.0)
                return double.PositiveInfinity; // no hit

            // finally find the distance where there is an intersection
            if (data.Hit(b * Edge1.Dot(w), this))
            {
                // set the texture coordinates
                data.u = d * TextureEdge1.X + c * TextureEdge0.X;
                data.v = c * TextureEdge0.Y + d * TextureEdge1.Y;
            }

            return data.MinDistance;
        }

        public override void SetBounds()
        {
            Bounds[0] = Double3.Minimum(Point0, Point1);
            Bounds[0] = Double3.Minimum(Bounds[0], Point2);

            Bounds[1] = Double3.Maximum(Point0, Point1);
            Bounds[1] = Double3.Maximum(Bounds[1], Point2);             
        }
    }
}
