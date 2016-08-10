
using System;
namespace RayTracer
{
    public class PhongMaterial : Material
    {
        public Double3 Diffuse { get; private set; }
        public Double3 Specular { get; private set; }
        public int Exponent { get; private set; }
        public double Reflectivity { get; private set; }

        public PhongMaterial(Double3 diffuse, Double3 specular, int exponent, double reflectivity)
        {
            Diffuse = diffuse;
            Specular = specular;
            Exponent = exponent;
            Reflectivity = reflectivity;
        }

        public override void Shade(out Double3 pixelColor, HitData data, Scene scene)
        {
            if (data.Attenuation.MaxValue() < scene.MinAttenuation)
            {
                pixelColor = new Double3();
                return;
            }

            Double3 hitPosition = data.HitPosition;

            Double3 normal = data.Primitive.GetNormal(hitPosition);
                        
            double costheta = normal.Dot(data.Ray.Direction);
            if (costheta > 0.0)
                normal = -normal;
            else
                costheta = -costheta;

            Double3 allLightsColor = new Double3();
            Double3 allLightsSpecular = new Double3();
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
                    // add diffuse light
                    allLightsColor.Add(lightData.Color * dDotL);

                    // add specular light using the blinn phong half-vector
                    Double3 halfvec = (lightData.Direction - data.Ray.Direction);
                    halfvec.Normalize();

                    double NdotH = normal.Dot(halfvec);
                    if (NdotH > 0.0)
                    {
                        double powterm = (double)Math.Pow(NdotH, Exponent);
                        allLightsSpecular.Add(lightData.Color * powterm);
                    }
                }
            }
            
            // add ambient diffuse and specular
            allLightsColor.Add(scene.Ambient);
            allLightsColor.Clamp(0.0, 1.0);
            pixelColor =
                allLightsColor * Diffuse * (1.0 - Reflectivity) +
                Specular * allLightsSpecular;

            // add reflection
            if (Reflectivity > 0.0 && data.Depth < scene.MaxDepth)
            {
                Double3 reflection = scene.Background.GetColor();
                Double3 reflectDir = data.Ray.Direction + normal * (double)(2.0 * costheta);
                Ray reflectRay = new Ray(hitPosition, reflectDir);
                HitData reflectData = new HitData(reflectRay, 
                    Single.PositiveInfinity, data.Attenuation * Reflectivity, data.Depth + 1);
                if (scene.Group.Intersect(reflectData) < Single.PositiveInfinity)
                    reflectData.Primitive.Material.Shade(out reflection, reflectData, scene);
                pixelColor.Add(reflection * Reflectivity);
            }
        }
    }
}
