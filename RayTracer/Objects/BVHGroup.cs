using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class BVHGroup : Group
    {
        public Object LeftGroup { get; set; }
        public Object RightGroup { get; set; }
        public List<Object> Objects { get; set; }

        public BVHGroup()
            : base("BVHGroup")
        {
            Objects = new List<Object>();
        }

        public BVHGroup(int capacity)
            : base("BVHGroup")
        {
            Objects = new List<Object>(capacity);
        }

        public override void Preprocess(Scene scene)
        {
            foreach (Object obj in Objects)
                obj.Preprocess(scene);

            CreateBVHTree();
        }

        private void CreateBVHTree()
        {
            int numObjects = Objects.Count;
            if (numObjects == 0)
                return;
            
            SetBounds();

            if (numObjects == 1)
            {
                LeftGroup = Objects[0];
                Objects.Clear();
                return;
            }
            else if (numObjects == 2)
            {
                LeftGroup = Objects[0];
                RightGroup = Objects[1];
                Objects.Clear();
                return;
            }

            SortObjectsByMaxAxis(this);
            
            // split the objects into two BVH
            LeftGroup = new BVHGroup();
            ((BVHGroup)LeftGroup).Objects = Objects.GetRange(0, numObjects / 2).ToList<Object>();
            Objects.RemoveRange(0, numObjects / 2);
            ((BVHGroup)LeftGroup).CreateBVHTree();

            RightGroup = new BVHGroup();
            ((BVHGroup)RightGroup).Objects = Objects.GetRange(0, Objects.Count).ToList<Object>();
            Objects.Clear();
            ((BVHGroup)RightGroup).CreateBVHTree();
        }

        private static void SortObjectsByMaxAxis(BVHGroup group)
        {
            // find the max axis and sort the bounds
            double xlen = group.Bounds[1].X - group.Bounds[0].X;
            double ylen = group.Bounds[1].Y - group.Bounds[0].Y;
            double zlen = group.Bounds[1].Z - group.Bounds[0].Z;

            if (xlen >= ylen && xlen >= zlen)
            {
                group.Objects.Sort((lhs, rhs) =>
                {
                    return lhs.Bounds[0].X.CompareTo(rhs.Bounds[0].X);
                });
            }
            else if (ylen > xlen && ylen > zlen)
            {
                group.Objects.Sort((lhs, rhs) =>
                {
                    return lhs.Bounds[0].Y.CompareTo(rhs.Bounds[0].Y);
                });
            }
            else if (zlen > xlen && zlen > ylen)
            {
                group.Objects.Sort((lhs, rhs) =>
                {
                    return lhs.Bounds[0].Z.CompareTo(rhs.Bounds[0].Z);
                });
            }
        }

        public override double Intersect(HitData data)
        {
            double distance = IntersectsBox(data.Ray, Bounds);

            if (distance < data.MinDistance)
            {
                LeftGroup.Intersect(data);
                if (RightGroup != null)
                {
                    RightGroup.Intersect(data);
                }
            }
            
            return data.MinDistance;
        }

        private static double IntersectsBox(Ray ray, Double3[] Bounds)
        {
            double tmin, tmax, tymin, tymax, tzmin, tzmax;
            tmin = (Bounds[ray.Sign[0]].X - ray.Origin.X) * ray.InvDirection.X;
            tmax = (Bounds[1 - ray.Sign[0]].X - ray.Origin.X) * ray.InvDirection.X;
            tymin = (Bounds[ray.Sign[1]].Y - ray.Origin.Y) * ray.InvDirection.Y;
            tymax = (Bounds[1 - ray.Sign[1]].Y - ray.Origin.Y) * ray.InvDirection.Y;
            if ((tmin > tymax) || (tymin > tmax))
                return Single.PositiveInfinity;
            if (tymin > tmin)
                tmin = tymin;
            if (tymax < tmax)
                tmax = tymax;
            tzmin = (Bounds[ray.Sign[2]].Z - ray.Origin.Z) * ray.InvDirection.Z;
            tzmax = (Bounds[1 - ray.Sign[2]].Z - ray.Origin.Z) * ray.InvDirection.Z;
            if ((tmin > tzmax) || (tzmin > tmax))
                return Single.PositiveInfinity;
            if (tzmin > tmin)
                tmin = tzmin;
            if (tzmax < tmax)
                tmax = tzmax;

            return (double)(tmax < tmin ? tmax : tmin);
        }        

        public override void AddObject(Object obj)
        {
            obj.SetBounds();
            Objects.Add(obj);
        }

        public override void SetBounds()
        {
            Bounds[0] = Double3.PositiveInfinity;
            Bounds[1] = Double3.NegativeInfinity;

            foreach (Object obj in Objects)
            {
                Bounds[0] = Double3.Minimum(obj.Bounds[0], Bounds[0]);
                Bounds[1] = Double3.Maximum(obj.Bounds[1], Bounds[1]);
            }
        }
    }
}
