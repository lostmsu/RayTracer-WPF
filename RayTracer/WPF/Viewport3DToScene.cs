using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using wpf3D = System.Windows.Media.Media3D;
using ray3D = RayTracer;

namespace RayTracer.WPF
{
    public class Viewport3DToScene
    {
        Viewport3D ViewPort3D;
        List<ray3D.Light> Lights;
        ray3D.Group Group;
        Stack<wpf3D.Matrix3D> TransformStack = new Stack<wpf3D.Matrix3D>();

        Scene Scene;

        public int NumberOfTriangles { get; private set; }

        public Viewport3DToScene(Viewport3D viewport3D)
        {            
            ViewPort3D = viewport3D;
        }

        public Scene CreateScene()
        {
            ray3D.SceneImage image = CreateImage(1);
            ray3D.Camera camera = CreateCamera();
            ray3D.Background background = new ray3D.ConstantBackground(new Double3(1.0, 1.0, 1.0));
            
            Lights = new List<ray3D.Light>();
            Group = new ray3D.BVHGroup();
        
            Scene = new Scene(image, camera, Group, null, background, 5, 0.01, 1);
            
            foreach (wpf3D.Visual3D visual3D in ViewPort3D.Children)
            {
                AddVisual3D(visual3D);
            }
           
            Scene.Lights = Lights.ToArray();

            return Scene;
        }

        private void AddVisual3D(wpf3D.Visual3D visual3D)
        {
            if (visual3D is wpf3D.ModelVisual3D)
            {
                AddModelVisual3D((wpf3D.ModelVisual3D)visual3D);
            }
            else if (visual3D is wpf3D.ContainerUIElement3D)
            {
                PushTransform(visual3D.Transform);

                var container = (wpf3D.ContainerUIElement3D)visual3D;
                foreach (wpf3D.Visual3D vis in container.Children)
                    AddVisual3D(vis);

                PopTransform(visual3D.Transform);
            }
            else if (visual3D is wpf3D.ModelUIElement3D)
            {
                PushTransform(visual3D.Transform);

                var element = (wpf3D.ModelUIElement3D)visual3D;
                AddModel3D(element.Model);

                PopTransform(visual3D.Transform);
            }
            //else if (visual3D is IShowModelUIElement3D) // the UIElement3D must be an IShowModelUIElment3D!!
            //{
            //    PushTransform(visual3D.Transform);

            //    var element = (IShowModelUIElement3D)visual3D;
            //    AddModel3D(element.GetVisual3DModel());

            //    PopTransform(visual3D.Transform);
            //}
            else
            {
                throw new NotImplementedException();
            }
        }

        private void AddModelVisual3D(wpf3D.ModelVisual3D visual3D)
        {
            PushTransform(visual3D.Transform);

            AddModel3D(visual3D.Content);

            if (visual3D.Children.Count > 0)
                throw new NotImplementedException();

            PopTransform(visual3D.Transform);
        }

        private void AddModel3D(wpf3D.Model3D model3D)
        {
            PushTransform(model3D.Transform);

            if (model3D is wpf3D.GeometryModel3D)
            {
                AddGeometryModel3D((wpf3D.GeometryModel3D)model3D);
            }
            else if (model3D is wpf3D.Light)
            {
                AddMedia3DLight((wpf3D.Light)model3D);
            }
            else if (model3D is wpf3D.Model3DGroup)
            {
                foreach (wpf3D.Model3D m in ((wpf3D.Model3DGroup)model3D).Children)
                {
                    AddModel3D(m);
                }
            }

            PopTransform(model3D.Transform);
        }

        private void PopTransform(wpf3D.Transform3D transform3D)
        {
            if (transform3D != null)
                TransformStack.Pop();
        }

        private void PushTransform(wpf3D.Transform3D transform3D)
        {
            if (transform3D != null)
                TransformStack.Push(transform3D.Value);
        }

        private void AddMedia3DLight(wpf3D.Light media3DLight)
        {
            if (media3DLight is wpf3D.DirectionalLight)
            {
                var dirLight = (wpf3D.DirectionalLight)media3DLight;
                Lights.Add(
                    new RayTracer.DirectionalLight(
                        new Double3(dirLight.Color),
                        new Double3(dirLight.Direction)));
            }
            else if (media3DLight is wpf3D.PointLight)
            {
                var pntLight = (wpf3D.PointLight)media3DLight;
                Lights.Add(
                    new ray3D.PointLight(
                        new Double3(pntLight.Color),
                        new Double3(pntLight.Position)));
            }
            else if (media3DLight is wpf3D.AmbientLight)
            {
                var ambLight = (wpf3D.AmbientLight)media3DLight;
                Scene.Ambient += new Double3(ambLight.Color);
            }
            else if (media3DLight is wpf3D.SpotLight)
            {
                // TODO, Create the correct Spot light with a direction
                var sptLight = (wpf3D.SpotLight)media3DLight;
                Lights.Add(
                    new ray3D.PointLight(
                        new Double3(sptLight.Color),
                        new Double3(sptLight.Position)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void AddGeometryModel3D(wpf3D.GeometryModel3D geometryModel3D)
        {
            ray3D.Material material = WPFMaterialToRayMaterial(geometryModel3D.Material);

            if (geometryModel3D.Geometry is wpf3D.MeshGeometry3D && material != null)
            {
                var meshGeometry3D = (wpf3D.MeshGeometry3D)geometryModel3D.Geometry;

                wpf3D.Point3D[] positions = meshGeometry3D.Positions.ToArray<wpf3D.Point3D>();
                wpf3D.Vector3D[] normals = meshGeometry3D.Normals.ToArray<wpf3D.Vector3D>();
                Point[] coordinates = meshGeometry3D.TextureCoordinates.ToArray<Point>();
                Int32[] indices = meshGeometry3D.TriangleIndices.ToArray<Int32>();

                wpf3D.Matrix3D transform = new wpf3D.Matrix3D();
                
                foreach (wpf3D.Matrix3D m in TransformStack)
                    transform.Append(m);

                // I don't know why I had to do this, but it was just to make the WPF trackball work correctly.
                var cameraMatrix = ViewPort3D.Camera.Transform.Value;
                cameraMatrix.Invert();
                transform.Append(cameraMatrix);

                transform.Transform(positions);
                transform.Transform(normals);

                // Adding the mesh to an acceleration group has the ability to speed up performance dramatically
                ray3D.BVHGroup bvh = new ray3D.BVHGroup();

                for (int i = 0; i < indices.Length; i += 3)
                {
                    ray3D.Triangle triangle;
                    if (i + 2 < coordinates.Length)
                        triangle = new ray3D.Triangle(material,
                            new Double3(positions[indices[i]]),
                            new Double3(positions[indices[i + 1]]),
                            new Double3(positions[indices[i + 2]]),
                            coordinates[indices[i]],
                            coordinates[indices[i + 1]],
                            coordinates[indices[i + 2]]);
                    else
                        triangle = new ray3D.Triangle(material,
                            new Double3(positions[indices[i]]),
                            new Double3(positions[indices[i + 1]]),
                            new Double3(positions[indices[i + 2]]));
                    bvh.AddObject(triangle);
                    NumberOfTriangles++;
                }

                Group.AddObject(bvh);

                // uncomment to render the bounds of the bvh
                //Group.AddObject(new Box(material, bvh.Bounds[0], bvh.Bounds[1]));

            }
            else
            {
                //throw new NotImplementedException();
            }
        }

        private ray3D.Box MakeRect3DIntoBox(ray3D.Material material, wpf3D.Rect3D rect3D)
        {
            return new ray3D.Box(
                material,
                new ray3D.Double3(rect3D.X, rect3D.Y, rect3D.Z),
                new ray3D.Double3(rect3D.X + rect3D.SizeX, rect3D.Y + rect3D.SizeY, rect3D.Z + rect3D.SizeZ));
        }

        private ray3D.Material WPFMaterialToRayMaterial(wpf3D.Material material)
        {
            WPFMaterial wpfMaterial = new WPFMaterial();
            AddMaterialToRayMaterial(ref wpfMaterial, material);
            return wpfMaterial;
        }

        private void AddMaterialToRayMaterial(ref WPFMaterial wpfMaterial, wpf3D.Material material)
        {
            if (wpfMaterial == null)
                return;
            else if (material is wpf3D.MaterialGroup)
            {
                foreach (wpf3D.Material mat in ((wpf3D.MaterialGroup)material).Children)
                    AddMaterialToRayMaterial(ref wpfMaterial, mat);
            }
            else if (material is wpf3D.DiffuseMaterial)
            {
                wpf3D.DiffuseMaterial difMat = (wpf3D.DiffuseMaterial)material;
                if (difMat.Brush is SolidColorBrush)
                    wpfMaterial.DiffuseColor = new Double3(((SolidColorBrush)difMat.Brush).Color);
                else if (difMat.Brush is ImageBrush)
                {
                    ImageBrush imageBrush = (ImageBrush)difMat.Brush;
                    wpfMaterial.Texture = new WriteableBitmapImage(imageBrush.ImageSource);
                }
                else
                {
                    wpfMaterial = null;
                    return;
                }
            }
            else if (material is wpf3D.SpecularMaterial)
            {
                wpf3D.SpecularMaterial spcMat = (wpf3D.SpecularMaterial)material;
                wpfMaterial.SpecularExponent = spcMat.SpecularPower;
                if (spcMat.Brush is SolidColorBrush)
                    wpfMaterial.SpecularColor = new Double3(((SolidColorBrush)spcMat.Brush).Color);
                else
                {
                    wpfMaterial = null;
                    return;
                }
            }
            else if (material is wpf3D.EmissiveMaterial)
            {
                wpf3D.EmissiveMaterial emsMat = (wpf3D.EmissiveMaterial)material;
                if (emsMat.Brush is SolidColorBrush)
                    wpfMaterial.EmissiveColor = new Double3(((SolidColorBrush)emsMat.Brush).Color);
                else
                {
                    wpfMaterial = null;
                    return;
                }
            }
            else
            {
                wpfMaterial = null;
                return;
            }
        }

        private ray3D.Camera CreateCamera()
        {
            if (ViewPort3D.Camera is wpf3D.PerspectiveCamera)
            {                
                var persCamera = ViewPort3D.Camera as wpf3D.PerspectiveCamera;                                
                return new ray3D.PinholeCamera(
                    new Double3(persCamera.Position),
                    new Double3(persCamera.Position + persCamera.LookDirection),
                    new Double3(persCamera.UpDirection),
                    (double)persCamera.FieldOfView);
            }
            else if (ViewPort3D.Camera is wpf3D.OrthographicCamera)
            {
                throw new NotImplementedException();
            }
            else
                throw new NotImplementedException();
        }

        private ray3D.SceneImage CreateImage(int scale)
        {
            return new ray3D.KImage((int)ViewPort3D.ActualWidth * scale, (int)ViewPort3D.ActualHeight * scale);
        }
    }
}
