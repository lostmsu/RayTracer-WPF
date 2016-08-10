using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

using wpf3D = System.Windows.Media.Media3D;
using System.Windows;
using System.Windows.Controls;

namespace RayTracer.WPF
{
    public class WPFMaterial : Material 
    {
        public Double3 DiffuseColor { get; set; }        
        public Double3 SpecularColor { get; set; }
        public Double3 EmissiveColor { get; set; }
        public double SpecularExponent { get; set; }
        
        
        //public double ReflectiveValue { get; set; }

        //public static readonly DependencyProperty ReflectiveProperty = DependencyProperty.RegisterAttached(
        //    "Reflective",
        //    typeof(Double),
        //    typeof(WPFMaterial));

        //public static Double GetReflective(DependencyObject obj)
        //{
        //    return (Double)obj.GetValue(ReflectiveProperty);
        //}

        //public static void SetReflective(DependencyObject obj, Double value)
        //{
        //    obj.SetValue(ReflectiveProperty, value);
        //}

        public WPFMaterial()
        {
            DiffuseColor = new Double3();
            SpecularColor = new Double3();
            EmissiveColor = new Double3();
            SpecularExponent = double.NaN;
            
            //ReflectiveValue = 0.0;
        }

        public override void Shade(out Double3 pixelColor, HitData data, Scene scene)
        {
            if (data.Attenuation.MaxValue() < scene.MinAttenuation)
            {
                pixelColor = new Double3();
                return;
            }

            // Get normal and hit position in 3D space where the intersection happened.
            Double3 hitPosition, normal;
            data.GetHitData(out hitPosition, out normal);

            double costheta = normal.Dot(data.Ray.Direction);
            if (costheta > 0.0)
            {
                normal = -normal;
            }
            else
                costheta = -costheta;

            if (Texture != null)
            {
                DiffuseColor = GetTextureColor(data.u, data.v);
            }

            Double3 allLightsDiffuseColor = new Double3();
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
                    allLightsDiffuseColor += lightData.Color * dDotL;

                    // add specular light using the blinn phong half-vector
                    if (SpecularExponent >= 0.0)
                    {
                        Double3 halfvec = (lightData.Direction - data.Ray.Direction);
                        halfvec.Normalize();
                        double NdotH = normal.Dot(halfvec);
                        if (NdotH > 0.0)
                        {
                            double powterm = (double)Math.Pow(NdotH, SpecularExponent);
                            allLightsSpecular += lightData.Color * powterm;
                        }
                    }
                }
            }

            // add diffuse and ambient
            pixelColor = DiffuseColor * (allLightsDiffuseColor + scene.Ambient);

            // add reflection
            if (SpecularExponent > 10.0 && data.Depth < scene.MaxDepth)
            {
                double reflectiveValue = SpecularExponent / 1000;
                reflectiveValue = reflectiveValue > 1.0 ? 1.0 : reflectiveValue;

                Double3 reflection = scene.Background.GetColor();
                Double3 reflectDir = data.Ray.Direction + normal * (double)(2.0 * costheta);
                Ray reflectRay = new Ray(hitPosition, reflectDir);
                HitData reflectData = new HitData(reflectRay, 
                    Single.PositiveInfinity, data.Attenuation * reflectiveValue, data.Depth + 1);
                if (scene.Group.Intersect(reflectData) < Single.PositiveInfinity)
                {
                    reflectData.Primitive.Material.Shade(out reflection, reflectData, scene);
                    //Double3 fresnel = SpecularColor + (-SpecularColor + 1.0f) * Math.Pow(1.0 - costheta, 5.0);
                    //reflection *= fresnel;
                }
                pixelColor = (pixelColor * (1.0 - reflectiveValue)) + (reflection * reflectiveValue);
            }

            // add specular
            pixelColor += SpecularColor * allLightsSpecular;
        }
    }
}