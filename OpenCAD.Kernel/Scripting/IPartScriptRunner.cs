using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using OpenCAD.Kernel.Modeling;
using OpenCAD.Kernel.Structure;
using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;
using Roslyn.Scripting.CSharp;

namespace OpenCAD.Kernel.Scripting
{
    public interface IScriptRunner
    {
        IModel Execute(IEnumerable<string> references, string code);
        void Execute(IProject project);
    }

    public class ScriptRunner : IScriptRunner
    {
        public void Execute(IProject project)
        {
            var refs = project.References.Select(MetadataReference.CreateAssemblyReference).ToList();
            refs.Add(new MetadataFileReference(typeof(IModel).Assembly.Location));

            var tree = project.Flatten().OfType<PartProjectItem>().Select(p => SyntaxTree.ParseText(p.Load()));

            var comp = Compilation.Create("OpenCADOutput", new CompilationOptions(OutputKind.DynamicallyLinkedLibrary), tree, refs);
            using (var output = new MemoryStream())
            {
                var emitResult = comp.Emit(output);
                foreach (var diagnostic in emitResult.Diagnostics)
                {
                    Debug.WriteLine(diagnostic.ToString());
                }
                if (!emitResult.Success) throw new Exception("failed build");
                byte[] compiledAssembly = output.ToArray();
                var assembly = Assembly.Load(compiledAssembly);

                var types = assembly.GetTypes();
                var type = types.FirstOrDefault();


            }
        }

        public IModel Execute(IEnumerable<string> references, string code)
        {
            try
            {
                List<MetadataReference> refs = references.Select(MetadataReference.CreateAssemblyReference).ToList();
                refs.Add(new MetadataFileReference(typeof(IModel).Assembly.Location));
                var comp = Compilation.Create("OpenCADOutput", new CompilationOptions(OutputKind.DynamicallyLinkedLibrary), new[] { SyntaxTree.ParseText(code) }, refs);

                using (var output = new MemoryStream())
                {
                    var emitResult = comp.Emit(output);
                    foreach (var diagnostic in emitResult.Diagnostics)
                    {
                        Debug.WriteLine(diagnostic.ToString());
                    }
                    if(!emitResult.Success) throw new Exception("failed build");
                    byte[] compiledAssembly = output.ToArray();
                    var assembly = Assembly.Load(compiledAssembly);

                    var types = assembly.GetTypes();
                    var type = types.FirstOrDefault();
                    if (type != null)
                    {
                        dynamic model = assembly.CreateInstance(type.FullName);

                        var partScript = (IPartScript)model;
                        if (partScript != null)
                        {
                            return partScript.Generate();
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return null;
        }
    }
}
