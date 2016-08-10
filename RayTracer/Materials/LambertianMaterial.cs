using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class LambertianMaterial : Material
    {
        public Double3 Diffuse { get; private set; }

        public LambertianMaterial(Double3 diffuse)
        {
            Diffuse = diffuse;
        }

        public override void Shade(out Double3 pixelColor, HitData data, Scene scene)
        {
            if (data.Attenuation.MaxValue() < scene.MinAttenuation)
            {
                pixelColor = Double3.Zero;
                return;
            }

            Double3 hitPosition = data.HitPosition;
            Double3 normal = data.Primitive.GetNormal(hitPosition);
            
            if (normal.Dot(data.Ray.Direction) > 0)
                normal = -normal;

            Double3 allLightsColor = new Double3();           
            LightData lightData = new LightData();

            foreach (Light light in scene.Lights)
            {
                light.GetLightData(ref lightData, hitPosition);
       
                // calculate diffuse
                double dDotL = normal.Dot(lightData.Direction);
                if (dDotL > 0 && 
                        !scene.TraceShadowRay(                             
                            new HitData(new Ray(hitPosition, lightData.Direction), 
                                lightData.Distance, data.Attenuation)))
                    allLightsColor.Add(lightData.Color * dDotL);
            }

            allLightsColor.Add(scene.Ambient);
            allLightsColor.Clamp(0.0, 1.0);
            pixelColor = allLightsColor * Diffuse;
        }
    }
}
