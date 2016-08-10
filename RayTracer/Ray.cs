using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class Ray
    {
        public Double3 Origin { get; private set; }
        public Double3 Direction { get; private set; }
        public Double3 InvDirection { get; private set; }
        public int[] Sign { get; private set; }

        public Ray(Double3 origin, Double3 direction)
        {
            Origin = origin;
            Direction = direction;

            InvDirection = 1.0f / Direction;
            Sign = new int[3] { 
                InvDirection.X < 0 ? 1 : 0, 
                InvDirection.Y < 0 ? 1 : 0, 
                InvDirection.Z < 0 ? 1 : 0
            };
        }
    }
}
