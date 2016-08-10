using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class Box : Primitive
    {
        public Double3 Corner0 { get; private set; }
        public Double3 Corner1 { get; private set; }

        public Box(Material material, Double3 corner0, Double3 corner1)
            : base(material, "Box")
        {
            Corner0 = corner0;
            Corner1 = corner1;
        }

        public override Double3 GetNormal(Double3 hitPosition)
        {
            Double3 normal;

            // this assumes axis aligned.
            if (Math.Abs(hitPosition.X - Corner0.X) < Constants.EPSILON)
                normal = -Double3.XAxis;
            else if (Math.Abs(hitPosition.X - Corner1.X) < Constants.EPSILON)
                normal = Double3.XAxis;
            else if (Math.Abs(hitPosition.Y - Corner0.Y) < Constants.EPSILON)
                normal = -Double3.YAxis;
            else if (Math.Abs(hitPosition.Y - Corner1.Y) < Constants.EPSILON)
                normal = Double3.YAxis;
            else if (Math.Abs(hitPosition.Z - Corner0.Z) < Constants.EPSILON)
                normal = -Double3.ZAxis;
            else
                normal = Double3.ZAxis;
            return normal;
        }

        public override void Preprocess(Scene scene)
        {

        }

        public override double Intersect(HitData data)
        {
            Ray ray = data.Ray;
            double tmin, tmax, tymin, tymax, tzmin, tzmax;
            tmin = (Bounds[ray.Sign[0]].X - ray.Origin.X) * ray.InvDirection.X;
            tmax = (Bounds[1 - ray.Sign[0]].X - ray.Origin.X) * ray.InvDirection.X;
            tymin = (Bounds[ray.Sign[1]].Y - ray.Origin.Y) * ray.InvDirection.Y;
            tymax = (Bounds[1 - ray.Sign[1]].Y - ray.Origin.Y) * ray.InvDirection.Y;
            if ((tmin > tymax) || (tymin > tmax))
                return double.PositiveInfinity;
            if (tymin > tmin)
                tmin = tymin;
            if (tymax < tmax)
                tmax = tymax;
            tzmin = (Bounds[ray.Sign[2]].Z - ray.Origin.Z) * ray.InvDirection.Z;
            tzmax = (Bounds[1 - ray.Sign[2]].Z - ray.Origin.Z) * ray.InvDirection.Z;
            if ((tmin > tzmax) || (tzmin > tmax))
                return double.PositiveInfinity;
            if (tzmin > tmin)
                tmin = tzmin;
            if (tzmax < tmax)
                tmax = tzmax;

            data.Hit(tmin, this);
            data.Hit(tmax, this);

            return data.MinDistance;
        }

        public override void SetBounds()
        {
            Bounds[0] = Double3.Minimum(Corner0, Corner1);
            Bounds[1] = Double3.Maximum(Corner0, Corner1);
        }
    }
}
