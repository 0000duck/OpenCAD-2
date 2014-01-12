using System;
using System.Collections.Generic;
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
using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;
using Roslyn.Scripting.CSharp;

namespace OpenCAD.Kernel.Scripting
{
    public interface IPartScriptRunner
    {
        IModel Execute(IEnumerable<string> references, string code);
    }

    public class PartScriptRunner : IPartScriptRunner
    {

        public IModel Execute(IEnumerable<string> references, string code)
        {
            List<MetadataReference> refs = references.Select(MetadataReference.CreateAssemblyReference).ToList();
            refs.Add(new MetadataFileReference(typeof(IModel).Assembly.Location));
            var comp = Compilation.Create("OpenCADOutput", new CompilationOptions(OutputKind.DynamicallyLinkedLibrary), new[] { SyntaxTree.ParseText(code) }, refs);

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
            throw new Exception("error");
        }
    }
}
