using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public abstract class Object
    {
        public Double3[] Bounds { get; protected set; }
        public string Name { get; private set; }

        public Double3 MinDirection { get; protected set; }
        public Double3 MaxDirection { get; protected set; }

        public Object(string name)
        {
            Name = name;

            Bounds = new Double3[2] { 
                new Double3(double.PositiveInfinity), 
                new Double3(double.NegativeInfinity) };
        }

        public abstract void Preprocess(Scene scene);
        public abstract double Intersect(HitData data);
        public abstract void SetBounds();
    }
}
