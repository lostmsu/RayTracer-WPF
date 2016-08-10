using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public abstract class Camera
    {
        public Scene Scene { get; private set; }

        public Double3 Eye { get; set; }
        public Double3 Lookat { get; set; }
        public Double3 Up { get; set; }

        public bool IsDirty { get; protected set; }

        public Camera(Double3 eye, Double3 lookat, Double3 up)
        {
            Eye = eye;
            Lookat = lookat;
            Up = up;
        }

        public virtual void Preprocess(Scene scene)
        {
            Scene = scene;
        }

        public abstract Ray MakeRay(int x, int y);
    }
}
