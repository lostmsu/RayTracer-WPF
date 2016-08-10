using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using RayTracer;

namespace RayTracer.SceneReaders
{
    public class SCNReader : SceneReader
    {
        string Filename;

        public SCNReader(string filename)
        {
            Filename = filename;

            if (!filename.EndsWith(".scn"))
            {
                throw new InvalidDataException("Invalid Scene File: File must be a .scn file");
            }
        }

        public override Scene ReadSceneFile()
        {
            StreamReader reader = File.OpenText(Filename);

            SceneImage image = null;
            int xres = -1;
            int yres = -1;

            Camera camera = null;
            Group group = null;
            List<Light> lights = null;
            Background background = null;
            int maxDepth = 0;
            double minAttenuation = 0;
            double ambient = 0;

            string wholeString = "";
            string nextLine = reader.ReadLine().Trim();
            while (nextLine != null)
            {
                nextLine.Trim();
                if (nextLine.StartsWith("//"))
                { /* skip comments */ }
                else
                    wholeString += nextLine;
                
                nextLine = reader.ReadLine();
            }

            reader.Close();

            wholeString.Trim();
            while (wholeString.Length > 0)
            {
                string token = ReadNextToken(wholeString);
            }

            if (xres < 0 || yres < 0)
                throw new Exception("Declare a positive xres, yres for the image.");
            else
                image = new KImage(xres, yres);

            Scene scene = new Scene(image, camera, group, lights, background, maxDepth, minAttenuation, 1);

            return scene;
        }

        private string ReadNextToken(string wholeString)
        {
            return "";
        }

        /*
        public override Scene ReadSceneFile()
        {
            StreamReader reader = File.OpenText(Filename);

            SceneImage image = null;
            int xres = -1;
            int yres = -1;

            Camera camera = null;
            Group group = null;
            List<Light> lights = null;
            Background background = null;
            int maxDepth = 0; 
            double minAttenuation = 0;
            double ambient = 0;

            string token = reader.ReadLine();
            while (token  != null)
            {
                // skip comments
                token.Normalize();
                if (!token.StartsWith("//") && token.Length > 0)
                {
                    string s;
                    if (token.StartsWith(s = "xres", StringComparison.OrdinalIgnoreCase))
                    {
                        xres = int.Parse(token.Substring(s.Length));
                    }
                    else if (token.StartsWith(s = "yres", StringComparison.OrdinalIgnoreCase))
                    {
                        yres = int.Parse(token.Substring(s.Length));
                    }
                    else if (token.StartsWith(s = "filename", StringComparison.OrdinalIgnoreCase))
                    {
                        // skip for now
                    }
                    else if (token.StartsWith(s = "maxraydepth", StringComparison.OrdinalIgnoreCase))
                    {
                        maxDepth = int.Parse(token.Substring(s.Length));
                    }
                    else if (token.StartsWith(s = "minattenuation", StringComparison.OrdinalIgnoreCase))
                    {
                        minAttenuation = double.Parse(token.Substring(s.Length));
                    }
                    else if (token.StartsWith(s = "ambient", StringComparison.OrdinalIgnoreCase))
                    {
                        ambient = double.Parse(token.Substring(s.Length));
                    }
                    else if (token.StartsWith(s = "camera", StringComparison.OrdinalIgnoreCase))
                    {
                        token = token.Substring(s.Length).Trim();

                        if (token.StartsWith(s = "pinhole"))
                        {
                            camera = ReadPinholeCamera(reader);
                        }
                        else
                            throw new NotImplementedException();
                    }
                    else if (token.StartsWith(s = "background", StringComparison.OrdinalIgnoreCase))
                    {
                        token = token.Substring(s.Length).Trim();

                        if (token.StartsWith(s = "constant"))
                        {
                            token = reader.ReadLine().Replace("[", "").Replace("]", "").Trim();
                            if (token.StartsWith(s = "color"))
                            {
                                Float3 color = Float3.Parse(token.Substring(s.Length));
                                background = new ConstantBackground(color);
                            }
                        }
                        else
                            throw new NotImplementedException();
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                    
                    
                }

                token = reader.ReadLine();
            }

            reader.Close();

            if (xres < 0 || yres < 0)
                throw new Exception("Declare a positive xres, yres for the image.");
            else
                image = new KImage(xres, yres);

            Scene scene = new Scene(image, camera, group, lights, background, maxDepth, minAttenuation);

            return scene;
        }

        private Camera ReadPinholeCamera(StreamReader reader)
        {
            Float3 eye, lookat, up;
            double fov;

            eye = lookat = up = null;
            fov = -1.0f;

            string s, token;
            token = "";
            while (true)
            {
                token = reader.ReadLine().Trim();
                token = token.Replace("[", "").Replace("]", "");
                if (token.StartsWith(s = "eye"))
                {
                    eye = Float3.Parse(token.Substring(s.Length));
                }
                else if (token.StartsWith(s = "lookat"))
                {
                    lookat = Float3.Parse(token.Substring(s.Length));              
                }
                else if (token.StartsWith(s = "up"))
                {
                    up = Float3.Parse(token.Substring(s.Length));
                }
                else if (token.StartsWith(s = "hfov"))
                {
                    fov = double.Parse(token.Substring(s.Length));                
                }
                else if (token.Contains("}"))
                    break;                    
            }

            return new PinholeCamera(eye, lookat, up, fov);
        }
         */ 
    }
}

