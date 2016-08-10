using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public static class Constants
    {
        public const double EPSILON = 1e-6;
       
        public static bool IsPowerOfTwo(int value)
        {
            return value > 0 && ((value & (value - 1)) == 0);
        }

        private static Random _Random = new Random();
        public static Random Random
        {
            get { return _Random; }
        }
    }
}
