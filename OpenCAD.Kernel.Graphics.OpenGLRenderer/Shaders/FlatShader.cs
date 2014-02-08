using System.Diagnostics;
using System.IO;
using OpenCAD.Kernel.Graphics.OpenGLRenderer.Buffers;
using SharpGL;
using SharpGL.SceneGraph.Shaders;

namespace OpenCAD.Kernel.Graphics.OpenGLRenderer
{
    public class FlatShader : ShaderBindable
    {

        public FlatShader(OpenGL gl)
            : base(gl)
        {
            _program = new ShaderProgram();
            var vertex = new VertexShader();
            var fragment = new FragmentShader();

            vertex.CreateInContext(gl);
            vertex.SetSource(File.ReadAllText("Shaders/Flat.vert.glsl"));
            vertex.Compile();

            fragment.CreateInContext(gl);
            fragment.SetSource(File.ReadAllText("Shaders/Flat.frag.glsl"));
            fragment.Compile();

            _program.CreateInContext(gl);
            _program.AttachShader(vertex);
            _program.AttachShader(fragment);
            _program.Link();

            Debug.WriteLine(_program.InfoLog);
            foreach (var attachedShader in _program.AttachedShaders)
            {
                Debug.WriteLine(attachedShader.InfoLog);
            }
        }
    }
}