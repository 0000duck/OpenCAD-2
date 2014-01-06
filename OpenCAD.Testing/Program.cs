using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCAD.Kernel.Features;
using OpenCAD.Kernel.Graphics;
using OpenCAD.Kernel.Maths;
using OpenCAD.Kernel.Modeling.Octree;
using OpenCAD.Kernel.References;

namespace OpenCAD.Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            var o = new OctreeModel(new OctreeNode(Vect3.Zero, 16, 8),"testing");

            var render = new TestModelRenderManager();

            //render.Fetch(o)


            //Console.WriteLine("Testing");
            //var features = new FeatureFactory();
            //var origin = new DatumOrigin();


            //features.Add(new DatumPlane(new IFeatureReference[] { new FeatureReference(origin) }));
            //features.Add(new DatumPlane(new IFeatureReference[] { new FeatureReference(origin) }));
            //features.Add(new DatumPlane(new IFeatureReference[] { new FeatureReference(origin) }));
            //features.Add(new DatumPlane(new IFeatureReference[] { new FeatureReference(origin) }));
            //features.Add(new DatumPlane(new IFeatureReference[] { new FeatureReference(origin) }));
            //features.Add(new DatumPlane(new IFeatureReference[] { new FeatureReference(origin) }));
            //features.Add(new DatumPlane(new IFeatureReference[] { new FeatureReference(origin) }));

            //features.Add(new DatumPlane(new IFeatureReference[] { new FeatureReference(origin) }));

            //features.Add(new DatumOrigin());
            //features.Add(origin);
            //Console.WriteLine("Running Regen");

            //features.RegenerateAsync();
            //Console.WriteLine("Run Regen");
            Console.ReadLine();
        }
    }

    public class TestModelRenderManager:BaseModelRenderManager
    {
        public override IRenderer Fetch(Type type)
        {
            Console.WriteLine(type);
            return null;
        }
    }
}
