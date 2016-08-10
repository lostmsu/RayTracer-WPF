using System.Windows;
using RayTracer;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using System;
using RayTracer.WPF;
using System.Windows.Input;
using System.Diagnostics;

namespace Viewport3DRayTracer
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        Scene Scene;
        RenderContext context;

        private void RayTraceScene_ButtonClick(object sender, RoutedEventArgs e)
        {            
            Viewport3DToScene sceneCreator = new Viewport3DToScene(ViewPort);
            Scene = sceneCreator.CreateScene();
            
            NumTrianglesText.Text = sceneCreator.NumberOfTriangles.ToString();

            context = new RenderContext(Scene, 0, 0, Scene.Image.Width, Scene.Image.Height);
            
            context.RenderScene();
            
            MainImage.Source = Scene.Image.UpdateImage(new Int32Rect(0, 0, Scene.Image.Width, Scene.Image.Height));

            RayTracerTime.Text = context.RenderTime;
        }

        private void MainImage_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && Scene != null)
            {
                Point hitPosition = e.GetPosition(MainImage);
                double real_y = hitPosition.Y * Scene.Image.Height / MainImage.ActualHeight;
                double real_x = hitPosition.X * Scene.Image.Width / MainImage.ActualWidth;

                Scene.TraceRay((int)real_x, (int)real_y);

                //Scene.Image.UpdateImage(new Int32Rect((int)real_x, (int)real_y, 1, 1));
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
