using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public abstract class Group : Object
    {
        public Group(string name)
            : base(name)
        {

        }

        public abstract void AddObject(Object obj);
    }
}
