using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenCAD.Kernel.Features;
using OpenCAD.Kernel.Graphics;
using OpenCAD.Kernel.Maths;
using OpenCAD.Kernel.Modeling;
using OpenCAD.Kernel.Modeling.Octree;
using OpenCAD.Kernel.References;
using OpenCAD.Kernel.Scripting;
using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;

namespace OpenCAD.Testing
{
    class Program
    {
        static void Main(string[] args)
        {

            var s = @"
using System;
using System.Collections.Generic;
using OpenCAD.Kernel.Maths;
using OpenCAD.Kernel.Modeling;
using OpenCAD.Kernel.Modeling.Octree;
using OpenCAD.Kernel.Primatives;
using OpenCAD.Kernel.Scripting;

namespace Test
{
    public class TestPart:IPartScript
    {
        public IEnumerable<IPartScriptOption> Options()
        {
            throw new NotImplementedException();
        }

        public IModel Generate()
        {
             return new OctreeModel(new OctreeNode(Vect3.Zero, 16, 5), String.Empty);
        }
    }
}
";

            var comp = Compilation.Create("test", new CompilationOptions(OutputKind.DynamicallyLinkedLibrary), new[] { SyntaxTree.ParseText(s) }, new[]
                                                  {

                                                      MetadataReference.CreateAssemblyReference("mscorlib"),
                                                      MetadataReference.CreateAssemblyReference("System"),
                                                      MetadataReference.CreateAssemblyReference("System.Core"),
                                                      MetadataReference.CreateAssemblyReference("Microsoft.CSharp"),
                                                      new MetadataFileReference(typeof(IModel).Assembly.Location)
                                                  });




            
            using (var output = new MemoryStream())
            {
                var emitResult = comp.Emit(output);
                foreach (var diagnostic in emitResult.Diagnostics)
                {
                    Console.WriteLine(diagnostic.ToString());
                }
                byte[] compiledAssembly = output.ToArray();
                var assembly = Assembly.Load(compiledAssembly);

                var types = assembly.GetTypes();
                var type= types.FirstOrDefault();
                if (type != null)
                {
                    dynamic model = assembly.CreateInstance(type.FullName);

                    var partScript = (IPartScript) model;

                    if (partScript != null)
                    {
                        var m = partScript.Generate();
                        Console.WriteLine(m);
                    }
                }

            }













//            var t = new CodeExecuter();




            

//            t.Execute(s);

            //var o = new OctreeModel(new OctreeNode(Vect3.Zero, 16, 8),"testing");

            //var render = new TestModelRenderManager();

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
