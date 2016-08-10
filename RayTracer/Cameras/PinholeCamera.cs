using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class PinholeCamera : Camera
    {
        public double Fov { get; set; }

        private Double3 lookDir;
        private Double3 U, V;
        
        public PinholeCamera(Double3 eye, Double3 lookat, Double3 up, double fov)
            : base(eye, lookat, up)
        {
            Fov = fov;
        }

        public override void Preprocess(Scene scene)
        {
            base.Preprocess(scene);

            double aspectRatio = (double)Scene.Image.Width / Scene.Image.Height;

            lookDir = Lookat - Eye;
            lookDir.Normalize();

            U = lookDir.Cross(Up);
            V = U.Cross(lookDir);
            double ulen = (double)Math.Tan(Fov * Math.PI / 360.0);
            U = U / U.Length() * ulen;
            V = V / V.Length() * ulen / aspectRatio;
        }

        public override Ray MakeRay(int x, int y)
        {
            double jitter_x = x;
            double jitter_y = y;

            if (Scene.Samples > 1)
            {
                jitter_x += (Constants.Random.NextDouble() - 0.5);
                jitter_y += (Constants.Random.NextDouble() - 0.5);
            }

            // map pixels to 0 to 1
            double tx = -1.0 + ((1.0 + 2.0 * jitter_x) / Scene.Image.Width);
            double ty = -1.0 + ((1.0 + 2.0 * jitter_y) / Scene.Image.Height);

            Double3 direction = lookDir + U * tx + V * ty;
            direction.Normalize();
            Ray ray = new Ray(Eye, direction);
            return ray;
        }
    }
}
