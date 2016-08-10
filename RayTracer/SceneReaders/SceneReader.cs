using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer.SceneReaders
{
    public abstract class SceneReader
    {
        private Scene Scene;
        public abstract Scene ReadSceneFile();
    }
}
