# RayTracer-WPF
Cloned from https://raytracer.codeplex.com/

#Just added Reflections and Shadows to WPF . . . with a ray tracer


#Project Description
Ray Tracer built in C# with a User Interface built in WPF. The project has been a lot for educational purposes, and gain experience in C# and WPF.

#New Project
WPF's Viewport3D can now be translated into an Image that is RayTraced. If you want to check it out look at the Viewport3DRayTracer project. I'm pretty happy about it because I'm able to use the Transforms to manipulate a Viewport3D scene, and then ray trace it with my ray tracer. 

Here are some screen shots of the new project. There are 3 cubes in the scene (made from a MeshGeometry3D), all of them are transformed in some way, you can also move the scroll bars to change the position of the 2 blocks facing each other. The Image on the right is WPF's Viewport3D scene, the image on the left is the RayTraced Image. The first image has demonstrates reflection, where the other demonstrates shadows. 

WPFReflection.png
WPFRayTraced1.png

#Project Direction
I've added the capabilities to manipulate the scene then re-render it by the click of a button. The UI isn't pretty or anything but at least it shows everything that is part of the scene. The basic design of the ray tracer source code is in place where it is easy to add new materials, new shapes, new lights, and test C# performance. There are a few things that increased my performance dramatically which were simple C# coding. When people look at the code, I wouldn't mind hearing about certain things that would speed things up.

So far I haven't found a fast way to interactively update the buffer and watch the ray tracer fill in the pixels. I would really like this to be fast, but even updating the back buffer on the new WriteableBitmap directly is not fast per pixel. I thought maybe per row would be good enough, but it's not really worth the wait. 

Now there are a few shapes that are ray traced correctly. I've been messing with a lot of performance stuff in C#, and I've actually gotten this thing faster then my C++ ray tracer http://www.eng.utah.edu/~kmadsen. I've added a couple of threads to speed things up when you're on a multicore computer. The design of the multi-threading is not yet as elegant as I want it. I think I'll open a discussion, because I'd like to know why it won't speed things up if I change my Float3 class into a C# struct. 

At the bottom I show the ray tracer at work. I'm still touching up materials so there is a more wide range of colors you can give objects. At least everything is drawing correctly, multiple lights is working, reflections are working, and the shape intersections are working as well. I'm almost to the point to focus on converting a Viewport3D into a Ray Tracer scene.
#Current Features

##Primitives
1. Plane
2. Sphere
3. Triangle
4. Box
5. Disk
6. Ring

##Materials
1. Lambertian
2. Blinn-Phong: Reflective

##Lights
1. Point Light
2. Directional Light

##Cameras
1. Pinhole Camera
2. Orthographic Camera

##Performance Acceleration
1. Bounding Volume Hierarchy (needs more work, a little broken)
2. Who needs to anti-alias with multi sampling when a WPF Image will do it for you?

##Windows Presentation Foundation
1. WPF scene manipulation capabilities - You can basically edit the camera, and resolution.

#Features Coming Soon

1. Refraction - Dielectric Materials
2. Better Scene Manipulation!
3. Convert a WPF Viewport3D into a ray traced seen
4. Other acceleration structures
5. Read/Write Scene files
6. Sampling
7. A bunch of other junk...

allShapes.png
