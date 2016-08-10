using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class LightData
    {
        public Double3 Color { get; set; }
        public Double3 Direction { get; set; }
        public double Distance { get; set; }

        public LightData(Double3 color, Double3 direction, double distance)
        {
            Color = color;
            Direction = direction;
            Distance = distance;
        }

        public LightData()
        {
        //    Color = Double3.Zero;
        //    Direction = Double3.Zero;
        //    Distance = 0.0;
        }
    }

    public abstract class Light
    {
        public string Name { get; private set; }
     
        public Light(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Sets the data for a light given a position in the world.
        /// </summary>
        /// <param name="data">The data that will be set when called from materials</param>
        /// <param name="hitPosition">The position of the surface that the light is shinning on</param>
        public abstract void GetLightData(ref LightData data, Double3 hitPosition);
    }
}
