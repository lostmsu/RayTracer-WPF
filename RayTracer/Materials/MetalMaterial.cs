using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class MetalMaterial : Material
    {
        public Double3 Specular { get; private set; }
        public int Exponent { get; private set; }

        private Double3 frenselValue;

        public MetalMaterial(Double3 specular, int exponent)            
        {
            Specular = specular;
            Exponent = exponent;

            frenselValue = (-Specular + 1.0f);
        }

        public override void Shade(out Double3 pixelColor, HitData data, Scene scene)
        {
            // get the point the ray hit the surface.
            Double3 hitPosition = data.HitPosition;

            Double3 normal = data.Primitive.GetNormal(hitPosition);

            double costheta = normal.Dot(data.Ray.Direction);
            if (costheta > 0)
                normal = -normal;
            else
                costheta = -costheta;

            // Accumulate the specular light from every light source.
            Double3 specularLight = new Double3();
            LightData lightData = new LightData();
                
            foreach (Light light in scene.Lights)
            {
                light.GetLightData(ref lightData, hitPosition);
                double dDotL = normal.Dot(lightData.Direction);
                if (dDotL > 0 && 
                    !scene.TraceShadowRay(                         
                        new HitData(new Ray(hitPosition, lightData.Direction),
                            lightData.Distance, data.Attenuation)))
                {
                    // add specular light
                    Double3 r = lightData.Direction - data.Ray.Direction;
                    r.Normalize();
                    double cosalpha = normal.Dot(r);
                    if (dDotL > Constants.EPSILON)
                        specularLight.Add(lightData.Color * Math.Pow(cosalpha, Exponent));
                }
            }

            pixelColor = specularLight * Specular;

            // Add in reflection
            if (data.Depth < scene.MaxDepth)
            {
                Double3 reflection = scene.Background.GetColor();
                Double3 fresnel = Specular + (frenselValue * Math.Pow(1.0 - costheta, 5.0));
                Double3 reflectDir = data.Ray.Direction + normal * (2.0 * costheta);
                Ray reflectRay = new Ray(hitPosition, reflectDir);
                HitData reflectData = new HitData(reflectRay, double.PositiveInfinity, data.Attenuation - pixelColor);
                if (scene.Group.Intersect(reflectData) < Single.PositiveInfinity)
                    reflectData.Primitive.Material.Shade(out reflection, reflectData, scene);
                pixelColor.Add(reflection * fresnel);
            }
        }
    }
}
