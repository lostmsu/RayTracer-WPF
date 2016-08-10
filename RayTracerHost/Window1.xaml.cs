using System.Windows;
using RayTracer;
using System.Windows.Controls;
using System;
using System.Windows.Media;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Data;
using RayTracer.SceneReaders;

namespace RayTracerHost
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        Scene scene;
        //RenderContext context;

        public Window1()
        {
            InitializeComponent();

            // create and render an entire scene.
            //scene = CreateScene();
            scene = CreateScene01();
            scene.Preprocess();

            SceneDisplay.Content = scene;

            //RenderScene();            
        }

        private static Scene CreateScene01()
        {
            // This is the scene to produce for program #1.

            SceneImage image = new KImage(512, 512);
            int maxDepth = 25;
            double minAttenuation = 0.01;
            
            Double3 eye = new Double3(-2.0, -20.0, 8.0);
            PinholeCamera camera = new PinholeCamera(
                new Double3(-2.0, -20.0, 8.0),
                new Double3(0.25, 0.0, 4.5),
                new Double3(0.0, -0.707107, 0.707107),
                26.0);

            Background background = new ConstantBackground(
                new Double3(0.8, 0.3, 0.9));
            
            List<Light> lights = new List<Light>(2);
            lights.Add(new PointLight(new Double3(0.7, 0.7, 0.7),
                new Double3(-40.0, -40.0, 100.0)));
            lights.Add(new PointLight(new Double3(0.3, 0.1, 0.1),
                new Double3(20.0, -40.0, 100.0)));

            LambertianMaterial disk1matl = new LambertianMaterial(
                new Double3(1.0, 0.5, 0.5));
            LambertianMaterial disk2matl = new LambertianMaterial(
                new Double3(0.5, 1.0, 0.5));
            LambertianMaterial ringmatl = new LambertianMaterial(
                new Double3(0.5, 0.5, 1.0));
            LambertianMaterial planematl = new LambertianMaterial(
                new Double3(0.4, 0.4, 0.7));
            LambertianMaterial box1matl = new LambertianMaterial(
                new Double3(1.0, 1.0, 1.0));
            MetalMaterial sphere1matl = new MetalMaterial(
                new Double3(0.3, 1.0, 0.7), 100);
            PhongMaterial sphere2matl = new PhongMaterial(
                new Double3(1.0, 0.9, 0.4), new Double3(0.5, 1.0, 1.0), 100, 0.0);
            LambertianMaterial trianglematl = new LambertianMaterial(
                new Double3(1.0, 1.0, 0.0));
            LambertianMaterial box2matl = new LambertianMaterial(
                new Double3(0.3, 0.2, 0.3));
            PhongMaterial sphere3matl = new PhongMaterial(
                new Double3(0.4, 0.9, 1.0), new Double3(0.5, 0.5, 0.5), 15, 0.0);
            
            Group group = new ObjectGroup();
            group.AddObject(new Plane(planematl, Double3.Zero, Double3.ZAxis));
            group.AddObject(new Box(box1matl, new Double3(-4.5, -4.5, 0.0), new Double3(4.5, 4.5, 2.0)));
            group.AddObject(new Disk(disk1matl, new Double3(-1.0, -1.0, 1.0), new Double3(3.0, -3.3, 2.5), 0.5));
            group.AddObject(new Disk(disk2matl, new Double3(-1.0, -1.0, 1.0), new Double3(2.5, -3.3, 2.5), 0.5));
            group.AddObject(new Disk(disk1matl, new Double3(-1.0, -1.0, 1.0), new Double3(2.0, -3.3, 2.5), 0.5));
            group.AddObject(new Disk(disk2matl, new Double3(-1.0, -1.0, 1.0), new Double3(1.5, -3.3, 2.5), 0.5));
            group.AddObject(new Disk(disk1matl, new Double3(-1.0, -1.0, 1.0), new Double3(1.0, -3.3, 2.5), 0.5));
            group.AddObject(new Sphere(sphere1matl, 2.4, new Double3(1.5, 3.5, 4.0)));
            group.AddObject(new Sphere(sphere2matl, 0.5, new Double3(-2.0, 2.0, 5.0)));
            group.AddObject(new Ring(ringmatl, new Double3(-1.0, -1.0, 1.0), new Double3(-2.0, 2.0, 5.0), 1.2, 1.8));
            group.AddObject(new Ring(ringmatl, new Double3(-1.0, -1.0, 1.0), new Double3(-2.0, 2.0, 5.0), 2.2, 2.8));
            group.AddObject(new Triangle(trianglematl, new Double3(1.0, 1.0, 4.0), new Double3(3.0, -0.5, 2.0), new Double3(3.0, 1.5, 6.0)));
            group.AddObject(new Box(box2matl, new Double3(-3.5, -3.5, 2.0), new Double3(0.0, 0.0, 2.35)));
            group.AddObject(new Sphere(sphere3matl, 0.8, new Double3(-1.75, -1.75, 3.15)));

            Scene scene = new Scene(image, camera, group, lights, background, maxDepth, minAttenuation, 1);
            scene.Ambient = new Double3(0.3);

            return scene;
        }

        private static Scene CreateScene02()
        {
            // This is the scene to produce for program #1.

            SceneImage image = new KImage(512, 512);
            int maxDepth = 25;
            double minAttenuation = 0.01;

            PinholeCamera camera = new PinholeCamera(
                new Double3(0.0, 3.0, 10.0),
                new Double3(0.0, 1.0, 0.0),
                new Double3(0.0, 1.0, 0.0),
                45.0);

            Background background = new ConstantBackground(
                new Double3(0.7, 0.3, 0.8));

            List<Light> lights = new List<Light>(2);
            lights.Add(new PointLight(new Double3(0.6, 0.6, 0.6),
                new Double3(-40.0, 80.0, 40.0)));
            lights.Add(new PointLight(new Double3(0.6, 0.6, 0.6),
                new Double3(20.0, 80.0, -20.0)));

            LambertianMaterial boxmatl = new LambertianMaterial(
                new Double3(1.0, 0.5, 0.5));
            LambertianMaterial boxmat2 = new LambertianMaterial(
                new Double3(0.5, 1.0, 0.5));
            LambertianMaterial planematl = new LambertianMaterial(
                new Double3(0.4, 0.4, 0.7));
            LambertianMaterial trianglematl = new LambertianMaterial(
                new Double3(1.0, 1.0, 0.0));

            Group group = new ObjectGroup();
            group.AddObject(new Plane(planematl, Double3.Zero, Double3.YAxis));

            int numSquares = 4;
            for (int i = 0; i < numSquares; i++)
            {
                Material mat = i % 2 == 0 ? boxmatl : boxmat2;
                group.AddObject(new Box(mat, new Double3(i - numSquares / 2.0, 0.0, 0.0), new Double3(i + 1 - numSquares / 2.0, 1, 1.0)));
            }
            
            Scene scene = new Scene(image, camera, group, lights, background, maxDepth, minAttenuation, 1);
            scene.Ambient = new Double3(0.3);

            return scene;
        }

        private void RenderScene()
        {
            //// Multiple threads
            //RenderContext context1 = new RenderContext(scene, 0, 0, scene.Image.Width / 2, scene.Image.Height);
            //RenderContext context2 = new RenderContext(scene, scene.Image.Width / 2, 0, scene.Image.Width, scene.Image.Height);
            //context1.Preprocess();
            //context2.Preprocess();

            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();

            ////context1.RenderScene();

            //context1.Preprocess();
            //context2.Preprocess();

            //Thread thread1 = new Thread(new ThreadStart(context1.RenderScene));
            //Thread thread2 = new Thread(new ThreadStart(context2.RenderScene));

            //// Start the thread
            //thread1.Start();
            //thread2.Start();
            //thread1.Join();
            //thread2.Join();

            //stopwatch.Stop();
            //RenderTime.Text = (stopwatch.ElapsedMilliseconds / 1000.0).ToString();

            //context1.Scene.Image.UpdateImage(new Int32Rect(0, 0, context1.Scene.Image.Width, context1.Scene.Image.Height));

            // No threading
            RenderContext context1 = new RenderContext(scene, 0, 0, scene.Image.Width, scene.Image.Height);
            context1.Preprocess();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            context1.RenderScene();

            stopwatch.Stop();
            RenderTime.Text = (stopwatch.ElapsedMilliseconds / 1000.0).ToString();

            MainImage.Source = context1.Scene.Image.UpdateImage(
                new Int32Rect(0, 0, context1.Scene.Image.Width, context1.Scene.Image.Height));
        }

        private Scene CreateScene()
        {
            // make an object for the scene.
            Material sphereMaterial = new PhongMaterial(new Double3(0.5, 0.3, 0.2), new Double3(1.0, 1.0, 1.0), 200, 0.5);
            Sphere sphere1 = new Sphere(sphereMaterial, 1.5, new Double3(-2.0, 0.0, 0.0));
            Sphere sphere2 = new Sphere(sphereMaterial, 1.5, new Double3(2.0, 0.0, 0.0));

            Material boxMaterial = new LambertianMaterial(new Double3(0.5, 0.3, 0.2));
            Box box = new Box(boxMaterial, new Double3(-2.0, 2.0, -2.0), new Double3(0.0, 3.0, 0.0));

            Material planeMaterial = new LambertianMaterial(new Double3(0.5, 0.3, 0.2));
            Plane plane = new Plane(boxMaterial, new Double3(0.0, -2.3, 0.0), new Double3(0.0, 1.0, 0.0));

            Material triangleMaterial = new LambertianMaterial(new Double3(0.5, 0.3, 0.2));
            Triangle triangle = new Triangle(triangleMaterial,
                new Double3(2.0, -1.0, 0.0),
                new Double3(-2.0, -2.0, 3.0), 
                new Double3(1.0, 1.0, -1.0));


            // give the scene a camera
            Camera camera = new PinholeCamera(
                new Double3(1.0, 2.3, 11.0),  // eye 
                new Double3(0.0, 0.0, 0.0),   // lookat 
                new Double3(0.3, 1.0, 0.8),   // up
                45.0);                         // field of view

            Background background = new ConstantBackground(new Double3(0.3, 0.4, 0.5));

            BVHGroup bvhGroup = new BVHGroup();
            bvhGroup.AddObject(sphere1);
            bvhGroup.AddObject(sphere2);
            bvhGroup.AddObject(box);
            bvhGroup.AddObject(triangle);

            ObjectGroup mainGroup = new ObjectGroup(2);
            mainGroup.AddObject(plane);
            //mainGroup.AddObject(sphere1);
            //mainGroup.AddObject(sphere2);
            //mainGroup.AddObject(box);
            //mainGroup.AddObject(triangle);

            mainGroup.AddObject(bvhGroup);

            List<Light> lights = new List<Light>();
            lights.Add(new PointLight(new Double3(0.5, 0.5, 0.5), new Double3(-10.0, 20.0, 10.0)));
            lights.Add(new PointLight(new Double3(0.5, 0.5, 0.5), new Double3(10.0, 20.0, 10.0)));
            lights.Add(new PointLight(new Double3(0.5, 0.5, 0.5), new Double3(20.0, 20.0, 0.0)));
            lights.Add(new PointLight(new Double3(0.5, 0.5, 0.5), new Double3(-20.0, 20.0, 0.0)));
            lights.Add(new DirectionalLight(new Double3(0.5, 0.5, 0.5), new Double3(0.0, -1.0, 0.0)));
            
            var image = new KImage(512, 512);
            
            Scene newScene = new Scene(image, camera, mainGroup, lights, background, 5, 0.1, 1);

            return newScene;
        }

        private void MainImage_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point hitPosition = e.GetPosition(MainImage);
                double real_y = hitPosition.Y * scene.Image.Height / MainImage.ActualHeight;
                double real_x = hitPosition.X * scene.Image.Width / MainImage.ActualWidth; 

                scene.TraceRay((int)real_x, (int)real_y);

                //UpdateSceneImage();
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            RenderScene();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            
            dlg.FileName = "Scene"; // Default file name
            dlg.DefaultExt = ".scn"; // Default file extension
            dlg.Filter = "Project documents (.scn)|*.scn"; // Filter files by extension

            // Show save file dialog box

            Nullable<bool> result = dlg.ShowDialog();
            // Process save file dialog box results

            if (result == true)
            {
                SCNReader reader = new SCNReader(dlg.FileName);
                scene = reader.ReadSceneFile();
            }
        }

        bool flag = true;
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            flag = !flag;

            if (flag)
            {
                RenderOptions.SetBitmapScalingMode(MainImage, BitmapScalingMode.NearestNeighbor);
                //RenderOptions.SetEdgeMode(MainImage, EdgeMode.Aliased);
            }
            else
            {
                RenderOptions.SetBitmapScalingMode(MainImage, BitmapScalingMode.Fant);
                //RenderOptions.SetEdgeMode(MainImage, EdgeMode.Aliased);
            }

        }
    }
}
