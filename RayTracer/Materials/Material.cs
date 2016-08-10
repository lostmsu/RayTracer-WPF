using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public abstract class Material
    {
        public SceneImage Texture { get; set; }
        public abstract void Shade(out Double3 pixelColor, HitData data, Scene scene);

        /// <summary>
        /// Get the texture color of the surface point on the primitive.
        /// </summary>
        /// <param name="hitPosition">The ray primitive intersection point</param>
        /// <param name="primitive">The Primitive that the ray hit</param>
        /// <returns>The color of the texture surface</returns>
        protected Double3 GetTextureColor(double u, double v)
        {
            if (Texture != null)
                return Texture.GetPixel(u, v);
            else
                return new Double3();
        }
    }
}
