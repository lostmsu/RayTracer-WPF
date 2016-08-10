using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace RayTracer
{
    public class ObjectGroup : Group
    {
        public List<Object> Objects { get; private set; }

        public ObjectGroup()
            : base("ObjectGroup")
        {
            Objects = new List<Object>();
        }

        public ObjectGroup(int capacity)
            : base("BVHGroup")
        {
            Objects = new List<Object>(capacity);
        }

        public override void Preprocess(Scene scene)
        {
            foreach (Object obj in Objects)
            {
                obj.Preprocess(scene);
            }

            SetBounds();
        }

        public override double Intersect(HitData data)
        {
            foreach (Object obj in Objects)
            {
                obj.Intersect(data);
            }
            return data.MinDistance;
        }

        public override void AddObject(Object obj)
        {
            obj.SetBounds();
            Objects.Add(obj);
        }

        public override void SetBounds()
        {
            Bounds[0] = new Double3(Single.PositiveInfinity);
            Bounds[1] = new Double3(Single.NegativeInfinity);

            foreach (Object obj in Objects)
            {
                Bounds[0] = Double3.Minimum(obj.Bounds[0], Bounds[0]);
                Bounds[1] = Double3.Maximum(obj.Bounds[1], Bounds[1]);
            }
        }

        internal void SortLeftToRight(Ray leftRay, Ray rightRay)
        {
            Objects.Sort((a, b) =>
                {
                    double aMin = Math.Min(
                        a.Bounds[0].Dot(leftRay.Direction),
                        a.Bounds[1].Dot(leftRay.Direction));
                    double bMin = Math.Min(
                        b.Bounds[0].Dot(leftRay.Direction),
                        b.Bounds[1].Dot(leftRay.Direction));
                    return aMin.CompareTo(bMin);
                });
        }
    }
}
