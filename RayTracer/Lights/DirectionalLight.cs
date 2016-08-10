using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class DirectionalLight : Light
    {
        public Double3 Color { get; private set; }

        private Double3 _Direction;
        public Double3 Direction 
        {
            get { return _Direction; }
            set
            {
                if (_Direction == null || !_Direction.Equals(value))
                {
                    _Direction = value;
                    _Direction.Normalize();
                }
            }
        }

        public DirectionalLight(Double3 color, Double3 direction)
            : base("DirectionalLight")
        {
            Color = color;
            Direction = direction;
        }

        public override void GetLightData(ref LightData data, Double3 hitPosition)
        {
            data.Color = Color;
            data.Direction = -Direction;
            data.Distance = double.PositiveInfinity;
        }
    }
}
