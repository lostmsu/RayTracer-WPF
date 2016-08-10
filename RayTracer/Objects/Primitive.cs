using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RayTracer.WPF;

namespace RayTracer
{
    public abstract class Primitive : Object
    {
        public Material Material { get; private set; }
        
        public Primitive(Material material, string name)
            : base(name)
        {
            Material = material;
        }

        /// <summary>
        /// returns a normalized normal value for the primitive at this position
        /// </summary>
        public abstract Double3 GetNormal(Double3 hitPosition);
    }
}
