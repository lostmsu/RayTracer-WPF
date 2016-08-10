using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class ConstantBackground : Background
    {
        public Double3 Color { get; set; }

        public ConstantBackground(Double3 color)
        {
            Color = color;
        }

        public override Double3 GetColor()
        {
            return Color;
        }
    }
}
