using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class OrthographicCamera : Camera
    {
        public double Scale { get; private set; }
        
        private Double3 lookDir;
        private Double3 U, V;

        public OrthographicCamera(Double3 eye, Double3 lookat, Double3 up, double scale)
            : base(eye, lookat, up)
        {
            Scale = scale;
        }

        public override void Preprocess(Scene scene)
        {
            base.Preprocess(scene);

            double aspectRatio = (double)Scene.Image.Width / Scene.Image.Height;

            lookDir = Lookat - Eye;
            lookDir.Normalize();

            U = lookDir.Cross(Up);
            V = U.Cross(lookDir);

            U = U / U.Length() * Scale;
            V = V / V.Length() * Scale / aspectRatio;
        }

        public override Ray MakeRay(int x, int y)
        {
            // map pixels to 0 to 1
            double tx = -1.0 + ((1.0 + 2.0 * x) / Scene.Image.Width);
            double ty = -1.0 + ((1.0 + 2.0 * y) / Scene.Image.Height);

            Double3 origin = Eye + U * tx + V * ty;
            Ray ray = new Ray(origin, lookDir);
            return ray;
        }
    }
}
