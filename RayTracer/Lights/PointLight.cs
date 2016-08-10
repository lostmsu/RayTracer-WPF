using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class PointLight : Light
    {
        public Double3 Position { get; set; }
        public Double3 Color { get; private set; }

        public PointLight(Double3 color, Double3 position)
            : base("PointLight")
        {
            Color = color;
            Position = position;
        }

        public override void GetLightData(ref LightData data, Double3 hitPosition)
        {
            data.Color = Color;

            // A vector pointing to the light
            data.Direction = Position - hitPosition;
            data.Distance = data.Direction.Length();
            data.Direction.Normalize(data.Distance);
        }
    }
}
